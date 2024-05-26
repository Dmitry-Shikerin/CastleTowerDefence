using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentationinterfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation
{
    public class EnemySpawnerView : PresentableView<EnemySpawnerPresenter>, IEnemySpawnerView, ISelfValidator
    {
        [ChildGameObjectsOnly]
        [SerializeField] private List<SpawnPoint> _enemySpawnPoints;

        public IReadOnlyList<SpawnPoint> SpawnPoints => _enemySpawnPoints;
        public ICharacterView CharacterView { get; set; }
        
        public void SetCharacterView(ICharacterView characterView) =>
            CharacterView = characterView;

        public void Validate(SelfValidationResult result) =>
            _enemySpawnPoints.ValidateSpawnPoints(SpawnPointType.Enemy, result);

        [Button]
        private void AddEnemySpawnPoints() =>
            _enemySpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.Enemy);
    }
}