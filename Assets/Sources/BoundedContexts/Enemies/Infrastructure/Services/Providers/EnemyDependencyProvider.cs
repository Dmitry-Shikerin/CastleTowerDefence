using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
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
        public IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService { get; private set; }
        
        public void Construct(
            Enemy enemy, 
            PlayerWallet playerWallet,
            IEnemyView view, 
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            Enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            PlayerWallet = playerWallet ?? throw new ArgumentNullException(nameof(playerWallet));
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodyBloodySpawnService = 
                explosionBodyBloodySpawnService ?? 
                throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
        }
    }
}