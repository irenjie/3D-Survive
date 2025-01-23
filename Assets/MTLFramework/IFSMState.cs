using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTLFramework.Core {
    public interface IFSMState {
        void Enter();
        void Update();
        void Exit();
    }

    public class FSM<T> where T:IFSMState {
        Dictionary<Type, T> stateTable = new Dictionary<Type, T>();
        public T curState = default;

        public void AddState(T state) {
            stateTable.TryAdd(state.GetType(), state);
        }

        public void ChangeState(System.Type nextStateType) {
            curState?.Exit();
            if (stateTable.TryGetValue(nextStateType, out curState)) {
                curState.Enter();
            }
        }

        public void OnUpdate() {
            curState?.Update();
        }
    }
}