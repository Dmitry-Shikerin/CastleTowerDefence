using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers
{
    public class EnemyDependencyProvider : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }
        public IEnemyView View { get; private set; }
        public IEnemyAnimation EnemyAnimation { get; private set; }
        
        public void Construct(Enemy enemy, IEnemyView view, IEnemyAnimation enemyAnimation)
        {
            Enemy = enemy ?? throw new System.ArgumentNullException(nameof(enemy));
            View = view ?? throw new System.ArgumentNullException(nameof(view));
            EnemyAnimation = view.Animation ?? throw new System.ArgumentNullException(nameof(view.Animation));
        }
    }
}