using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces
{
    public interface IEnemyKamikazeView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; } 
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        float FindRange { get; }
        
        void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint);
    }
}