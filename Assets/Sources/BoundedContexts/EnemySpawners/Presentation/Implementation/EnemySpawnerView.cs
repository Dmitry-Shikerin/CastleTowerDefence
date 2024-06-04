using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class EnemySpawnerView : PresentableView<EnemySpawnerPresenter>, IEnemySpawnerView, ISelfValidator
    {
        [ChildGameObjectsOnly]
        [SerializeField] private List<EnemySpawnPoint> _enemySpawnPoints;
        [ChildGameObjectsOnly]

        public IReadOnlyList<IEnemySpawnPoint> SpawnPoints => _enemySpawnPoints;
        public IBunkerView BunkerView { get; private set; }
        public ICharacterMeleeView CharacterMeleeView { get; private set; }
        
        public void SetCharacterView(ICharacterMeleeView characterMeleeView) =>
            CharacterMeleeView = characterMeleeView;

        public void SetBunkerView(IBunkerView bunkerView) =>
            BunkerView = bunkerView ?? throw new ArgumentNullException(nameof(bunkerView));

        public void Validate(SelfValidationResult result) =>
            _enemySpawnPoints.ValidateSpawnPoints(SpawnPointType.Enemy, result);

        [Button]
        private void AddEnemySpawnPoints() =>
            _enemySpawnPoints = this.GetSpawnPoints(SpawnPointType.Enemy);
    }
}