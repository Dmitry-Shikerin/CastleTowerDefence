using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Characters.Controllers;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;

namespace Sources.BoundedContexts.Characters.PresentationInterfaces
{
    public interface ICharacterView
    {
        public ICharacterAnimation Animation { get; }
        public ICharacterHealthView HealthView { get; }
        public CharacterDependencyProvider Provider { get; }
        public FSMOwner FSMOwner { get; }
        public IEnemyHealthView EnemyHealthView { get; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
    }
}