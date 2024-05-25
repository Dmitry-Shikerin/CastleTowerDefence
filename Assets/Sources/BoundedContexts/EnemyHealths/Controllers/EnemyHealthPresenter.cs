﻿using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Controllers.Common;

namespace Sources.BoundedContexts.Enemies.Controllers
{
    public class EnemyHealthPresenter : PresenterBase
    {
        private readonly EnemyHealth _enemyHealth;
        private readonly IEnemyHealthView _enemyHealthView;

        public EnemyHealthPresenter(EnemyHealth enemyHealth, IEnemyHealthView enemyHealthView)
        {
            _enemyHealth = enemyHealth ?? throw new ArgumentNullException(nameof(enemyHealth));
            _enemyHealthView = enemyHealthView ?? throw new ArgumentNullException(nameof(enemyHealthView));
        }

        public float CurrentHealth => _enemyHealth.CurrentHealth;
        
        public void TakeDamage(float damage)
        {
            _enemyHealth.TakeDamage(damage);
            _enemyHealthView.PlayBloodParticle();
        }
    }
}