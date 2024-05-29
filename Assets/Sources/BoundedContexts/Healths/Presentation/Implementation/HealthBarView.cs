using Sirenix.OdinInspector;
using Sources.BoundedContexts.Healths.Controllers;
using Sources.BoundedContexts.Healths.Presentation.Interfaces;
using Sources.Presentations.UI.Images;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.UI.Images;
using UnityEngine;

namespace Sources.BoundedContexts.Healths.Presentation.Implementation
{
    public class HealthBarView : PresentableView<HealthBarPresenter>, IHealthBarView
    {
        [Required] [SerializeField] private ImageView _healthBarImage;
        
        public IImageView HealthBarImage => _healthBarImage;
    }
}