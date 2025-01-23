namespace MTLFramework.Event {
    public class FloatEventArgs : GameEventArgs {
        public float floatArgs { get; private set; }

        public FloatEventArgs(float floatArgs) {
            this.floatArgs = floatArgs;
        }
    }
}
