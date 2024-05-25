using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Presentation;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers
{
    public class EnemyDependencyProvider : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }
        public IEnemyView View { get; private set; }
        
        public void Construct(Enemy enemy, IEnemyView view)
        {
            Enemy = enemy ?? throw new System.ArgumentNullException(nameof(enemy));
            View = view ?? throw new System.ArgumentNullException(nameof(view));
        }

        [Inject]
        private void OnAfterConstruct()
        {
            Debug.Log($"DependencyProvider: {Enemy} {View}");
        }
    }
}