using System;

namespace Sources.Frameworks.YandexSdkFramework.PlayerAccounts.Interfaces
{
    public interface IPlayerAccountAuthorizeService
    {
        bool IsAuthorized();
        void Authorize(Action onSuccessCallback);
    }
}