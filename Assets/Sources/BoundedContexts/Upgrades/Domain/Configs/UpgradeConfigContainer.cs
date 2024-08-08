using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Upgrades.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "UpgradeConfig",
        menuName = "Configs/UpgradeConfigContainer",
        order = 51)]
    public class UpgradeConfigContainer : ScriptableObject
    {
        [Header("Data")]
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private List<UpgradeConfig> _upgradeConfigs;
        [Header("CreateConfig")] [ValidateInput("ValidateId")]
        [SerializeField] private string _upgradeId;

        public IReadOnlyList<UpgradeConfig> UpgradeConfigs => _upgradeConfigs;

        public void RemoveUpgradeConfig(UpgradeConfig upgradeConfig)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(upgradeConfig);
            _upgradeConfigs.Remove(upgradeConfig);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
#endif
        }
        
        [UsedImplicitly]
        [ResponsiveButtonGroup("Buttons")]
        private void CreateUpgradeConfig()
        {
#if UNITY_EDITOR
            UpgradeConfig upgradeConfig = CreateInstance<UpgradeConfig>();
            AssetDatabase.CreateAsset(upgradeConfig, UpgradeConst.UpgradeConfigAssetPath);
            string path = AssetDatabase.GetAssetPath(upgradeConfig);
            AssetDatabase.RenameAsset(path, $"{_upgradeId}_UpgradeConfig");
            upgradeConfig.SetParent(this);
            upgradeConfig.SetUpgradeId(_upgradeId);
            _upgradeConfigs.Add(upgradeConfig);
            AssetDatabase.SaveAssets();
#endif
        }

        private bool ValidateId() =>
            _upgradeConfigs.Any(upgradeConfig => upgradeConfig.Id == _upgradeId) == false;
    }
}