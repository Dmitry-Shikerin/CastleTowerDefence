using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyView : EnemyViewBase, IEnemyView
    {
        [Required] [SerializeField] private EnemyDependencyProvider _provider;
        [Required] [SerializeField] private EnemyAnimation _animation;

        public IEnemyAnimation Animation => _animation;
        public EnemyDependencyProvider Provider => _provider;
        public ICharacterSpawnPoint CharacterMeleePoint { get; private set; }
        public ICharacterSpawnPoint CharacterRangePoint { get; private set; }

        public void SetTargetFollow(ICharacterHealthView characterViewHealthView)
        {
            throw new System.NotImplementedException();
        }

        public void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint) =>
            CharacterMeleePoint = spawnPoint ?? throw new ArgumentNullException(nameof(spawnPoint));

        public void SetCharacterRangePoint(ICharacterSpawnPoint spawnPoint) =>
            CharacterRangePoint = spawnPoint ?? throw new ArgumentNullException(nameof(spawnPoint));
    }
}