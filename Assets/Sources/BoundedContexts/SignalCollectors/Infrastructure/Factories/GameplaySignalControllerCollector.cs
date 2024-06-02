using System.Collections.Generic;
using Sources.BoundedContexts.SignalCollectors.Controllers;
using Sources.Frameworks.GameServices.DoozySignalBuses.Controllers.Interfaces;

namespace Sources.BoundedContexts.SignalCollectors.Infrastructure.Factories
{
    public class GameplaySignalControllerCollector : ISignalControllersCollector
    {
        private readonly List<ISignalController> _signalControllers;


        public GameplaySignalControllerCollector(
            GameplayButtonsCommandSignalController mainMenuButtonsCommandSignalController,
            AudioServiceSignalController audioServiceSignalController)
        {
            _signalControllers = new List<ISignalController>()
            {
                mainMenuButtonsCommandSignalController,
                audioServiceSignalController,
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