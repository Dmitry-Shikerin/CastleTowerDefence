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
            await UniTask.WhenAll(
                _resourcesAssetLoader.LoadAsset<AchievementConfigCollector>(PrefabPath.AchievementConfigCollector),
                _resourcesAssetLoader.LoadAsset<SkyAndWeatherCollector>(PrefabPath.SkyAndWeatherCollector),
                _resourcesAssetLoader.LoadAsset<PoolManagerCollector>(PrefabPath.PoolManagerCollector),
                _resourcesAssetLoader.LoadAsset<ExplosionBodyBloodyView>(PrefabPath.ExplosionBodyBloody),
                _resourcesAssetLoader.LoadAsset<ExplosionBodyView>(PrefabPath.ExplosionBody),
                _resourcesAssetLoader.LoadAsset<CharacterMeleeView>(PrefabPath.CharacterMeleeView),
                _resourcesAssetLoader.LoadAsset<CharacterRangeView>(PrefabPath.CharacterRangeView),
                _resourcesAssetLoader.LoadAsset<EnemyView>(EnemyConst.PrefabPath),
                _resourcesAssetLoader.LoadAsset<EnemyBossView>(PrefabPath.BossEnemy),
                _resourcesAssetLoader.LoadAsset<EnemyKamikazeView>(PrefabPath.EnemyKamikaze),
                _resourcesAssetLoader.LoadAsset<UpgradeConfigContainer>(PrefabPath.UpgradeConfigContainer),
                _resourcesAssetLoader.LoadAsset<EnemySpawnStrategyCollector>(PrefabPath.EnemySpawnStrategyCollector),
                _resourcesAssetLoader.LoadAsset<EnemySpawnerConfig>(PrefabPath.EnemySpawnerConfigContainer)
            );
        }

        public void Release()
        {
            _resourcesAssetLoader.ReleaseAll();
            _addressablesAssetLoader.ReleaseAll();
        }
    }
}