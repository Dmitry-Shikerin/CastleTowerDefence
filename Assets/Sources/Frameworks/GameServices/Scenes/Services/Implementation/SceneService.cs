using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Scenes.Controllers.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;
using Sources.Frameworks.StateMachines.SceneStateMachines.Implementation;
using Zenject;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.GameServices.Scenes.Services.Implementation
{
    public class SceneService : ISceneService
    {
        private readonly List<Func<string, UniTask>> _enteringHandlers = new List<Func<string, UniTask>>();
        private readonly List<Func<UniTask>> _exitingHandlers = new List<Func<UniTask>>();

        private readonly SceneStateMachine _stateMachine;
        private readonly IReadOnlyDictionary<string, Func<object, SceneContext, UniTask<IScene>>> _sceneFactories;
        
        public SceneService(IReadOnlyDictionary<string, Func<object, SceneContext, UniTask<IScene>>> sceneFactories)
        {
            _stateMachine = new SceneStateMachine();
            _sceneFactories = sceneFactories ?? throw new ArgumentNullException(nameof(sceneFactories));
        }
        
        public void AddBeforeSceneChangeHandler(Func<string, UniTask> handler) =>
            _enteringHandlers.Add(handler);
        
        public void AddAfterSceneChangeHandler(Func<UniTask> handler) =>
            _exitingHandlers.Add(handler);
        
        public void RemoveBeforeSceneChangeHandler(Func<string, UniTask> handler) =>
            _enteringHandlers.Remove(handler);
        
        public void RemoveAfterSceneChangeHandler(Func<UniTask> handler) =>
            _exitingHandlers.Remove(handler);

        public async UniTask ChangeSceneAsync(string sceneName, object payload = null)
        {
            try
            {
                if (_sceneFactories.TryGetValue(sceneName, out Func<object, SceneContext, UniTask<IScene>> sceneFactory) == false)
                    throw new InvalidOperationException(nameof(sceneName));

                foreach (Func<string, UniTask> enteringHandler in _enteringHandlers)
                    await enteringHandler.Invoke(sceneName);

                SceneContext sceneContext = Object.FindObjectOfType<SceneContext>();
                IScene scene = await sceneFactory.Invoke(payload, sceneContext);
                _stateMachine.ChangeState(scene, payload);

                foreach (Func<UniTask> exitingHandler in _exitingHandlers)
                    await exitingHandler.Invoke();
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Disable() =>
            _stateMachine.Exit();

        public void Update(float deltaTime) =>
            _stateMachine.Update(deltaTime);

        public void UpdateFixed(float fixedDeltaTime) =>
            _stateMachine.UpdateFixed(fixedDeltaTime);

        public void UpdateLate(float deltaTime) =>
            _stateMachine.UpdateLate(deltaTime);
    }
}