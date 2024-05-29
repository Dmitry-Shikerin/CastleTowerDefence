using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.Presentations.Views;
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

        public HealthBarView HealthBarView => _healthBarView;
        public float FindRange => _findRange;
        public Vector3 Position => transform.position;
        public ICharacterMeleeAnimation MeleeAnimation => meleeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterMeleeDependencyProvider Provider => _provider;
        public FSMOwner FSMOwner => _fsmOwner;
        public IEnemyHealthView EnemyHealth { get; private set; }


        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealth = enemyHealthView ?? throw new ArgumentNullException(nameof(enemyHealthView));

        public void SetLookRotation(float angle)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler(0, angle, 0), CharacterConst.DeltaRotation);
        }
    }
}