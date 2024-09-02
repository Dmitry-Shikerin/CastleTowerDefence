using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.Enemies.PresentationInterfaces
{
    public interface IEnemyView : IEnemyViewBase
    {
        IEnemyAnimation Animation { get; }
        ICharacterSpawnPoint CharacterMeleePoint { get; }
        float FindRange { get; }

        void SetCharacterMeleePoint(ICharacterSpawnPoint spawnPoint);
    }
}