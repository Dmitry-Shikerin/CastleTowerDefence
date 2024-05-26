using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "EnemySpawnerConfigContainer", 
        menuName = "Configs/EnemySpawnerConfigContainer",
        order = 51)]
    public class EnemySpawnerConfigContainer : ScriptableObject
    {
        [SerializeField] private List<EnemySpawnerWave> _waves;
        
        public IReadOnlyList<EnemySpawnerWave> Waves => _waves;
        
        private void RenameAsset(Object asset, string newName)
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(path, newName);
#endif
        }

        [Button(ButtonSizes.Medium)]
        private void AddWaves() =>
            _waves = FindAssets<EnemySpawnerWave>("t:EnemySpawnerWave");
        
        [Button(ButtonSizes.Medium)]
        private void CreateWave()
        {
#if UNITY_EDITOR
            EnemySpawnerWave phrase = CreateInstance<EnemySpawnerWave>();
            
            AssetDatabase.CreateAsset(phrase, 
                "Assets/Resources/Configs/EnemySpawnerWaves/Waves/EnemySpawnerWave.asset");
            int waveId = _waves.Count + 1;
            RenameAsset(phrase, $"Wave_{waveId}");
            phrase.SetWaveId(waveId);
            _waves.Add(phrase);
            AssetDatabase.SaveAssets();
#endif
        }

        private static List<T> FindAssets<T>(string assetName) where T : Object
        {
            return AssetDatabase
                .FindAssets(assetName)
                .Select(path => AssetDatabase.GUIDToAssetPath(path))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .ToList();
        }
    }
}