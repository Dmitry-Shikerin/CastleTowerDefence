using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Implementation
{
    public class CharacterMeleeView : View, ICharacterMeleeView
    {
        [SerializeField] private float _findRange = 1.2f;
        [Required] [SerializeField] private CharacterMeleeAnimation meleeAnimation;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterMeleeDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        [Required] [SerializeField] private HealthBarView _healthBarView;

        private IPODestroyerService _poDestroyerService = new PODestroyerService();
        
        public HealthBarView HealthBarView => _healthBarView;
        public float FindRange => _findRange;
        public Vector3 Position => transform.position;
        public ICharacterMeleeAnimation MeleeAnimation => meleeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterMeleeDependencyProvider Provider => _provider;
        public IEnemyHealthView EnemyHealth { get; private set; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; private set; }

        public override void Destroy()
        {
            StopBehaviour();
            _poDestroyerService.Destroy(this);
        }

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealth = enemyHealthView;

        public void SetLookRotation(float angle)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler(0, angle, 0), CharacterConst.DeltaRotation);
        }

        public void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint) =>
            CharacterSpawnPoint = spawnPoint;

        public void StartBehaviour() =>
            _fsmOwner.StartBehaviour();

        public void StopBehaviour() =>
            _fsmOwner.StopBehaviour();
    }
}