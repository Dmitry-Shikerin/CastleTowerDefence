using System;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Implementation
{
    public class CharacterRangeView : View, ICharacterRangeView
    {
        [Required] [SerializeField] private CharacterRangeAnimation _rangeAnimation;
        [Required] [SerializeField] private CharacterHealthView _healthView;
        [Required] [SerializeField] private CharacterRangeDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public ICharacterRangeAnimation RangeAnimation => _rangeAnimation;
        public CharacterHealthView HealthView => _healthView;
        public CharacterRangeDependencyProvider Provider => _provider;
        public FSMOwner FSMOwner => _fsmOwner;
        public IEnemyHealthView EnemyHealthView { get; private set; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView) =>
            EnemyHealthView = enemyHealthView ?? throw new ArgumentNullException(nameof(enemyHealthView));
    }
}