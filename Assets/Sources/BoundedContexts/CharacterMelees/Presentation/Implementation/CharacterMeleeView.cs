using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.CharacterMelees.Presentation
{
    public class CharacterMeleeView : View, ICharacterMeleeView
    {
        [Required] [SerializeField] private CharacterMeleeAnimation meleeAnimation;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterMeleeDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public ICharacterMeleeAnimation MeleeAnimation => meleeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterMeleeDependencyProvider Provider => _provider;
        public FSMOwner FSMOwner => _fsmOwner;
        public IEnemyHealthView EnemyHealthView { get; private set; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealthView = enemyHealthView ?? throw new ArgumentNullException(nameof(enemyHealthView));
    }
}