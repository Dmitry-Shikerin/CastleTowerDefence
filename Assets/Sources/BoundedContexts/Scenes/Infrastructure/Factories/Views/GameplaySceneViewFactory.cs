using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Characters.Presentation;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.Upgrades.Domain;
using UnityEngine.TextCore.Text;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views
{
    public class GameplaySceneViewFactory
    {
        private readonly EnemyViewFactory _enemyViewFactory;
        private readonly CharacterViewFactory _characterViewFactory;

        public GameplaySceneViewFactory(
            EnemyViewFactory enemyViewFactory,
            CharacterViewFactory characterViewFactory)
        {
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
            _characterViewFactory = characterViewFactory ?? 
                                    throw new ArgumentNullException(nameof(characterViewFactory));
        }

        public void Create()
        {
            EnemyAttacker enemyAttacker = new EnemyAttacker();
            EnemyHealth enemyHealth = new EnemyHealth(0);
            Enemy enemy = new Enemy(enemyHealth, enemyAttacker);
            EnemyView enemyView = Object.FindObjectOfType<EnemyView>();
            _enemyViewFactory.Create(enemy, enemyView);
            
            CharacterView characterView = Object.FindObjectOfType<CharacterView>();
            _characterViewFactory.Create(characterView);
        }
    }
}