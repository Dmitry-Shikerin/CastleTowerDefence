using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Implementation
{
    public class CharacterRangeView : View, ICharacterRangeView
    {
        [SerializeField] private float _findRange = 10f;
        [Required] [SerializeField] private CharacterRangeAnimation _rangeAnimation;
        [Required] [SerializeField] private HealthBarView _healthBarView;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterRangeDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        [Required] [SerializeField] private ParticleSystem _shootParticle;

        private IPODestroyerService _poDestroyerService = new PODestroyerService();
        
        public HealthBarView HealthBarView => _healthBarView;
        public ICharacterRangeAnimation RangeAnimation => _rangeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterRangeDependencyProvider Provider => _provider;
        public float FindRange => _findRange;
        public Vector3 Position => transform.position;
        public ICharacterHealthView CharacterHealth => _healthView;
        public IEnemyHealthView EnemyHealth { get; private set; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; private set; }

        public override void Destroy()
        {
            _fsmOwner.StopBehaviour();
            _poDestroyerService.Destroy(this);
        }
        
        public void PlayShootParticle() =>
            _shootParticle.Play();

        public void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint) =>
            CharacterSpawnPoint = spawnPoint;

        public void StartFsm() =>
            _fsmOwner.StartBehaviour();

        public void StopFsm() =>
            _fsmOwner.StopBehaviour();

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealth = enemyHealthView;

        public void SetLookRotation(float angle)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler(0, angle, 0), CharacterConst.DeltaRotation);
        }
    }
}