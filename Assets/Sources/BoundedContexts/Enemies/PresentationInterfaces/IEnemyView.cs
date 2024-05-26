using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public interface IEnemyView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; }
        FSMOwner FsmOwner { get; }
        EnemyDependencyProvider Provider { get; }
        void SetTargetFollow(ICharacterHealthView characterViewHealthView);
    }
}