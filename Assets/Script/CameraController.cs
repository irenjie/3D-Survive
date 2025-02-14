using MTLFramework.Helper;
using Unity.VisualScripting;
using UnityEngine;

namespace Survive3D {
    public class CameraController : MonoBehaviour {
        private Transform mTransform;
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;
        [SerializeField] float moveSpeed;
        [SerializeField] Camera mCamera;

        public new Camera camera => mCamera;

        private Vector2 xScope;     // X·¶Î§
        private Vector2 zScope;     // Z·¶Î§

        public void Init(float mapSizeOnWorld) {
            mTransform = transform;

            xScope = new Vector2(5, mapSizeOnWorld - 5);
            zScope = new Vector2(-1, mapSizeOnWorld - 5);

        }

        private void LateUpdate() {
            if (target == null)
                return;

            Vector3 targetPos = target.position + offset;
            targetPos.x = Mathf.Clamp(targetPos.x, xScope.x, xScope.y);
            targetPos.z = Mathf.Clamp(targetPos.z, zScope.x, zScope.y);
            mTransform.position = Vector3.Lerp(mTransform.position, targetPos, UnityEngine.Time.deltaTime * moveSpeed);
        }
    }
}