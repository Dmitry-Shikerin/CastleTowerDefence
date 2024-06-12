using System;
using Leopotam.Ecs;
using Sources.ECSBoundedContexts.Systems;
using UnityEngine;
using Voody.UniLeo;

namespace Sources.ECSBoundedContexts.StarUps
{
    public class EcsGameStartUp : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;

        private void Start()
        {
            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
            _systems.ConvertScene();
            AddSystems();
            AddInjections();
            AddOneFrame();
            _systems.Init();
        }

        private void Update() =>
            _systems.Run();
        
        private void OnDestroy()
        {
            _systems.Destroy();
            _systems = null;
            _ecsWorld.Destroy();
            _ecsWorld = null;
        }

        private void AddSystems()
        {
            _systems
                .Add(new MovementSystem());
        }

        private void AddInjections()
        {
            
        }

        private void AddOneFrame()
        {
            
        }
    }
}