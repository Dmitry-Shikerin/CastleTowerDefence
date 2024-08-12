using System;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers
{
    public class EnemyDependencyProvider : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }
        public PlayerWallet PlayerWallet { get; private set; }
        public IEnemyView View { get; private set; }
        public IEnemyAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public ExplosionBodyBloodyViewFactory ExplosionBodyBloodyViewFactory { get; private set; }
        public IEntityRepository EntityRepository { get; private set; }
        
        public void Construct(
            Enemy enemy, 
            IEntityRepository entityRepository,
            IEnemyView view, 
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService,
            ExplosionBodyBloodyViewFactory explosionBodyBloodySpawnService)
        {
            EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            Enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            PlayerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodyBloodyViewFactory = 
                explosionBodyBloodySpawnService ?? 
                throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
        }
    }
}