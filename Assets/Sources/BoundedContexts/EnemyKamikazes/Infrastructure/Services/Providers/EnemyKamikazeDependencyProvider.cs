using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers
{
    public class EnemyKamikazeDependencyProvider : MonoBehaviour
    {
        public EnemyKamikaze EnemyKamikaze { get; private set; }
        public IEnemyKamikazeView View { get; private set; }
        public IEnemyAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public IExplosionBodySpawnService ExplosionBodySpawnService { get; private set; }
        public IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService { get; set; }

        public void Construct(
            EnemyKamikaze enemyKamikaze,
            IEnemyKamikazeView view,
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService,
            IExplosionBodySpawnService explosionBodySpawnService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            EnemyKamikaze = enemyKamikaze ?? throw new ArgumentNullException(nameof(enemyKamikaze));
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodySpawnService = explosionBodySpawnService ?? 
                                         throw new ArgumentNullException(nameof(explosionBodySpawnService));
            ExplosionBodyBloodySpawnService = explosionBodyBloodySpawnService ?? 
                                              throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
        }
    }
}