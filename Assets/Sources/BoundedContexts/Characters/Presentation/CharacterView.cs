using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Presentation
{
    public class CharacterView : View, ICharacterView
    {
        [Required] [SerializeField] private CharacterAnimation _animation;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public ICharacterAnimation Animation => _animation;
        public ICharacterHealthView HealthView => _healthView;
        public CharacterDependencyProvider Provider => _provider;
        public FSMOwner FSMOwner => _fsmOwner;
        public IEnemyHealthView EnemyHealthView { get; private set; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealthView = enemyHealthView ?? throw new ArgumentNullException(nameof(enemyHealthView));
    }
}