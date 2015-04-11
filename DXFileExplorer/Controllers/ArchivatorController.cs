using System.Resources;
using System.Windows.Forms;
using DXFileExplorer.Properties;
using DXFileExplorer.Utils;
using DXFileExplorer.Views;
using System.Linq;
using System.IO;
using SharpCompress.Archive.Zip;
using SharpCompress.Reader;
using DXFileExplorer.Models;
using SharpCompress.Common;
using SharpCompress.Archive;
using SharpCompress.Archive.Rar;
using SharpCompress.Archive.SevenZip;

namespace DXFileExplorer.Controllers {
    public class ArchivatorController :BaseController {
        const string ItemCompressName = "Default_Archivator_Compress";
        const string ItemDecompressName = "Default_Archivator_Decompress";

        public ArchivatorController(IControllerManager manager)
            : base(manager) {
            Manager.AddCommand(new CreateCommandArgs() {
                GroupText = Resources.DefaultArchivatorGroupText, PageText = EditPageText,
                Info = new CommandInfo() {
                    Callback = t => {
                        using (ZipArchive archive = ZipArchive.Create()) {
                            FilesAdder.AddFiles(archive, Manager.CurrentDirectory, Manager.GetSelectedFiles().Select(i => {
                                switch (i.ItemType) {
                                    case FileSystemItemType.Directory: return new DirectoryInfo(Path.Combine(Manager.CurrentDirectory, i.Name));
                                    case FileSystemItemType.File: return new FileInfo(Path.Combine(Manager.CurrentDirectory, i.Name));
                                    default: return (FileSystemInfo)null;
                                }
                            }).Where(i => i != null).ToArray());
                            using (Stream stream = File.Create(Path.Combine(Manager.CurrentDirectory, string.Concat(Path.GetFileName(Manager.CurrentDirectory), ".zip")))) {
                                archive.SaveTo(stream, new CompressionInfo());
                            }
                        }
                        Manager.CurrentDirectory = Manager.CurrentDirectory;
                    },
                    CommandName = ItemCompressName,
                    CommandText = Resources.DefaultArchivatorItemCompress,
                    Key = Keys.Shift | Keys.F1
                }
            });
            Manager.AddCommand(new CreateCommandArgs() {
                GroupText = Resources.DefaultArchivatorGroupText, PageText = EditPageText,
                Info = new CommandInfo() {
                    Callback = t => {
                        Manager.GetSelectedFiles().ToList().ForEach(i => {
                            if (i.ItemType == FileSystemItemType.File) {
                                switch (Path.GetExtension(i.Name).ToUpperInvariant()) {
                                    case ".ZIP":
                                        using (IArchive archive = ZipArchive.Open(Path.Combine(Manager.CurrentDirectory, i.Name))) {
                                            archive.ExtractAllEntries().WriteAllToDirectory(Manager.CurrentDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                                        }
                                        break;
                                    case ".RAR":
                                        using (IArchive archive = RarArchive.Open(Path.Combine(Manager.CurrentDirectory, i.Name))) {
                                            archive.ExtractAllEntries().WriteAllToDirectory(Manager.CurrentDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                                        }
                                        break;
                                    case ".7Z":
                                        using (IArchive archive = SevenZipArchive.Open(Path.Combine(Manager.CurrentDirectory, i.Name))) {
                                            archive.ExtractAllEntries().WriteAllToDirectory(Manager.CurrentDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                                        }
                                        break;
                                }
                            }
                        });
                        Manager.CurrentDirectory = Manager.CurrentDirectory;
                    },
                    CommandName = ItemDecompressName,
                    CommandText = Resources.DefaultArchivatorItemDecompress,
                    Key = Keys.Shift | Keys.F2
                }
            });
        }

        protected override string SettingsName {
            get { return "Default_Archivator"; }
        }

        protected override void OnCurrentDirectoryChanged() { }

        protected override void RestoreSetting(params string[] args) { }

        protected override void Dispose(bool disposing) {
            Manager.RemoveCommand(ItemCompressName);
            Manager.RemoveCommand(ItemDecompressName);
            base.Dispose(disposing);
        }

        class FilesAdder {
            string Root;
            ZipArchive Archive;

            FilesAdder() { }

            public static void AddFiles(ZipArchive archive, string root, params FileSystemInfo[] entries) {
                FilesAdder adder = new FilesAdder() { Archive = archive, Root = root };
                adder.AddFiles(entries);
                adder.Archive = null;
            }

            void AddFiles(params FileSystemInfo[] entries) {
                entries.ToList().ForEach(i => {
                    DirectoryInfo di = i as DirectoryInfo;
                    FileInfo fi = i as FileInfo;
                    if (di != null) AddFiles(di.GetFileSystemInfos());
                    else Archive.AddEntry(fi.FullName.Replace(Root, string.Empty), fi);
                });
            }
        }
    }
}
