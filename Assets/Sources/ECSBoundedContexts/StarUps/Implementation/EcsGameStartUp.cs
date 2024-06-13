using Leopotam.Ecs;
using Sources.ECSBoundedContexts.SearchlightMoves.Systems;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Voody.UniLeo;

namespace Sources.ECSBoundedContexts.StarUps.Implementation
{
    public class EcsGameStartUp : IEcsGameStartUp
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;

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
                .Add(new SearchlightMovementSystem());
        }

        private void AddInjections()
        {
            
        }

        private void AddOneFrame()
        {
            
        }
    }
}