namespace Sources.Frameworks.MyGameCreator.Triggers.Interfaces
{
    public interface ITrigger<out T> : IEnteredTrigger<T>, IExitedTrigger<T>
    {
    }
}