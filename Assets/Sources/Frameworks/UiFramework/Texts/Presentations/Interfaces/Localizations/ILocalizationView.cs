using System.Collections.Generic;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;

namespace Sources.PresentationsInterfaces.Views.Localizations
{
    public interface ILocalizationView
    {
        Localization Localization { get; }
        IReadOnlyList<IUiText> Texts { get; }
    }
}