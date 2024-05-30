using System.Collections.Generic;
using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories
{
    public class MainMenuSignalControllerCollector : ISignalControllersCollector
    {
        private readonly List<ISignalController> _signalControllers;
        
        public MainMenuSignalControllerCollector(
            ButtonSignalController buttonSignalController)
        {
            _signalControllers = new List<ISignalController>()
            {
                buttonSignalController,
            };
        }

        public void Initialize()
        {
            foreach (ISignalController signalController in _signalControllers)
                signalController.Initialize();
        }

        public void Destroy()
        {
            foreach (ISignalController signalController in _signalControllers)
                signalController.Destroy();
            
            _signalControllers.Clear();
        }
    }
}