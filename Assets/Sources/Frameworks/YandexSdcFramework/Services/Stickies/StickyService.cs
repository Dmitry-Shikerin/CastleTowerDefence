using Agava.WebUtility;
using Agava.YandexGames;

namespace Sources.Frameworks.YandexSdcFramework.Services.Stickies
{
    public class StickyService : IStickyService
    {
        public void ShowSticky()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;

            StickyAd.Show();
        }
    }
}