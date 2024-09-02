using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Texts.Presentations.Views.Implementation;
using Sources.Frameworks.UiFramework.Texts.Presentations.Views.Interfaces;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation.Types;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Views.Presentations.Implementation
{
    public class UiCollector : View
    {
        [DisplayAsString(false)] 
        [HideLabel] 
        [Indent(8)]
        [SerializeField] private string _lebel = UiConstant.UiCollectorLabel;
        [TabGroup("Tab1", "Texts", true, 1)] 
        [SerializeField] private List<UiLocalizationText> _uiTexts;        
        [TabGroup("Tab1", "Texts", true, 1)] 
        [SerializeField] private List<UiLocalizationSprite> _uiLocalizationSprites;
        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeTexts = Enable.Enable;
        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [UsedImplicitly]
        [SerializeField] private Enable _testLocalization = Enable.Enable; 
        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [EnableIf("_testLocalization", Enable.Enable)]
        [SerializeField] private Localization _localization;
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [SerializeField] private List<UiAudioSource> _uiAudioSources;
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [EnumToggleButtons] 
        [HideLabel] 
        [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeAudioSources = Enable.Enable;
        
        private bool IncludeTexts => _includeTexts == Enable.Enable;
        private bool IncludeAudioSources => _includeAudioSources == Enable.Enable;

        public Localization Localization => _localization;
        public IReadOnlyList<IUiLocalizationText> UiTexts => _uiTexts;
        public IReadOnlyList<UiLocalizationSprite> UiLocalizationSprites => _uiLocalizationSprites;
        public IReadOnlyList<IUiAudioSource> UiAudioSources => _uiAudioSources;
        
        [TabGroup("Tab1","Texts", true, 1)] 
        [Button(ButtonSizes.Large)] 
        private void AddTexts() =>
            _uiTexts = GetComponentsInChildren<UiLocalizationText>(IncludeTexts).ToList();  
        
        [TabGroup("Tab1","Texts", true, 1)] 
        [Button(ButtonSizes.Large)] 
        private void AddSprites() =>
            _uiLocalizationSprites = GetComponentsInChildren<UiLocalizationSprite>(IncludeTexts).ToList();

        [TabGroup("Tab1","Texts", true, 1)] 
        [Button(ButtonSizes.Medium)] 
        private void ClearTexts() =>
            _uiTexts.Clear();
        
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [Button(ButtonSizes.Large)]
        private void AddAudioSources() =>
            _uiAudioSources = GetComponentsInChildren<UiAudioSource>(IncludeAudioSources).ToList();
        
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [Button(ButtonSizes.Medium)]
        private void ClearAudioSources() =>
            _uiAudioSources.Clear();
    }
}