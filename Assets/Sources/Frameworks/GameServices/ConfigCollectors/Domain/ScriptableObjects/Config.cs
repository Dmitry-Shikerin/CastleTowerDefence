using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Core.Presentation.CommonTypes;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects
{
    public class Config : SerializedScriptableObject
    {
        [EnumToggleButtons] [HideLabel] [UsedImplicitly]
        [SerializeField] private Enable _enable = Enable.Disable;
        [EnableIf(nameof(_enable), Enable.Enable)]
        [SerializeField] private string _id;
        [EnableIf(nameof(_enable), Enable.Enable)]
        [SerializeField] private ScriptableObject _parent;
        
        public ScriptableObject Parent => _parent;
        public string Id => _id;
        
        public void SetId(string id) =>
            _id = id;
        
        public void SetParent(ScriptableObject parent) =>
            _parent = parent;
    }
}