using System;
using Sources.Frameworks.GameServices.ActionRegisters.Implementation;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces;

namespace Sources.Frameworks.GameServices.UpdateServices.Implementation
{
    public class UpdateService : ActionRegisterer<float>, IUpdateRegister, IUpdateService
    {
        public event Action<float> UpdateChanged;
        
        public void Update(float deltaTime)
        {
            UpdateChanged?.Invoke(deltaTime);
            
            if(Actions.Count == 0)
                return;
            
            for (int i = Actions.Count - 1; i >= 0; i--)
                Actions[i].Invoke(deltaTime);
        }
    }
}