using Agava.WebUtility;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.YandexSdkFramework.SdcInitializes.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.SdcInitializes.Implementation
{
    public class SdkInitializeService : ISdkInitializeService
    {
        public void GameReady()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // YandexGamesSdk.GameReady();
        }

        public void EnableCallbackLogging()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // if (YandexGamesSdk.CallbackLogging)
            //     return;
            //
            // YandexGamesSdk.CallbackLogging = true;
        }

        public async UniTask Initialize()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // if (YandexGamesSdk.IsInitialized)
            //     return;
            //
            // await YandexGamesSdk.Initialize();
        }
    }
}