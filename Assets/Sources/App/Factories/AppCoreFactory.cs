using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.Ids;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Controllers.Interfaces;
using Sources.ControllersInterfaces.Scenes;
using Sources.Domain.Models.Constants;
using Sources.Infrastructure.Services.SceneServices;
using Sources.InfrastructureInterfaces.Services.SceneLoaderService;
using Sources.Presentations.UI.Curtains;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Sources.App.Factories
{
    public class AppCoreFactory
    {
        public AppCore Create()
        {
            AppCore appCore = new GameObject(nameof(AppCore)).AddComponent<AppCore>();

            ProjectContext projectContext = Object.FindObjectOfType<ProjectContext>();
            CurtainView curtainView =
                Object.Instantiate(Resources.Load<CurtainView>(PrefabPath.Curtain)) ??
                throw new NullReferenceException(nameof(CurtainView));
            projectContext.Container.Bind<CurtainView>().FromInstance(curtainView);
            curtainView.Hide();

            Dictionary<string, Func<object, SceneContext, UniTask<IScene>>> sceneFactories =
                new Dictionary<string, Func<object, SceneContext, UniTask<IScene>>>();
            SceneService sceneService = new SceneService(sceneFactories);
            projectContext.Container.BindInterfacesAndSelfTo<SceneService>().FromInstance(sceneService);

            sceneFactories[ModelId.MainMenu] = (payload, sceneContext) =>
                sceneContext.Container.Resolve<ISceneFactory>().Create(payload);
            sceneFactories[ModelId.Gameplay] = (payload, sceneContext) =>
                sceneContext.Container.Resolve<ISceneFactory>().Create(payload);

            // sceneService.AddBeforeSceneChangeHandler(async _ => await curtainView.ShowCurtain());
            //
            sceneService.AddBeforeSceneChangeHandler(async sceneName =>
                await projectContext.Container.Resolve<ISceneLoaderService>().LoadSceneAsync(sceneName));

            appCore.Construct(sceneService);
            // Debug.Log($"appCore: {nameof(AppCoreFactory)}");

            return appCore;
        }
    }
}