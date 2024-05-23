using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Sources.Frameworks.UiFramework.Presentation.Animations
{
    public class UiAnimatorController : View
    {
        [DisplayAsString(false)] [HideLabel] [Indent(5)]
        [SerializeField] private string _label = UiConstant.UiAnimatorControllerLabel;

        [SerializeField] private List<UnityEvent> _unityAction;
        
        private async void Awake()
        {
            // if(_unityAction[0] is Func<UniTask> action)
            //     await action.Invoke();
            
            // await _unityAction[0].OnInvokeAsync(new CancellationToken());
        }

        //TODO придумать сборку аниматоров
    }
}