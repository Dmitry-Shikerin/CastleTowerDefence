using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices
{
    public interface ISdkInitializeService
    {
        void GameReady();
        void EnableCallbackLogging();
        UniTask Initialize();
    }
}