using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Presentation.Implementation
{
    public class BunkerUi : PresentableView<BunkerUiPresenter>, IBunkerUi
    {
        [Required] [SerializeField] private TextView _healthText;

        public ITextView HealthText => _healthText;
    }
}