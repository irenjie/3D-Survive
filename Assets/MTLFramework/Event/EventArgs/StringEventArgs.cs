namespace MTLFramework.Event {
    public class StringEventArgs : GameEventArgs {
        public string args { get; private set; }
        public StringEventArgs(string args) {
            this.args = args;
        }
    }
}