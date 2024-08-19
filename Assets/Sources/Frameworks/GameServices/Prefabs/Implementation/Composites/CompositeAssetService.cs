using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.Enemies.Domain.Constants;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.Prefabs;
using Sources.Frameworks.GameServices.Addressables.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Domain;
using Sources.Frameworks.MyGameCreator.SkyAndWeawers.Domain;

namespace Sources.Frameworks.GameServices.Addressables.Implementation
{
    public class CompositeAssetService : ICompositeAssetService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPrefabLoader _prefabLoader;
        private readonly IAssetProvider[] _assetServices;
        
        public CompositeAssetService(
            IAssetProvider assetProvider,
            IPrefabLoader prefabLoader)
        {
            _assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            _prefabLoader = prefabLoader ?? throw new ArgumentNullException(nameof(prefabLoader));
        }
        
        public async UniTask LoadAsync()
        {
            await _prefabLoader.LoadObject<SkyAndWeatherCollector>("Configs/SkyAndWeathers/SkyAndWeatherCollector");
            await _prefabLoader.LoadObject<PoolManagerCollector>("Services/PoolManagers/PoolManagerCollector");
            await _prefabLoader.LoadAsset<ExplosionBodyBloodyView>(PrefabPath.ExplosionBodyBloody);
            await _prefabLoader.LoadAsset<ExplosionBodyView>(PrefabPath.ExplosionBody);
            await _prefabLoader.LoadAsset<CharacterMeleeView>(PrefabPath.CharacterMeleeView);
            await _prefabLoader.LoadAsset<CharacterRangeView>(PrefabPath.CharacterRangeView);
            await _prefabLoader.LoadAsset<EnemyView>(EnemyConst.PrefabPath);
            await _prefabLoader.LoadAsset<EnemyBossView>(PrefabPath.BossEnemy);
            await _prefabLoader.LoadAsset<EnemyKamikazeView>(PrefabPath.EnemyKamikaze);
        }

        public void Release()
        {
            _prefabLoader.Release();
            _assetProvider.Release();
        }
    }
}