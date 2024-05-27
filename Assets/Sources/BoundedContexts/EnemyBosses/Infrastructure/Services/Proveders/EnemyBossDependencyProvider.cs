using System;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders
{
    public class EnemyBossDependencyProvider : MonoBehaviour
    {
        private bool _isConstructed;
        
        public BossEnemy BossEnemy { get; private set; }
        public KillEnemyCounter KillEnemyCounter { get; private set; }
        public IBossEnemyView BossEnemyView { get; private set; }
        public IBossEnemyAnimation BossEnemyAnimation { get; private set; }
        
        public void Construct(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            IBossEnemyView bossEnemyView, 
            IBossEnemyAnimation bossEnemyAnimation)
        {
            BossEnemy = bossEnemy ?? throw new ArgumentNullException(nameof(bossEnemy));
            KillEnemyCounter = killEnemyCounter ?? throw new ArgumentNullException(nameof(killEnemyCounter));
            BossEnemyView = bossEnemyView ?? throw new ArgumentNullException(nameof(bossEnemyView));
            BossEnemyAnimation = bossEnemyAnimation ?? throw new ArgumentNullException(nameof(bossEnemyAnimation));
        }
    }
}