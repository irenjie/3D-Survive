using Extensions;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Buff {
    public class BuffHandler : MonoBehaviour {
        // 按 buffData.Priority 正序
        private LinkedList<BuffInfo> buffList = new LinkedList<BuffInfo>();

        private void Update() {
            TickBuff();
        }

        public void AddBuff(BuffInfo buffInfo) {
            BuffInfo findBuff = FindBuff(buffInfo.buffData.Id);
            if (findBuff != null) {
                // buff 已存在
                if (findBuff.curStack < findBuff.buffData.MaxStack) {
                    findBuff.curStack++;
                }

                switch (findBuff.buffData.TimeUpdateType) {
                    case EBuffTimeUpdate.Add:
                        findBuff.durationTimer += findBuff.buffData.Duration;
                        break;
                    case EBuffTimeUpdate.Replace:
                        findBuff.durationTimer = findBuff.buffData.Duration;
                        break;
                    case EBuffTimeUpdate.Keep:
                        break;
                }

                findBuff.buffData.OnCreate?.Apply(findBuff);

            } else {
                buffInfo.Init(null, null);
                buffInfo.buffData.OnCreate?.Apply(buffInfo);
                int addPriority = buffInfo.buffData.Priority;
                buffList.AddBefore(buff => buff.buffData.Priority >= addPriority, buffInfo);
            }
        }

        public void RemoveBuff(int id) {
            var findBuff = FindBuff(id);
            if (findBuff == null)
                return;
            buffList.Remove(findBuff);
            findBuff.buffData.OnRemove?.Apply(findBuff);
        }


        List<BuffInfo> tmpTimeOutBuff = new List<BuffInfo>();
        public void TickBuff() {
            foreach (var buff in buffList) {
                if (buff.buffData.OnTick != null) {
                    if (buff.tickTimer <= 0) {
                        buff.buffData.OnTick.Apply(buff);
                        buff.tickTimer = buff.buffData.TickTime;
                    } else {
                        buff.tickTimer -= Time.deltaTime;
                    }
                }
                if (buff.durationTimer <= 0) {
                    tmpTimeOutBuff.Add(buff);
                } else {
                    buff.durationTimer -= Time.deltaTime;
                }
            }

            foreach (var buff in tmpTimeOutBuff) {
                buffList.Remove(buff);
                buff.buffData.OnRemove?.Apply(buff);
            }
        }

        private BuffInfo FindBuff(int buffId) {
            buffList.TryFind<BuffInfo>(buffInfo => buffInfo.buffData.Id == buffId, out BuffInfo result);
            return result;
        }

    }
}