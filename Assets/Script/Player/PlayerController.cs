using MTLFramework.Core;
using System;
using UnityEngine;

namespace Survive3D.Player {
    public class PlayerController : MonoBehaviour {
        Animator animator;
        FSM<PlayerStateBase> stateMachine;
        public CharacterController characterController { get; private set; }
        public Transform playerTransform { get; private set; }

        #region �����ֵ
        [SerializeField] private float mMoveSpeed;
        [SerializeField] private float mRotateSpeed;
        public float moveSpeed => mMoveSpeed;
        public float rotateSpeed => mRotateSpeed;
        // ����ƶ��߽�
        public Vector2 positionXScope { get; private set; }
        public Vector2 positionZScope { get; private set; }
        #endregion


        private void Awake() {
            animator = GetComponent<Animator>();
            playerTransform = transform;
            characterController = GetComponent<CharacterController>();

        }

        public void Init(float mapSizeOnWorld) {
            positionXScope = new Vector2(1, mapSizeOnWorld - 1);
            positionZScope = new Vector2(1, mapSizeOnWorld - 1);

            #region ��ʼ��״̬��
            stateMachine = new FSM<PlayerStateBase>();
            PlayerIdleState idleState = new PlayerIdleState();
            PlayerMoveState movingState = new PlayerMoveState();
            idleState.Init(this);
            movingState.Init(this);
            stateMachine.AddState(idleState);
            stateMachine.AddState(movingState);
            ChangeState(PlayerState.Idle);
            #endregion
        }

        // Update is called once per frame
        void Update() {
            stateMachine.OnUpdate();
            //transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * UnityEngine.Time.deltaTime * 8f);
        }

        #region ״̬�붯��
        public void PlayAnimation(string name, float fixedTime = 0.25f) {
            animator.CrossFadeInFixedTime(name, fixedTime);
        }

        public void ChangeState(PlayerState state) {
            switch (state) {
                case PlayerState.Idle:
                    stateMachine.ChangeState(typeof(PlayerIdleState));
                    break;
                case PlayerState.Move:
                    stateMachine.ChangeState(typeof(PlayerMoveState));
                    break;


            }
        }
        #endregion

    }
}