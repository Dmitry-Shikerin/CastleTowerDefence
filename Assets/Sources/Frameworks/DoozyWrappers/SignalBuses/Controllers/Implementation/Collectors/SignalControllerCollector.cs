﻿using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Implementation
{
    public class SignalControllerCollector : ISignalControllersCollector
    {
        private readonly List<ISignalController> _signalControllers;
        
        public SignalControllerCollector(
            ButtonsCommandSignalController buttonsCommandSignalController,
            AudioServiceSignalController audioServiceSignalController)
        {
            _signalControllers = new List<ISignalController>()
            {
                buttonsCommandSignalController,
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