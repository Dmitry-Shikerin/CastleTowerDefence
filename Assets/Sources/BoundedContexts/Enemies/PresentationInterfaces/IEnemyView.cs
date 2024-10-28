namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; }
        float FindRange { get; }
    }
}