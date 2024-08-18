using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Cameras;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Providers
{
    public class EnemyKamikazeDependencyProviderFactory
    {
        private readonly ICameraService _cameraService;
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly ExplosionBodyViewFactory _explosionBodyViewFactory;
        private readonly ExplosionBodyBloodyViewFactory _explosionBodyBloodyViewFactory;

        public EnemyKamikazeDependencyProviderFactory(
            ICameraService cameraService,
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            ExplosionBodyViewFactory explosionBodyViewFactory,
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory)
        {
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
            _entityRepository = entityRepository ??
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodyViewFactory = explosionBodyViewFactory ??
                                        throw new ArgumentNullException(nameof(explosionBodyViewFactory));
            _explosionBodyBloodyViewFactory = explosionBodyBloodyViewFactory ??
                                              throw new ArgumentNullException(nameof(explosionBodyBloodyViewFactory));
        }

        public EnemyKamikazeDependencyProvider Create(EnemyKamikaze enemyKamikaze, IEnemyKamikazeView view)
        {
            view.Provider.Construct(
                enemyKamikaze,
                _entityRepository,
                view,
                view.Animation,
                _overlapService,
                _explosionBodyViewFactory,
                _explosionBodyBloodyViewFactory,
                _cameraService);

            return view.Provider;
        }
    }
}