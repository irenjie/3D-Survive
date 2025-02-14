using MTLFramework.Core;
using UnityEngine;

namespace Survive3D.Player {

    public enum PlayerState {
        Idle,
        Move,
        Attack,
        Hurt,
        Dead
    }


    public abstract class PlayerStateBase : IFSMState {
        protected PlayerController playerController;

        public void Init(PlayerController playerController) {
            this.playerController = playerController;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Update();
    }
}
