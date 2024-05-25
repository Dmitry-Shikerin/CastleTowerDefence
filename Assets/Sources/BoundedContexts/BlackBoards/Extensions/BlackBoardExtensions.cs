using NodeCanvas.Framework;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.Enemies.Extensions
{
    public  static partial class BlackBoardExtensions
    {
        public static T GetDependencyProvider<T>(this IBlackboard blackboard) =>
            (blackboard.parent as Blackboard).GetComponent<T>();
    }
}