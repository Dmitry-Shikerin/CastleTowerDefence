using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.DomainInterfaces.Models.Payloads;
using Sources.Frameworks.UiFramework.Collectors;
using UnityEngine;

public class MainMenuSceneViewFactory : ISceneViewFactory
{
    private readonly UiCollectorFactory _uiCollectorFactory;

    public MainMenuSceneViewFactory(UiCollectorFactory uiCollectorFactory)
    {
        _uiCollectorFactory = uiCollectorFactory ?? throw new ArgumentNullException(nameof(uiCollectorFactory));
    }
    
    public void Create(IScenePayload payload)
    {
        //ui framework
        _uiCollectorFactory.Create();
    }
}
