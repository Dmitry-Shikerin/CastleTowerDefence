using System;

namespace Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts
{
    public interface IPlayerAccountAuthorizeService
    {
        bool IsAuthorized();
        void Authorize(Action onSuccessCallback);
    }
}