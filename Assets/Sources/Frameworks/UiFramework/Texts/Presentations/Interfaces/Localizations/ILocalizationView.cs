using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.PresentationsInterfaces.UI.Texts;

namespace Sources.PresentationsInterfaces.Views.Localizations
{
    public interface ILocalizationView
    {
        Localization Localization { get; }
        IReadOnlyList<IUiText> Texts { get; }
    }
}