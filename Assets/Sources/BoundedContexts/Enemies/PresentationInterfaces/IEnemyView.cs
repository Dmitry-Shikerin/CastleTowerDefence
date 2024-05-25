using NodeCanvas.StateMachines;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public interface IEnemyView : IEnemyViewBase
    {
        FSMOwner FsmOwner { get; }
        EnemyDependencyProvider Provider { get; }
    }
}