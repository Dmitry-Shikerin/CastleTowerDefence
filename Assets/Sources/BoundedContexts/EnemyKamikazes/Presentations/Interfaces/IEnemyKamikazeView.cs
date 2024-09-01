using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces
{
    public interface IEnemyKamikazeView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; } 
        float FindRange { get; }
    }
}