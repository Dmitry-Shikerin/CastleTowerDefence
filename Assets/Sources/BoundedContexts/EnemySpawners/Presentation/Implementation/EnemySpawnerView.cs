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

        public IBunkerView BunkerView { get; private set; }
        public ICharacterMeleeView CharacterMeleeView { get; private set; }
        public IEnemySpawnPoint[,] SpawnPoints { get; private set; }

        private void Awake()
        {
            SpawnPoints = new IEnemySpawnPoint[2, 4];
             
            for (int i = 0; i < SpawnPoints.GetLength(0); i++)
            {
                for (int j = 0; j < SpawnPoints.GetLength(1); j++)
                {
                    SpawnPoints[i, j] = _enemySpawnPoints[i * 4 + j];
                }
            }
        }

        public void StartSpawn() =>
            Presenter.OnStartSpawn();

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