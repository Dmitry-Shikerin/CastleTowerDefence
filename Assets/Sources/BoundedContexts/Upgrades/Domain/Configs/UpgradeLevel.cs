using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Domain.Configs
{
    public class UpgradeLevel : ScriptableObject
    {
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private int _id;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private UpgradeConfig _parent;
        [Space(10)]
        [SerializeField] private int _moneyPerUpgrade;
        [SerializeField] private float _currentAmount;

        public int MoneyPerUpgrade => _moneyPerUpgrade;
        public float CurrentAmount => _currentAmount;
        public int Id => _id;

        public void SetLevelId(int id) =>
            _id = id;
        
        public void SetParent(UpgradeConfig parent) =>
            _parent = parent;

        [PropertySpace(7)]
        [Button(ButtonSizes.Medium)]
        private void Remove() =>
            _parent.RemoveLevel(this);
    }
}