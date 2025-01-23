using MTLFramework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 状态机使用方法
 * 1. 创建状态总类继承 IFSMState
 * 2. 创建实际状态(继承状态总类)
 * 3. 不是必须，创建状态机(继承FSM<FSMTestState>),或者简单情况直接使用FSM
 * 4. 在实体中使用状态机
 */
namespace Test {

    public class FSMTest : MonoBehaviour {
        TestFSM fsm = new TestFSM();
        private void Awake() {
            fsm.AddState(new ARealState());
            fsm.ChangeState(Type.GetType(nameof(ARealState)));
        }
    }

    public class TestFSM : FSM<FSMTestState> {

    }


    public abstract class FSMTestState : IFSMState {
        public abstract void Enter();

        public abstract void Exit();

        public abstract void Update();
    }

    public class ARealState : FSMTestState {
        public override void Enter() {
            throw new NotImplementedException();
        }

        public override void Exit() {
            throw new NotImplementedException();
        }

        public override void Update() {
            throw new NotImplementedException();
        }
    }
}