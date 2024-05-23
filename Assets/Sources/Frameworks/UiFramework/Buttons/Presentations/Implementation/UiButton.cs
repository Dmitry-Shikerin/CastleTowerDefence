using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Controllers.Buttons;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Presentation.Buttons.Types;
using Sources.Frameworks.UiFramework.Presentation.Forms.Types;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.UiFramework.Presentation.Buttons
{
    public class UiButton : PresentableUiButton<UiButtonPresenter>, IUiButton
    {
        [DisplayAsString(false)] [HideLabel] [Indent(8)] [SerializeField]
        private string _labelText = UiConstant.UiButtonLabel;

        [TabGroup("Ids")] [EnableIf("_buttonId", ButtonId.Default)] [SerializeField]
        private FormId _formId;

        [TabGroup("Ids")] [EnableIf("_formId", FormId.Default)] [SerializeField]
        private ButtonId _buttonId;

        [TabGroup("Settings")] [EnableIf("_formId", FormId.Default)] [SerializeField]
        private ButtonType _buttonType;

        [TabGroup("Settings")] [SerializeField]
        private UseButtonType _useButtonType;

        [TabGroup("Settings")] [Range(0f, 1000f)] [SerializeField]
        private float _delay;

        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _onClickCommandId;

        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> enableCommandId;

        [TabGroup("Commands")] [SerializeField]
        private List<ButtonCommandId> _disableCommandId;

        public float Delay => _delay;
        public List<ButtonCommandId> OnClickCommandId => _onClickCommandId;
        public List<ButtonCommandId> EnableCommandId => enableCommandId;
        public List<ButtonCommandId> DisableCommandId => _disableCommandId;
        public UseButtonType UseButtonType => _useButtonType;
        public ButtonId ButtonId => _buttonId;
        public ButtonType ButtonType => _buttonType;
        public FormId FormId => _formId;
    }
}