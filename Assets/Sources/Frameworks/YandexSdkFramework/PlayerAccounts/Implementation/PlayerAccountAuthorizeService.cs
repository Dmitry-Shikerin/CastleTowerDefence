using System;
using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Interfaces;
using YG;

namespace Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Implementation
{
    public class PlayerAccountAuthorizeService : IPlayerAccountAuthorizeService
    {
        public bool IsAuthorized()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return false;

            if (YandexGame.auth == false)
                return false;

            return true;
        }

        public void Authorize(Action onSuccessCallback)
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;
            
            if (YandexGame.auth)
                return;
            
            //Todo нормально ли?
            YandexGame.RequestAuth();
            YandexGame.GetDataEvent += onSuccessCallback.Invoke;
        }
    }
}