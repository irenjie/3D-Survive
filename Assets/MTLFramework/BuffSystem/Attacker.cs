using UnityEngine;

namespace MTLFramework.Buff {
    public interface Attacker {
        public int ID { get;}

        public float GetAttackDamage();
    }
}