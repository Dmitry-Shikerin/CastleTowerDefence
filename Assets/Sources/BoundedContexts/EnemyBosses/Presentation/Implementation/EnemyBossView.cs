﻿using System;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Implementation
{
    public class EnemyBossView : EnemyViewBase, IEnemyBossView
    {
        [Required] [SerializeField] private EnemyBossAnimation enemyBossAnimation;
        [Required] [SerializeField] private ParticleSystem _massAttackParticle;
        [SerializeField] private float _findRange;
        
        public IEnemyBossAnimation Animation => enemyBossAnimation;
        public ICharacterSpawnPoint CharacterMeleePoint { get; set; }

        public float FindRange => _findRange;

        public void PlayMassAttackParticle() =>
            _massAttackParticle.Play();

        public void SetAgentSpeed(float speed) =>
            NavMeshAgent.speed = speed;

        public void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint) =>
            CharacterMeleePoint = characterSpawnPoint ?? throw new ArgumentNullException(nameof(characterSpawnPoint));
    }
}