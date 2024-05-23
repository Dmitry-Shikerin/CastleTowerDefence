using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Controllers.Forms;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Presentation.CommonTypes;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.Frameworks.UiFramework.PresentationsInterfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Presentation.Views
{
    public class UiView : PresentableView<UiViewPresenter>, IUiView
    {
        [DisplayAsString(false)] [HideLabel] [Indent(8)] [SerializeField]
        private string _label = UiConstant.UiContainerLabel;

        [TitleGroup("Tabs")]
        [TabGroup("Tabs/Split","Showed Settings")] 
        [EnumToggleButtons] [HideLabel] [LabelText("Enabled GameObject")] 
        [SerializeField] private Enable _enabledGameObject;
        
        [TabGroup("Tabs/Split","Showed Settings")]
        [EnumToggleButtons] [HideLabel] [LabelText("Enabled CanvasGroup")] 
        [SerializeField] private Enable _EnabledCanvasGroup;
        
        [TabGroup("Tabs/Split","Type")] [EnumToggleButtons] [SerializeField]
        private ContainerType _containerType = ContainerType.Form;
        
        [TabGroup("Tabs/Split","Type")] 
        [EnableIf(nameof(_customFormId), CustomFormId.Default)] [SerializeField]
        private FormId _formId;
        
        [TabGroup("Tabs/Split","Type")] 
        [EnableIf(nameof(_formId), FormId.Default)] [SerializeField]
        private CustomFormId _customFormId = CustomFormId.Default;

        [TabGroup("Tabs/Split", "OnEnable")] [HideLabel] [LabelText("Enabled Forms")] 
        [SerializeField] private List<FormId> _onEnableEnabledForms;

        [TabGroup("Tabs/Split", "OnEnable")] [HideLabel] [LabelText("Disabled Forms")]
        [SerializeField] private List<FormId> _onEnableDisabledForms;

        [TabGroup("Tabs/Split", "OnDisable")] [HideLabel] [LabelText("Enabled Forms")] 
        [SerializeField] private List<FormId> _onDisableEnabledForms;

        [TabGroup("Tabs/Split", "OnDisable")] [HideLabel] [LabelText("Disabled Forms")]
        [SerializeField] private List<FormId> _onDisableDisabledForms;

        [TabGroup("Tabs/Split", "Commands")] 
        [SerializeField] private List<FormCommandId> _enabledFormCommands;

        [TabGroup("Tabs/Split", "Commands")] 
        [SerializeField] private List<FormCommandId> _disabledFormCommands;

        public Enable EnabledGameObject => _enabledGameObject;
        public Enable EnabledCanvasGroup => _EnabledCanvasGroup;
        public ContainerType ContainerType => _containerType;
        public IReadOnlyList<FormId> OnEnableEnabledForms => _onEnableEnabledForms;
        public IReadOnlyList<FormId> OnEnableDisabledForms => _onEnableDisabledForms;
        public IReadOnlyList<FormId> OnDisableEnabledForms => _onDisableEnabledForms;
        public IReadOnlyList<FormId> OnDisableDisabledForms => _onDisableDisabledForms;

        public IReadOnlyList<FormCommandId> EnabledFormCommands => _enabledFormCommands;
        public IReadOnlyList<FormCommandId> DisabledFormCommands => _disabledFormCommands;

        public FormId FormId => _formId;
        public CustomFormId CustomFormId => _customFormId;
        public bool IsActive => gameObject.activeSelf;

        [TabGroup("Tabs/Split","OnEnable")]
        [ResponsiveButtonGroup("Tabs/Split/OnEnable/Responsive")]
        [Button(ButtonSizes.Medium)]
        private void AddAllOnEnableEnabledForms() =>
            _onEnableEnabledForms = AddAllForms();
        
        [TabGroup("Tabs/Split","OnEnable")]
        [ResponsiveButtonGroup("Tabs/Split/OnEnable/Responsive")]
        [Button(ButtonSizes.Medium)]
        private void AddAllOnEnableDisabledForms() =>
            _onEnableDisabledForms = AddAllForms();
        
        [TabGroup("Tabs/Split","OnDisable")]
        [ResponsiveButtonGroup("Tabs/Split/OnDisable/Responsive")]
        [Button(ButtonSizes.Medium)]
        private void AddAllOnDisableEnabledForms() =>
            _onDisableEnabledForms = AddAllForms();
        
        [TabGroup("Tabs/Split","OnDisable")]
        [ResponsiveButtonGroup("Tabs/Split/OnDisable/Responsive")]
        [Button(ButtonSizes.Medium)]
        private void AddAllOnDisableDisabledForms() =>
            _onDisableDisabledForms = AddAllForms();

        private List<FormId> AddAllForms() =>
            Enum.GetValues(typeof(FormId)).Cast<FormId>().ToList();
    }
}