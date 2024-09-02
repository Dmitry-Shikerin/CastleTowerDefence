using Cysharp.Threading.Tasks;

namespace Sources.Frameworks.YandexSdkFramework.SdcInitializes.Interfaces
{
    public interface ISdkInitializeService
    {
        void GameReady();
        void EnableCallbackLogging();
        UniTask Initialize();
    }
}