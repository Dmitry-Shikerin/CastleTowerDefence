using Sources.BoundedContexts.AttackTargetFinders.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Presentation.Interfaces
{
    public interface ICharacterView : IView, IAttackTargetFinder
    {
        Vector3 Position { get; }
        ICharacterHealthView CharacterHealth { get; }
        IEnemyHealthView EnemyHealth { get; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; }

        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
        void SetLookRotation(float angle);
        void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint);
    }
}