using Survive3D.Player;
using UnityEngine;

namespace Survive3D.Player {
    public class PlayerMoveState : PlayerStateBase {


        public override void Enter() {
            playerController.PlayAnimation(nameof(PlayerState.Move));
        }

        public override void Exit() {

        }

        public override void Update() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            // 检查输入值
            if (h == 0 && v == 0) {
                playerController.ChangeState(PlayerState.Idle);
                return;
            }


            Vector3 inputDir = new Vector3(h, 0, v);
            Quaternion targetQua = Quaternion.LookRotation(inputDir);
            playerController.playerTransform.localRotation = Quaternion.Slerp(playerController.playerTransform.localRotation, targetQua, UnityEngine.Time.deltaTime * playerController.rotateSpeed);

            // 检查地图边界
            if ((playerController.playerTransform.position.x < playerController.positionXScope.x && h < 0)
                || (playerController.playerTransform.position.x > playerController.positionXScope.y) && h > 0)
                inputDir.x = 0;

            if ((playerController.playerTransform.position.z < playerController.positionZScope.x && v < 0)
                || (playerController.playerTransform.position.z > playerController.positionZScope.y) && v > 0)
                inputDir.z = 0;

            playerController.characterController.Move(UnityEngine.Time.deltaTime * playerController.moveSpeed * inputDir);

        }
    }
}