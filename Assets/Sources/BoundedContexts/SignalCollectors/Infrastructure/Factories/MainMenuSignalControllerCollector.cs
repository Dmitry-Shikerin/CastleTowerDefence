using System.Collections.Generic;
using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;

namespace Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories
{
    public class MainMenuSignalControllerCollector : ISignalControllersCollector
    {
        private readonly List<ISignalController> _signalControllers;
        
        public MainMenuSignalControllerCollector(
            MainMenuButtonsCommandSignalController mainMenuButtonsCommandSignalController)
        {
            _signalControllers = new List<ISignalController>()
            {
                mainMenuButtonsCommandSignalController,
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