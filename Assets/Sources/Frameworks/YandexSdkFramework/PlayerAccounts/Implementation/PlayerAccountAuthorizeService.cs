using System;
using Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Implementation
{
    public class PlayerAccountAuthorizeService : IPlayerAccountAuthorizeService
    {
        public bool IsAuthorized()
        {
            return true;
        }

        public void Authorize(Action onSuccessCallback)
        {
        }
    }
}