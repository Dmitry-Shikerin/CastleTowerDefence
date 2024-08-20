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
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.MyGameCreator.SkyAndWeawers.Domain;

namespace Sources.Frameworks.GameServices.Prefabs.Implementation.Composites
{
    public class CompositeAssetService : ICompositeAssetService
    {
        private readonly IAddressablesAssetLoader _addressablesAssetLoader;
        private readonly IPrefabLoader _prefabLoader;
        private readonly IAddressablesAssetLoader[] _assetServices;
        
        public CompositeAssetService(
            IAddressablesAssetLoader addressablesAssetLoader,
            IPrefabLoader prefabLoader)
        {
            _addressablesAssetLoader = addressablesAssetLoader ?? throw new ArgumentNullException(nameof(addressablesAssetLoader));
            _prefabLoader = prefabLoader ?? throw new ArgumentNullException(nameof(prefabLoader));
        }
        
        public async UniTask LoadAsync()
        {
            await _prefabLoader.LoadAsset<SkyAndWeatherCollector>(PrefabPath .SkyAndWeatherCollector);
            await _prefabLoader.LoadAsset<PoolManagerCollector>(PrefabPath.PoolManagerCollector);
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
            _prefabLoader.ReleaseAll();
            _addressablesAssetLoader.ReleaseAll();
        }
    }
}