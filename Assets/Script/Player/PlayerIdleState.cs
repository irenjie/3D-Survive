using Survive3D.Player;
using UnityEngine;

namespace Survive3D.Player {
    public class PlayerIdleState : PlayerStateBase {

        public override void Enter() {
            playerController.PlayAnimation(nameof(PlayerState.Idle));
        }

        public override void Exit() {
        }

        public override void Update() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h != 0 || v != 0)
                playerController.ChangeState(PlayerState.Move);
        }
    }
}
