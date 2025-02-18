using UnityEngine;

namespace Buff {
    [CreateAssetMenu(menuName = "BuffSystem/BuffData")]
    public class BuffData : ScriptableObject {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private string _iconAddress;
        [Header("优先级")]
        [SerializeField] private int _priority;
        [SerializeField] private int _maxStack;
        [Header("buff类型标签")]
        [SerializeField] private string[] _tags;
        [SerializeField] private bool _isForever;
        [SerializeField] private float _duration;
        [SerializeField] private float _tickTime;
        [SerializeField] private EBuffTimeUpdate _timeUpdateType;
        [SerializeField] private EBuffRemoveStackUpdate _removeStackUpdateType;

        [Header("回调点")]
        [SerializeField] private BaseBuffModule _onCreate;
        [SerializeField] private BaseBuffModule _onRemove;
        [SerializeField] private BaseBuffModule _onTick;
        [SerializeField] private BaseBuffModule _onHit;
        [SerializeField] private BaseBuffModule _beHit;
        [SerializeField] private BaseBuffModule _onTakeDamage;
        [SerializeField] private BaseBuffModule _onDealDamage;
        [SerializeField] private BaseBuffModule _onKill;
        [SerializeField] private BaseBuffModule _beKill;

        public int Id => _id;
        public string Name => _name;
        public string Description => _description;
        public string IconAddress => _iconAddress;
        public int Priority => _priority;
        public int MaxStack => _maxStack;
        public float Duration => _duration;
        public float TickTime => _tickTime;
        public EBuffTimeUpdate TimeUpdateType => _timeUpdateType;
        public EBuffRemoveStackUpdate RemoveStackUpdateType => _removeStackUpdateType;
        public BaseBuffModule OnCreate => _onCreate;
        public BaseBuffModule OnRemove => _onRemove;
        public BaseBuffModule OnTick => _onTick;
        public BaseBuffModule OnHit => _onHit;
        public BaseBuffModule BeHit => _beHit;
        public BaseBuffModule OnTakeDamage => _onTakeDamage;
        public BaseBuffModule OnDealDamage => _onDealDamage;
        public BaseBuffModule OnKill => _onKill;
        public BaseBuffModule BeKill => _beKill;
    }
}