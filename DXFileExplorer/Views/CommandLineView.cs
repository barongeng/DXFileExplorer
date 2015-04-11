using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DXFileExplorer.Actions;
using DXFileExplorer.Utils;
using DXFileExplorer.Views.Controls.CommandLine;

namespace DXFileExplorer.Views {
    [ToolboxItem(false)]
    public partial class CommandLineView :BaseControl {
        public const char Pointer = '>';
        List<string> CommandStack = new List<string>();

        string fCommand;
        public string Command {
            get { return fCommand; }
        }

        BaseControlViewInfo fViewInfo;
        protected override BaseControlViewInfo ViewInfo {
            get {
                if (fViewInfo == null)
                    fViewInfo = new CommandLineViewInfo(this);
                return fViewInfo;
            }
        }

        CommandLinePainter fPainter;
        protected override BaseControlPainter Painter {
            get {
                if (fPainter == null)
                    fPainter = new CommandLinePainter();
                return fPainter;
            }
        }

        string fDisplayText;
        public string DisplayText {
            get { return fDisplayText; }
        }

        string fCurrentDirectory;
        public string CurrentDirectory {
            get { return fCurrentDirectory; }
        }

        CommandLineViewInfo ViewInfoCore {
            get { return (CommandLineViewInfo)ViewInfo; }
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    if (string.IsNullOrEmpty(Command)) return;
                    if (CommandStack.Contains(Command))
                        CommandStack.Remove(Command);
                    CommandStack.Add(Command);
                    RaiseExecuteCommand(Command.Trim());
                    break;
                case Keys.Back:
                    if (!string.IsNullOrEmpty(fCommand)) {
                        fCommand = fCommand.Substring(0, fCommand.Length - 1);
                        LayoutChanged();
                    }
                    break;
                default:
                    string val = KeyCodeToString(e.KeyValue, e.Shift);
                    if (string.IsNullOrEmpty(val)) return;
                    fCommand = string.Concat(Command, val);
                    LayoutChanged();
                    break;
            }
        }

        void RaiseExecuteCommand(string command) {
            EventHandler<ExecuteCommandEventArgs> handler =
                (EventHandler<ExecuteCommandEventArgs>)Events[fExecuteCommand];
            if (handler != null)
                handler(this, new ExecuteCommandEventArgs(command));
        }

        static readonly object fExecuteCommand = new object();
        public event EventHandler<ExecuteCommandEventArgs> ExecuteCommand {
            add { Events.AddHandler(fExecuteCommand, value); }
            remove { Events.RemoveHandler(fExecuteCommand, value); }
        }

        public void DisplayPrompt(string dir, bool newLine) {
            if (newLine) {
                if (!string.IsNullOrEmpty(CurrentDirectory))
                    fDisplayText = ViewInfo.DisplayText;
                fCommand = string.Empty;
            }
            fCurrentDirectory = dir;
            LayoutChanged();
        }

        string KeyCodeToString(int code, bool shift) {
            switch (code) {
                case 186: return shift ? ":" : ";";
                case 220: return shift ? "|" : @"\";
                case 53: return shift ? "%" : "5";
                case 189: return shift ? "=" : "-";
                default:
                    char c = (char)code;
                    if (char.IsLetterOrDigit(c) || c == ' ')
                        return shift ? c.ToString() : char.ToLowerInvariant(c).ToString();
                    return string.Empty;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e) {
             if (e.KeyCode == Keys.Up) {
                if (CommandStack.Count == 0) return;
                fCommand = CommandStack[0];
                CommandStack.RemoveAt(0);
                CommandStack.Add(Command);
                LayoutChanged();
            }
        }
    }
}
