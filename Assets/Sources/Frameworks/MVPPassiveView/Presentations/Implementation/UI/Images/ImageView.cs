using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Images;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images
{
    public class ImageView : View, IImageView
    {
        [Required] [SerializeField] private Image _image;

        public float FillAmount => _image.fillAmount;
        
        public void SetSprite(Sprite sprite) =>
            _image.sprite = sprite;

        public void SetFillAmount(float fillAmount) =>
            _image.fillAmount = fillAmount;

        public UniTask FillMoveTowardsAsync(float duration, CancellationToken cancellationToken) =>
            UniTask.CompletedTask;

        public void ShowImage() => 
            _image.fillAmount = 1f;

        public void HideImage() => 
            _image.fillAmount = 0f;
    }
}