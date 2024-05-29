using NodeCanvas.StateMachines;
using Sources.BoundedContexts.AttackTargetFinders.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.PresentationsInterfaces.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces
{
    public interface ICharacterMeleeView : IView, IAttackTargetFinder
    {
        public Vector3 Position { get; }
        public ICharacterMeleeAnimation MeleeAnimation { get; }
        public CharacterHealthView HealthView { get; }
        public CharacterMeleeDependencyProvider Provider { get; }
        public FSMOwner FSMOwner { get; }
        public IEnemyHealthView EnemyHealth { get; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
        void SetLookRotation(float angle);
    }
}