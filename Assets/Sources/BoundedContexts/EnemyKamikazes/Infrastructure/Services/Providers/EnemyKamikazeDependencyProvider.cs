using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Cameras;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers
{
    public class EnemyKamikazeDependencyProvider : MonoBehaviour
    {
        public EnemyKamikaze EnemyKamikaze { get; private set; }
        public PlayerWallet PlayerWallet { get; private set; }
        public KillEnemyCounter KillEnemyCounter { get; private set; }
        public IEnemyKamikazeView View { get; private set; }
        public IEnemyAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public IEntityRepository EntityRepository { get; private set; }
        public ExplosionBodyViewFactory ExplosionBodyViewFactory { get; private set; }
        public ExplosionBodyBloodyViewFactory ExplosionBodyBloodyViewFactory { get; private set; }
        public ICameraService CameraService { get; private set; }

        public void Construct(
            EnemyKamikaze enemyKamikaze,
            IEntityRepository entityRepository,
            IEnemyKamikazeView view,
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService,
            ExplosionBodyViewFactory explosionBodySpawnService,
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory,
            ICameraService cameraService)
        {
            EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            EnemyKamikaze = enemyKamikaze ?? throw new ArgumentNullException(nameof(enemyKamikaze));
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodyViewFactory = explosionBodySpawnService ?? 
                                         throw new ArgumentNullException(nameof(explosionBodySpawnService));
            ExplosionBodyBloodyViewFactory = explosionBodyBloodyViewFactory ?? 
                                              throw new ArgumentNullException(nameof(explosionBodyBloodyViewFactory));
            KillEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            PlayerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            CameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
        }
    }
}