
namespace MTLFramework.Event {
    public class BoolEventArgs : GameEventArgs {
        public bool status { get; private set; }

        public BoolEventArgs(bool status) {
            this.status = status;
        }
    }
}
