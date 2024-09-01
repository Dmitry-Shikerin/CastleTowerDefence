using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects
{
    public class CollectorContainer<TCollector, TConfig> : ScriptableObject
        where TCollector : ConfigCollector<TConfig>
        where TConfig : Config
    {
        [Header("Data")]
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf("_enable", Enable.Enable)]
        [SerializeField] private List<TCollector> _collectors;
        [FolderPath(ParentFolder = "Assets/Resources")]
        [SerializeField] private string _collectorsPath;
        [TabGroup("Create")]
        [ValidateInput("ValidateId")]
        [SerializeField] private string _collectorId;
        [TabGroup("Remove")]
        [ValueDropdown(nameof(GetRemovedId))]
        [SerializeField] private string _removedCollectorId;

        public List<TCollector> Collectors => _collectors;

        public List<string> GetRemovedId() =>
            _collectors.Select(collector => collector.Id).ToList();
        
        [TabGroup("Remove")]
        [PropertySpace(10)]
        [Button]
        public void RemoveUpgradeConfig()
        {
#if UNITY_EDITOR
            TCollector collector = _collectors.FirstOrDefault(collector => collector.Id == _removedCollectorId);
            string path = AssetDatabase.GetAssetPath(collector);
            _collectors.Remove(collector);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
#endif
        }
        
        [UsedImplicitly]
        [PropertySpace(10)]
        [TabGroup("Create")]
        [Button]
        private void CreateUpgradeConfig()
        {
#if UNITY_EDITOR
            TCollector collector = CreateInstance<TCollector>();
            AssetDatabase.CreateAsset(collector, 
                $"Assets/Resources/{_collectorsPath}/{_collectorId}_{typeof(TCollector).Name}.asset");
            string path = AssetDatabase.GetAssetPath(collector);
            AssetDatabase.RenameAsset(path, $"{_collectorId}_{typeof(TCollector).Name}");
            collector.SetId(_collectorId);
            _collectors.Add(collector);
            AssetDatabase.SaveAssets();
#endif
        }

        private bool ValidateId() =>
            _collectors.Any(collector => collector.Id == _collectorId) == false;
    }
}