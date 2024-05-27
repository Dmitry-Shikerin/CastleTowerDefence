﻿using System;
using System.Collections.Generic;
using Sources.BoundedContexts.Characters.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Characters.Presentation;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.Upgrades.Domain;
using UnityEngine.TextCore.Text;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views
{
    public class GameplaySceneViewFactory
    {
        private readonly IEnemyViewFactory _enemyViewFactory;
        private readonly CharacterViewFactory _characterViewFactory;
        private readonly IBossEnemyViewFactory _bossEnemyViewFactory;

        public GameplaySceneViewFactory(
            IEnemyViewFactory enemyViewFactory,
            CharacterViewFactory characterViewFactory,
            IBossEnemyViewFactory bossEnemyViewFactory)
        {
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
            _characterViewFactory = characterViewFactory ?? 
                                    throw new ArgumentNullException(nameof(characterViewFactory));
            _bossEnemyViewFactory = bossEnemyViewFactory ?? 
                                    throw new ArgumentNullException(nameof(bossEnemyViewFactory));
        }

        public void Create()
        {
            EnemyAttacker enemyAttacker = new EnemyAttacker(0);
            EnemyHealth enemyHealth = new EnemyHealth(0);
            Enemy enemy = new Enemy(enemyHealth, enemyAttacker);
            KillEnemyCounter killEnemyCounter = new KillEnemyCounter("KillEnemyCounter", 0);
            EnemyView enemyView = Object.FindObjectOfType<EnemyView>();
            _enemyViewFactory.Create(enemy, killEnemyCounter, enemyView);
            
            EnemyAttacker bossEnemyAttacker = new EnemyAttacker(0);
            EnemyHealth bossEnemyHealth = new EnemyHealth(0);
            BossEnemy bossEnemy = new BossEnemy(bossEnemyHealth, bossEnemyAttacker, 0, 0, 0);
            KillEnemyCounter bossKillEnemyCounter = new KillEnemyCounter("KillEnemyCounter", 0);
            EnemyBosses.Presentation.Implementation.BossEnemyView bossEnemyView = 
                Object.FindObjectOfType<EnemyBosses.Presentation.Implementation.BossEnemyView>();
            _bossEnemyViewFactory.Create(bossEnemy, bossKillEnemyCounter, bossEnemyView);
            
            CharacterView characterView = Object.FindObjectOfType<CharacterView>();
            _characterViewFactory.Create(characterView);
        }
    }
}