using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Sources.Frameworks.UiFramework.Animations.Presentations.Implementation
{
    public class UiAnimatorController : View
    {
        [DisplayAsString(false)] [HideLabel] [Indent(5)]
        [SerializeField] private string _label = UiConstant.UiAnimatorControllerLabel;

        [SerializeField] private List<UnityEvent> _unityAction;
    }
}