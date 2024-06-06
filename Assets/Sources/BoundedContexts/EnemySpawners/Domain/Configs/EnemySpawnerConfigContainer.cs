using System.Collections.Generic;
using JetBrains.Annotations;
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

        public void RemoveWave(EnemySpawnerWave wave)
        {
            AssetDatabase.RemoveObjectFromAsset(wave);
            _waves.Remove(wave);
            AssetDatabase.SaveAssets();
        }
        
        [UsedImplicitly]
        [ButtonGroup("Buttons")]
        [ResponsiveButtonGroup("Buttons/Buttons")]
        private void RenameWaves()
        {
#if UNITY_EDITOR
            for (int i = 0; i < _waves.Count; i++)
            {
                _waves[i].name = $"Wave_{i + 1}";
                _waves[i].SetWaveId(i + 1);
            }
            
            AssetDatabase.SaveAssets();
#endif
        }
        
        [UsedImplicitly]
        [ResponsiveButtonGroup("Buttons/Buttons")]
        private void CreateWave()
        {
#if UNITY_EDITOR
            EnemySpawnerWave wave = CreateInstance<EnemySpawnerWave>();
            int waveId = _waves.Count + 1;
            wave.Parent = this;
            AssetDatabase.AddObjectToAsset(wave, this);
            wave.SetWaveId(waveId);
            wave.name = $"Wave_{waveId}";
            _waves.Add(wave);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}