namespace Sources.PresentationsInterfaces.Triggers
{
    public interface ITrigger<out T> : IEnteredTrigger<T>, IExitedTrigger<T>
    {
    }
}