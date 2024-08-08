using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Domain.Configs
{
    public class UpgradeConfig : ScriptableObject
    {
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private UpgradeConfigContainer _parent;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private string _upgradeId;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private List<UpgradeLevel> _upgradeLevels;

        public string Id => _upgradeId;

        public List<UpgradeLevel> Levels => _upgradeLevels;
        
        public void RemoveLevel(UpgradeLevel wave)
        {
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(wave);
            _upgradeLevels.Remove(wave);
            RenameWaves();
            AssetDatabase.SaveAssets();
#endif
        }

        public void SetParent(UpgradeConfigContainer upgradeConfigContainer) =>
            _parent = upgradeConfigContainer ?? throw new NullReferenceException("Parent is null");

        private void RenameWaves()
        {
#if UNITY_EDITOR
            for (int i = 0; i < _upgradeLevels.Count; i++)
            {
                _upgradeLevels[i].name = $"Level_{i + 1}";
                _upgradeLevels[i].SetLevelId(i + 1);
            }
            
            AssetDatabase.SaveAssets();
#endif
        }

        [UsedImplicitly]
        [ResponsiveButtonGroup("Buttons")]
        private void CreateLevel()
        {
#if UNITY_EDITOR
            UpgradeLevel level = CreateInstance<UpgradeLevel>();
            int levelId = _upgradeLevels.Count + 1;
            level.SetParent(this);
            AssetDatabase.AddObjectToAsset(level, this);
            level.SetLevelId(levelId);
            level.name = $"Level_{levelId}";
            _upgradeLevels.Add(level);
            AssetDatabase.SaveAssets();
#endif
        }

        [PropertySpace(20)]
        [Button(ButtonSizes.Medium)]
        private void RemoveConfig() =>
            _parent.RemoveUpgradeConfig(this);

        public void SetUpgradeId(string upgradeId) =>
            _upgradeId = upgradeId;
    }
}