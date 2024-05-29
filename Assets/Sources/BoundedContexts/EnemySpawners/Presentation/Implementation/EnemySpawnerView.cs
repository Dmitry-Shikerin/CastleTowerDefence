using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.BoundedContexts.TargetPoints.Presentation.Implementation;
using Sources.BoundedContexts.TargetPoints.Presentation.Implementation.Types;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class EnemySpawnerView : PresentableView<EnemySpawnerPresenter>, IEnemySpawnerView, ISelfValidator
    {
        [ChildGameObjectsOnly]
        [SerializeField] private List<SpawnPoint> _enemySpawnPoints;
        [ChildGameObjectsOnly]
        [SerializeField] private TargetPoint _targetPoint;

        public IReadOnlyList<SpawnPoint> SpawnPoints => _enemySpawnPoints;
        public ITargetPoint TargetPoint => _targetPoint;
        public ICharacterMeleeView CharacterMeleeView { get; private set; }
        
        public void SetCharacterView(ICharacterMeleeView characterMeleeView) =>
            CharacterMeleeView = characterMeleeView;

        public void Validate(SelfValidationResult result)
        {
            _enemySpawnPoints.ValidateSpawnPoints(SpawnPointType.Enemy, result);
            
            if(_targetPoint.Type != TargetPointType.Enemy)
                result.AddError($"TargetPoint type must be {TargetPointType.Enemy}");
        }

        [Button]
        private void AddEnemySpawnPoints() =>
            _enemySpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.Enemy);
    }
}