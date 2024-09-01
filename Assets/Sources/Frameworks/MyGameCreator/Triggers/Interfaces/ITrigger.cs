using Sources.PresentationsInterfaces.Triggers;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Triggers
{
    public interface ITrigger<out T> : IEnteredTrigger<T>, IExitedTrigger<T>
    {
    }
}