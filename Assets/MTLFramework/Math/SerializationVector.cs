
using Unity.VisualScripting;
using UnityEngine;


namespace MTLFramework.Math {
    public struct SerializationVector2 {
        public float x, y;
        public SerializationVector2(float x, float y) => (this.x, this.y) = (x, y);
        public override string ToString() {
            return $"({x},{y})";
        }
    }

    public struct SerializationVector3 {
        public float x, y, z;
        public SerializationVector3(float x, float y, float z) => (this.x, this.y, this.z) = (x, y, z);
        public override string ToString() {
            return $"({x},{y},{z})";
        }

        public Vector3 ConverToVector3() {
            return new Vector3(x, y, z);
        }
    }

    public static class SerializationVectorExtensions {
        public static SerializationVector3 ConverToSVector3(this Vector3 v) {
            return new SerializationVector3(v.x, v.y, v.z);
        }
    }

}