using System;
using Leopotam.Ecs;
using Sources.ECSBoundedContexts.SearchlightMoves.Infrastructure.Features;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Voody.UniLeo;
using Zenject;

namespace Sources.ECSBoundedContexts.StarUps.Implementation
{
    public class EcsGameStartUp : IEcsGameStartUp
    {
        private DiContainer _container;
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;

        public EcsGameStartUp(DiContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public void Initialize()
        {
            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
            _systems.ConvertScene();
            AddSystems();
            AddInjections();
            AddOneFrame();
            _systems.Init();
        }

        public void Update(float deltaTime) =>
            _systems.Run();

        public void Destroy()
        {
            _systems.Destroy();
            _systems = null;
            _ecsWorld.Destroy();
            _ecsWorld = null;
        }

        private void AddSystems()
        {
            _systems
                .Add(new SearchlightMovementFeature());
        }

        private void AddInjections()
        {
            _systems.Inject(_container, typeof(DiContainer));
        }

        private void AddOneFrame()
        {
            
        }
    }
}