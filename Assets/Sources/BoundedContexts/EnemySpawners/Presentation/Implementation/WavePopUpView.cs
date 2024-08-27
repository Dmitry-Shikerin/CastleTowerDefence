using Agava.WebUtility;
using Agava.YandexGames;
using Doozy.Runtime.Reactor.Animators;
using Sirenix.OdinInspector;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class WavePopUpView : View
    {
        [Required] [SerializeField] private UIAnimator _popUpAnimator;
        [Required] [SerializeField] private ImageView _image;
        [Required] [SerializeField] private Sprite _rus;
        [Required] [SerializeField] private Sprite _eng;
        [Required] [SerializeField] private Sprite _tur;
        
        private void OnEnable()
        {
            if (WebApplication.IsRunningOnWebGL == false)
                return;
            
            string languageCode = YandexGamesSdk.Environment.i18n.lang;

            switch (languageCode)
            {
                case LocalizationConst.English:
                    _image.SetSprite(_eng);
                    break;
                case LocalizationConst.Turkish:
                    _image.SetSprite(_eng);
                    break;
                case LocalizationConst.Russian:
                    _image.SetSprite(_rus);
                    break;
                default:
                    _image.SetSprite(_eng);
                    break;
            }
        }

        public void PlayAnimation() => 
            _popUpAnimator.Play();
    }
}