

using System;

namespace MTLFramework.Helper {
    public class Timer {
        public object handle { get; private set; }
        public int ID { get; private set; }
        public float step { get; private set; } = 1f;
        public float delay { get; private set; }
        // delay后是否立即调用一次
        public bool callBeforeStep { get; private set; } = false;
        // 循环次数，-1无限循环
        public int loop { get; private set; } = 1;
        private Action onStep;
        private Action onComplete;
        // 帧计时器标记（尚未实现）
        public bool isFrameTimer { get; private set; }
        public bool isFinish { get; private set; } = false;

        // delay 是否完成
        private bool isDelayCompleted = false;
        // 触发间隔时间
        private float elapsedTime = 0f;
        private int triggerCount = 0;

        public Timer(object handle, int id, float step = 1f, float delay = 0f, int loop = 1, bool callbefoteStep = false, Action onStep = null, Action onComplete = null, bool isFameTimer = false) {
            this.handle = handle;
            this.ID = id;
            this.step = step;
            this.delay = delay;
            this.loop = loop;
            this.onStep = onStep;
            this.onComplete = onComplete;
            //this.isFinish = isFameTimer;
        }

        public void Update(float deltaTime) {
            if (isFinish)
                return;

            if (!isDelayCompleted) {
                delay -= deltaTime;
                if (delay <= 0f) {
                    isDelayCompleted = true;
                    if (callBeforeStep) {
                        onStep?.Invoke();
                        triggerCount++;
                    }
                    elapsedTime = 0f;
                } else
                    return;
            }

            elapsedTime += deltaTime;
            while (elapsedTime >= step && (loop == -1 || triggerCount < loop)) {
                triggerCount++;
                onStep?.Invoke();
                elapsedTime -= step;
            }

            if (loop >= 0 && loop <= triggerCount) {
                onComplete?.Invoke();
                isFinish = true;
            }
        }

        public void Stop() {
            isFinish = true;
        }
    }
}