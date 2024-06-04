using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Implementation
{
    public class CharacterRangeView : View, ICharacterRangeView
    {
        [SerializeField] private float _findRange = 10f;
        [Required] [SerializeField] private CharacterRangeAnimation _rangeAnimation;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterRangeDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        [Required] [SerializeField] private ParticleSystem _shootParticle;
        
        public ICharacterRangeAnimation RangeAnimation => _rangeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterRangeDependencyProvider Provider => _provider;
        public FSMOwner FSMOwner => _fsmOwner;
        public float FindRange => _findRange;
        public Vector3 Position => transform.position;
        public ICharacterHealthView CharacterHealth => _healthView;
        public IEnemyHealthView EnemyHealth { get; private set; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; private set; }

        public void PlayShootParticle() =>
            _shootParticle.Play();

        public void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint) =>
            CharacterSpawnPoint = spawnPoint;

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealth = enemyHealthView;

        public void SetLookRotation(float angle)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler(0, angle, 0), CharacterConst.DeltaRotation);
        }
    }
}