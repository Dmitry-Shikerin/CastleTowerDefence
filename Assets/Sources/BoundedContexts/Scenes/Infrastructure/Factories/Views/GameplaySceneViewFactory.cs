using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.Upgrades.Domain;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views
{
    public class GameplaySceneViewFactory
    {
        private readonly EnemyViewFactory _enemyViewFactory;

        public GameplaySceneViewFactory(
            EnemyViewFactory enemyViewFactory)
        {
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public void Create()
        {
            EnemyAttacker enemyAttacker = new EnemyAttacker();
            EnemyHealth enemyHealth = new EnemyHealth(0);
            Enemy enemy = new Enemy(enemyHealth, enemyAttacker);
            EnemyView enemyView = Object.FindObjectOfType<EnemyView>();
            _enemyViewFactory.Create(enemy, enemyView);
        }
    }
}