namespace MTLFramework.Event {
    public class IntEventArgs : GameEventArgs {
        public int intArgs { get; private set; }

        public IntEventArgs(int intArgs) {
            this.intArgs = intArgs;
        }
    }
}