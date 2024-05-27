using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.PresentationInterfaces
{
    public interface ICharacterMeleeView
    {
        public ICharacterMeleeAnimation MeleeAnimation { get; }
        public CharacterHealthView HealthView { get; }
        public CharacterMeleeDependencyProvider Provider { get; }
        public FSMOwner FSMOwner { get; }
        public IEnemyHealthView EnemyHealthView { get; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
    }
}