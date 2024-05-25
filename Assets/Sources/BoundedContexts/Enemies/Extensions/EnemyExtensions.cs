using NodeCanvas.Framework;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.Enemies.Extensions
{
    public static class EnemyExtensions
    {
        public static EnemyDependencyProvider GetDependencyProvider(this IBlackboard blackboard) =>
            (blackboard.parent as Blackboard).GetComponent<EnemyDependencyProvider>();
    }
}