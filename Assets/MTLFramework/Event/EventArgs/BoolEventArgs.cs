
namespace MTLFramework.Event {
    public class BoolEventArgs : GameEventArgs {
        public bool isPause { get; private set; }

        public BoolEventArgs(bool isPause) {
            this.isPause = isPause;
        }
    }
}
