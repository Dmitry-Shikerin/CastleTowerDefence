using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation.Collectors
{
    public class SignalControllerCollector : ISignalControllersCollector
    {
        private readonly List<ISignalController> _signalControllers;
        
        public SignalControllerCollector(
            ButtonsCommandSignalController buttonsCommandSignalController,
            ViewCommandSignalController viewCommandSignalController)
        {
            _signalControllers = new List<ISignalController>()
            {
                buttonsCommandSignalController,
                viewCommandSignalController,
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