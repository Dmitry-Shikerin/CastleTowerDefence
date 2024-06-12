using System;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.EnemyHealths.Controllers
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