using MTLFramework.Buff;
using UnityEngine;

namespace Buff {
    public abstract class BaseBuffModule : ScriptableObject {
        public abstract void Apply(BuffInfo buffInfo, DamageInfo damageInfo = null);
    }

}