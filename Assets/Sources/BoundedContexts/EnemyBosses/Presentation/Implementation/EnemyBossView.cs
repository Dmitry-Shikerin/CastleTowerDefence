using System;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Implementation
{
    public class EnemyBossView : EnemyViewBase, IEnemyBossView
    {
        [FormerlySerializedAs("bossEnemyAnimation")] [Required] [SerializeField] private EnemyBossAnimation enemyBossAnimation;
        [Required] [SerializeField] private ParticleSystem _massAttackParticle;
        [Required] [SerializeField] private EnemyBossDependencyProvider _provider;
        [SerializeField] private float _findRange;
        // [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public IEnemyBossAnimation Animation => enemyBossAnimation;

        public ICharacterSpawnPoint CharacterMeleePoint { get; set; }

        // public FSMOwner FsmOwner => _fsmOwner;
        public EnemyBossDependencyProvider Provider => _provider;

        public float FindRange => _findRange;

        public void PlayMassAttackParticle() =>
            _massAttackParticle.Play();

        public void SetAgentSpeed(float speed) =>
            NavMeshAgent.speed = speed;

        public void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint) =>
            CharacterMeleePoint = characterSpawnPoint ?? throw new ArgumentNullException(nameof(characterSpawnPoint));
    }
}