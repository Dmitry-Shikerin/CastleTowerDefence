using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors
{
    public class Config : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private ScriptableObject _parent;
        public string Id => _id;
        
        public void SetId(string id) =>
            _id = id;
        
        public void SetParent(ScriptableObject parent) =>
            _parent = parent;
    }
}