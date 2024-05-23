using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.SdcInitializeServices
{
    public interface ISdcInitializeService
    {
        void GameReady();
        void EnableCallbackLogging();
        UniTask Initialize();
    }
}