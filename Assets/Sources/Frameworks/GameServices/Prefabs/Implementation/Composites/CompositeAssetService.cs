using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.Enemies.Domain.Constants;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces.Composites;
using Sources.Frameworks.MyGameCreator.Achievements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.SkyAndWeawers.Domain;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation.Composites
{
    public class CompositeAssetService : ICompositeAssetService
    {
        private readonly IAddressablesAssetLoader _addressablesAssetLoader;
        private readonly IResourcesAssetLoader _resourcesAssetLoader;
        private readonly IAddressablesAssetLoader[] _assetServices;
        
        public CompositeAssetService(
            IAddressablesAssetLoader addressablesAssetLoader,
            IResourcesAssetLoader resourcesAssetLoader)
        {
            _addressablesAssetLoader = addressablesAssetLoader ?? throw new ArgumentNullException(nameof(addressablesAssetLoader));
            _resourcesAssetLoader = resourcesAssetLoader ?? throw new ArgumentNullException(nameof(resourcesAssetLoader));
        }
        
        public async UniTask LoadAsync()
        {
            await _resourcesAssetLoader.LoadAsset<AchievementConfigCollector>(PrefabPath.AchievementConfigCollector);
            await _resourcesAssetLoader.LoadAsset<SkyAndWeatherCollector>(PrefabPath .SkyAndWeatherCollector);
            await _resourcesAssetLoader.LoadAsset<PoolManagerCollector>(PrefabPath.PoolManagerCollector);
            await _resourcesAssetLoader.LoadAsset<ExplosionBodyBloodyView>(PrefabPath.ExplosionBodyBloody);
            await _resourcesAssetLoader.LoadAsset<ExplosionBodyView>(PrefabPath.ExplosionBody);
            await _resourcesAssetLoader.LoadAsset<CharacterMeleeView>(PrefabPath.CharacterMeleeView);
            await _resourcesAssetLoader.LoadAsset<CharacterRangeView>(PrefabPath.CharacterRangeView);
            await _resourcesAssetLoader.LoadAsset<EnemyView>(EnemyConst.PrefabPath);
            await _resourcesAssetLoader.LoadAsset<EnemyBossView>(PrefabPath.BossEnemy);
            await _resourcesAssetLoader.LoadAsset<EnemyKamikazeView>(PrefabPath.EnemyKamikaze);
            await _resourcesAssetLoader.LoadAsset<UpgradeConfigContainer>(PrefabPath.UpgradeConfigContainer);
            await _resourcesAssetLoader.LoadAsset<EnemySpawnStrategyCollector>(PrefabPath.EnemySpawnStrategyCollector);
            await _resourcesAssetLoader.LoadAsset<EnemySpawnerConfig>(PrefabPath.EnemySpawnerConfigContainer);
        }

        public void Release()
        {
            _resourcesAssetLoader.ReleaseAll();
            _addressablesAssetLoader.ReleaseAll();
        }
    }
}