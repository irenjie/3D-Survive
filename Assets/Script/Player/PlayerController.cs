using MTLFramework.Core;
using MTLFramework.Event;
using Survive3D.Data;
using System;
using UnityEngine;

namespace Survive3D.Player {
    public class PlayerController : MonoBehaviour {
        Animator animator;
        FSM<PlayerStateBase> stateMachine;
        public CharacterController characterController { get; private set; }
        public Transform playerTransform { get; private set; }
        private PlayerModel playerModel;
        PlayerConfig playerConfig;

        #region 玩家数值
        private float health;
        private float hungry;
        [SerializeField] private float mMoveSpeed;
        [SerializeField] private float mRotateSpeed;
        public float moveSpeed => mMoveSpeed;
        public float rotateSpeed => mRotateSpeed;
        // 玩家移动边界
        public Vector2 positionXScope { get; private set; }
        public Vector2 positionZScope { get; private set; }
        #endregion


        private void Awake() {
            animator = GetComponent<Animator>();
            playerTransform = transform;
            characterController = GetComponent<CharacterController>();
            playerModel = GetComponent<PlayerModel>();
            playerModel.Init();
        }

        private void Start() {
            playerConfig = PlayerConfig.Get();
            health = playerConfig.MaxHp;
            hungry = playerConfig.MaxHungry;
            TriggerHPUpdateEvent();
            TriggerHungryUpdateEvent();
        }

        public void Init(float mapSizeOnWorld) {
            positionXScope = new Vector2(1, mapSizeOnWorld - 1);
            positionZScope = new Vector2(1, mapSizeOnWorld - 1);

            #region 初始化状态机
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
            UpdateHungry();
        }

        #region 状态与动画
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

        #region 玩家数值
        private void UpdateHungry() {
            if (hungry > 0) {
                hungry -= Time.deltaTime * playerConfig.HungryReduceSpeed;
                hungry = MathF.Max(hungry, 0);
                TriggerHungryUpdateEvent();
            } else {
                if (health > 0) {
                    health -= Time.deltaTime * playerConfig.HpReduceSpeedOnHungryIsZero;
                    health = MathF.Max(health, 0);
                    TriggerHPUpdateEvent();
                }
            }
        }

        void TriggerHPUpdateEvent() {
            EventManager.Get().Fire(this, EventID.PlayerHPUpdate, new FloatEventArgs(health));
        }

        void TriggerHungryUpdateEvent() {
            EventManager.Get().Fire(this, EventID.PlayerHungryUpdate, new FloatEventArgs(hungry));
        }

        #endregion
    }
}