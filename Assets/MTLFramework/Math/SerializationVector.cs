

namespace MTLFramework.Math {
    public struct SerializationVector2 {
        public float x, y;
        public SerializationVector2(float x, float y) => (this.x, this.y) = (x, y);
        public override string ToString() {
            return $"({x},{y})";
        }
    }
}