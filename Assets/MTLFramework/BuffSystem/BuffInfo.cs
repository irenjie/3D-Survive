using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buff {
    public class BuffInfo {
        public BuffData buffData;
        public GameObject creator { get; private set; }
        public GameObject target { get; private set; }
        public float durationTimer;
        public float tickTimer;
        public int curStack;

        public void Init(GameObject creator, GameObject target, bool tickOnInit = true, int stack = 1) {
            this.creator = creator;
            this.target = target;
            durationTimer = buffData.Duration;
            tickTimer = tickOnInit ? 0 : buffData.TickTime;
            curStack = stack;
        }
    }
}