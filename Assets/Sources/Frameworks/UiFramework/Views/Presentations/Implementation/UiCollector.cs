using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.ButtonProviders.Presentation.Implementation;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Presentation.AudioSources;
using Sources.Frameworks.UiFramework.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.Frameworks.UiFramework.Presentation.Texts;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.AudioSources;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Views.Presentations.Implementation
{
    public class UiCollector : View
    {
        [DisplayAsString(false)] [HideLabel] [Indent(8)]
        [SerializeField] private string _lebel = UiConstant.UiCollectorLabel;
        
        [TabGroup("Tab1", "Texts", true, 1)] 
        [SerializeField] private List<UiText> _uiTexts;

        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeTexts = Enable.Enable;
        
        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [UsedImplicitly]
        [SerializeField] private Enable _testLocalization = Enable.Enable; 
        
        [TabGroup("Tab1", "Texts", true, 1)] 
        [EnumToggleButtons] [EnableIf("_testLocalization", Enable.Enable)]
        [SerializeField] private Localization _localization;
        
        [TabGroup("Tab1", "Containers", true, 1)] 
        [SerializeField] private List<UiView> _uiContainers;
        
        [TabGroup("Tab1", "Containers", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive" , SdfIconType.Search)]
        [SerializeField] private Enable _includeContainers = Enable.Enable;
        
        [TabGroup("Tab1", "Buttons", true, 1)] 
        [SerializeField] private List<ButtonCommandProviderView> _buttonCommandProviders;
        
        [TabGroup("Tab1", "Buttons", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeButtons = Enable.Enable;
        
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [SerializeField] private List<UiAudioSource> _uiAudioSources;
        
        [TabGroup("Tab1", "AudioSources", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeAudioSources = Enable.Enable;
        
        [TabGroup("Tab1", "Animators", true, 1)] 
        [EnumToggleButtons] [HideLabel] [LabelText("IncludeInactive", SdfIconType.Search)]
        [SerializeField] private Enable _includeAnimators = Enable.Enable;
        
        private bool IncludeTexts => _includeTexts == Enable.Enable;
        private bool IncludeContainers => _includeContainers == Enable.Enable;
        private bool IncludeButtons => _includeButtons == Enable.Enable;
        private bool IncludeAudioSources => _includeAudioSources == Enable.Enable;
        private bool IncludeAnimators => _includeAnimators == Enable.Enable;

        public Localization Localization => _localization;
        public IReadOnlyList<ButtonCommandProviderView> UiFormButtons => _buttonCommandProviders;
        public IReadOnlyList<UiView> UiContainers => _uiContainers;
        public IReadOnlyList<IUiText> UiTexts => _uiTexts;
        public IReadOnlyList<IUiAudioSource> UiAudioSources => _uiAudioSources;
        
        [TabGroup("Tab1","Texts", true, 1)] 
        [Button(ButtonSizes.Large)] 
        private void AddTexts() =>
            _uiTexts = GetComponentsInChildren<UiText>(IncludeTexts).ToList();

        [TabGroup("Tab1","Texts", true, 1)] 
        [Button(ButtonSizes.Medium)] 
        private void ClearTexts() =>
            _uiTexts.Clear();

        [TabGroup("Tab1", "Containers", true, 1)] 
        [Button(ButtonSizes.Large)]
        private void AddContainers() =>
            _uiContainers = GetComponentsInChildren<UiView>(IncludeContainers).ToList();
        
        [TabGroup("Tab1", "Containers", true, 1)] 
        [Button(ButtonSizes.Medium)]
        private void ClearContainers() =>
            _uiContainers.Clear();
        
        [TabGroup("Tab1", "Buttons", true, 1)] 
        [Button(ButtonSizes.Large)] 
        private void AddButtons() =>
            _buttonCommandProviders = 
                GetComponentsInChildren<ButtonCommandProviderView>(IncludeButtons)
                .ToList();
        
        [TabGroup("Tab1", "Buttons", true, 1)] 
        [Button(ButtonSizes.Medium)]
        private void ClearButtons() =>
            _buttonCommandProviders.Clear();
        
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