﻿using Sirenix.OdinInspector;
using Sources.BoundedContexts.Abilities.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Huds;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.Huds.Presentations
{
    public class GameplayHud : MonoBehaviour, IHud
    {
        [FoldoutGroup("UiFramework")]
        [Required] [SerializeField] private UiCollector _uiCollector;
        
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _nukeAbilityApplier;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _spawnAbilityApplier;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private AbilityApplierView _flamethrowerAbilityApplier;

        [FoldoutGroup("UiFramework")] 
        [Required] [SerializeField] private EnemySpawnerUi _enemySpawnerUi;

        public UiCollector UiCollector => _uiCollector;

        public AbilityApplierView NukeAbilityApplier => _nukeAbilityApplier;
        public AbilityApplierView SpawnAbilityApplier => _spawnAbilityApplier;
        public AbilityApplierView FlamethrowerAbilityApplier => _flamethrowerAbilityApplier;
        
        public EnemySpawnerUi EnemySpawnerUi => _enemySpawnerUi;
    }
}