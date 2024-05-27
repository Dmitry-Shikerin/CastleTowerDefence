﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentationinterfaces;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
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
        public ICharacterMeleeView CharacterMeleeView { get; set; }
        
        public void SetCharacterView(ICharacterMeleeView characterMeleeView) =>
            CharacterMeleeView = characterMeleeView;

        public void Validate(SelfValidationResult result) =>
            _enemySpawnPoints.ValidateSpawnPoints(SpawnPointType.Enemy, result);

        [Button]
        private void AddEnemySpawnPoints() =>
            _enemySpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.Enemy);
    }
}