using System;
using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Implementation
{
    public class PlayerAccountAuthorizeService : IPlayerAccountAuthorizeService
    {
        public bool IsAuthorized()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return false;

            //TODO закоментил
            // if (PlayerAccount.IsAuthorized == false)
            //     return false;

            return true;
        }

        public void Authorize(Action onSuccessCallback)
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // if (PlayerAccount.IsAuthorized)
            //     return;
            //
            // PlayerAccount.Authorize(onSuccessCallback);
        }
    }
}