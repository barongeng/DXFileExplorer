namespace DXFileExplorer.Actions {
    public class ExecuteRenameEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
