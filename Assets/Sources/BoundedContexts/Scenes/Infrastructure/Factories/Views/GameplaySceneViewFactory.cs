using System;
using System.Collections.Generic;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Upgrades.Domain;
using Sources.Domain.Models.Data;
using UnityEngine.TextCore.Text;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views
{
    public class GameplaySceneViewFactory
    {
        private readonly RootGameObject _rootGameObject;
        private readonly EnemySpawnerViewFactory _enemySpawnerViewFactory;
        private readonly ICharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly CharacterSpawnerViewFactory _characterSpawnerViewFactory;
        private readonly IEnemyViewFactory _enemyViewFactory;

        public GameplaySceneViewFactory(
            RootGameObject rootGameObject,
            EnemySpawnerViewFactory enemySpawnerViewFactory,
            CharacterSpawnerViewFactory characterSpawnerViewFactory,
            IEnemyViewFactory enemyViewFactory,
            ICharacterMeleeViewFactory characterMeleeViewFactory)
        {
            _rootGameObject = rootGameObject ?? throw new ArgumentNullException(nameof(rootGameObject));
            _enemySpawnerViewFactory = enemySpawnerViewFactory ?? 
                                       throw new ArgumentNullException(nameof(enemySpawnerViewFactory));
            _characterMeleeViewFactory = characterMeleeViewFactory ?? 
                                         throw new ArgumentNullException(nameof(characterMeleeViewFactory));
            _characterSpawnerViewFactory = characterSpawnerViewFactory ?? 
                                           throw new ArgumentNullException(nameof(characterSpawnerViewFactory));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public void Create()
        {
            // EnemyAttacker enemyAttacker = new EnemyAttacker(0);
            // EnemyHealth enemyHealth = new EnemyHealth(0);
            // Enemy enemy = new Enemy(enemyHealth, enemyAttacker);
            // KillEnemyCounter killEnemyCounter = new KillEnemyCounter("KillEnemyCounter", 0);
            // EnemyView enemyView = Object.FindObjectOfType<EnemyView>();
            // _enemyViewFactory.Create(enemy, killEnemyCounter, enemyView);
            //
            // EnemyAttacker bossEnemyAttacker = new EnemyAttacker(0);
            // EnemyHealth bossEnemyHealth = new EnemyHealth(0);
            // BossEnemy bossEnemy = new BossEnemy(bossEnemyHealth, bossEnemyAttacker, 0, 0, 0);
            // KillEnemyCounter bossKillEnemyCounter = new KillEnemyCounter("KillEnemyCounter", 0);
            // EnemyBosses.Presentation.Implementation.BossEnemyView bossEnemyView = 
            //     Object.FindObjectOfType<EnemyBosses.Presentation.Implementation.BossEnemyView>();
            // _bossEnemyViewFactory.Create(bossEnemy, bossKillEnemyCounter, bossEnemyView);
            //
            // CharacterMeleeView characterMeleeView = Object.FindObjectOfType<CharacterMeleeView>();
            // CharacterMelee characterMelee = new CharacterMelee(
            //     new Characters.CharacterHealth(
            //         new Upgrade(
            //             0,
            //             0,
            //             0,
            //             new List<int>(), "CharacterHealthUpgrade"))
            // );
            // _characterMeleeViewFactory.Create(characterMelee, characterMeleeView);

            //Characters
            CharacterSpawner characterSpawner = new CharacterSpawner();
            _characterSpawnerViewFactory.Create(characterSpawner, _rootGameObject.CharacterSpawnerView);
            
            //Enemies
            EnemySpawner enemySpawner = new EnemySpawner();
            _enemySpawnerViewFactory.Create(
                enemySpawner, 
                new KillEnemyCounter(new KillEnemyCounterDto()), 
                _rootGameObject.EnemySpawnerView);
        }
    }
}