using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using Sources.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces
{
    public interface ICharacterRangeView : IView
    {
        float FindRange { get; }
        Vector3 Position { get; }
        ICharacterHealthView CharacterHealth { get; }
        IEnemyHealthView EnemyHealth { get; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; }

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
        void SetLookRotation(float angle);
        void PlayShootParticle();
        void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint);
    }
}