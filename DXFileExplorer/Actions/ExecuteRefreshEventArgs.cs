namespace DXFileExplorer.Actions {
    public class ExecuteRefreshEventArgs :ExecuteActionEventArgs {
        public override void Accept(IActionVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
