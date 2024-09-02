using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "EnemySpawnerConfigContainer",
        menuName = "Configs/EnemySpawnerConfig",
        order = 51)]
    public class EnemySpawnerConfig : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private List<EnemySpawnerWave> _waves;
        [Header("Enemy")]
        [SerializeField] private int _startEnemyAttackPower;
        [SerializeField] private int _addedEnemyAttackPower;
        [Space(5)]
        [SerializeField] private int _startEnemyHealth;
        [SerializeField] private int _addedenemyHealth;
        [Header("Boss")] 
        [SerializeField] private int _startBossAttackPower;
        [SerializeField] private int _addedBossAttackPower;
        [Space(5)]
        [SerializeField] private int _startbossMassAttackPower;
        [SerializeField] private int _addedBossMassAttackPower;
        [Space(5)]
        [SerializeField] private int _startBossHealth;
        [SerializeField] private int _addedBossHealth;
        [Header("Kamikaze")] 
        [SerializeField] private int _startKamikazeHealth;
        [SerializeField] private int _addedKamikazeHealth;
        [Space(5)]
        [SerializeField] private int _startKamikazeAttackPower;
        [SerializeField] private int _addedKamikazeAttackPower;
        [Space(5)]
        [SerializeField] private int _startKamikazeMassAttackPower;
        [SerializeField] private int _addedKamikazeMassAttackPower;
        
        //Enemy
        public int StartEnemyAttackPower => _startEnemyAttackPower;
        public int AddedEnemyAttackPower => _addedEnemyAttackPower;
        public int StartEnemyHealth => _startEnemyHealth;
        public int AddedEnemyHealth => _addedenemyHealth;
        
        //Boss
        public int StartBossAttackPower => _startBossAttackPower;
        public int AddedBossAttackPower => _addedBossAttackPower;
        public int StartBossMassAttackPower => _startbossMassAttackPower;
        public int AddedBossMassAttackPower => _addedBossMassAttackPower;
        public int StartBossHealth => _startBossHealth;
        public int AddedBossHealth => _addedBossHealth;
        
        //Kamikaze
        public int StartKamikazeMassAttackPower => _startKamikazeMassAttackPower;
        public int AddedKamikazeMassAttackPower => _addedKamikazeMassAttackPower;
        public int StartKamikazeAttackPower => _startKamikazeAttackPower;
        public int AddedKamikazeAttackPower => _addedKamikazeAttackPower;
        public int StartKamikazeHealth => _startKamikazeHealth;
        public int AddedKamikazeHealth => _addedKamikazeHealth;
        
        public IReadOnlyList<EnemySpawnerWave> Waves => _waves;

        //TODO добавить стратегии для спавна
        public void RemoveWave(EnemySpawnerWave wave)
        {
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(wave);
            _waves.Remove(wave);
            RenameWaves();
            AssetDatabase.SaveAssets();
#endif
        }
        
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
        [ResponsiveButtonGroup("Buttons")]
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