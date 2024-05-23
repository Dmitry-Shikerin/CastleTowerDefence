using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.PresentationsInterfaces.UI.Images
{
    public interface IImageView
    {
        float FillAmount { get; }
        
        void SetSprite(Sprite sprite);
        void SetFillAmount(float fillAmount);
        UniTask FillMoveTowardsAsync(float duration, CancellationToken cancellationToken);
        void ShowImage();
        void HideImage();
    }
}