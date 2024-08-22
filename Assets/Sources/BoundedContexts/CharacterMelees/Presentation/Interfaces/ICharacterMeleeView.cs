using Sources.BoundedContexts.AttackTargetFinders.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces
{
    public interface ICharacterMeleeView : IView, IAttackTargetFinder
    {
        public Vector3 Position { get; }
        public ICharacterMeleeAnimation MeleeAnimation { get; }
        public CharacterHealthView HealthView { get; }
        public CharacterMeleeDependencyProvider Provider { get; }
        public IEnemyHealthView EnemyHealth { get; }
        public ICharacterSpawnPoint CharacterSpawnPoint { get; }
        
        public void SetEnemyHealth(IEnemyHealthView enemyHealthView);
        void SetLookRotation(float angle);
        void SetCharacterSpawnPoint(ICharacterSpawnPoint spawnPoint);
    }
}