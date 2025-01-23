using MTLFramework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * ״̬��ʹ�÷���
 * 1. ����״̬����̳� IFSMState
 * 2. ����ʵ��״̬(�̳�״̬����)
 * 3. ���Ǳ��룬����״̬��(�̳�FSM<FSMTestState>),���߼����ֱ��ʹ��FSM
 * 4. ��ʵ����ʹ��״̬��
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