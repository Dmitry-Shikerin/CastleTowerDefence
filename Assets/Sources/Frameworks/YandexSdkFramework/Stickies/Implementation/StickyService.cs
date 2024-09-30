using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.Stickies.Interfaces;

namespace Sources.Frameworks.YandexSdkFramework.Stickies.Implementation
{
    public class StickyService : IStickyService
    {
        public void ShowSticky()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            //TODO закоментил
            // StickyAd.Show();
        }
    }
}