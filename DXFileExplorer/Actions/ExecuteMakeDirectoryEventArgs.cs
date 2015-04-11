namespace DXFileExplorer.Actions {
    public class ExecuteMakeDirectoryEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
