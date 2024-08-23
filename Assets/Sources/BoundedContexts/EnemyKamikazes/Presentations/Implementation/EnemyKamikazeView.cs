using System;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation
{
    public class EnemyKamikazeView : EnemyViewBase, IEnemyKamikazeView
    {
        [Required] [SerializeField] private EnemyAnimation _animation;
        [Range(1, 5)]
        [Required] [SerializeField] private float _findRange;

        public IEnemyAnimation Animation => _animation;
        
        public ICharacterSpawnPoint CharacterMeleePoint { get; private set; }
        public float FindRange => _findRange;
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