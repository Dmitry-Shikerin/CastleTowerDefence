using Agava.WebUtility;
using Sources.Frameworks.YandexSdkFramework.Stickies.Interfaces;
using YG;

namespace Sources.Frameworks.YandexSdkFramework.Stickies.Implementation
{
    public class StickyService : IStickyService
    {
        public void ShowSticky()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;
            
            YandexGame.StickyAdActivity(true);
        }
    }
}