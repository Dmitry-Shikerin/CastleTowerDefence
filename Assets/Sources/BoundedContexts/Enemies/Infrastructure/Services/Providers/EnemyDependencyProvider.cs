using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers
{
    public class EnemyDependencyProvider : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }
        public IEnemyView View { get; private set; }
        public IEnemyAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        
        public void Construct(
            Enemy enemy, 
            IEnemyView view, 
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService)
        {
            Enemy = enemy ?? throw new System.ArgumentNullException(nameof(enemy));
            View = view ?? throw new System.ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new System.ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new System.ArgumentNullException(nameof(overlapService));
        }
    }
}