using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [CreateAssetMenu(
        fileName = "Attribute", 
        menuName = "Game Creator/Stats/Attribute",
        order = 50)]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoAttribute.png")]
    
    public class Attribute : ScriptableObject
    {
        [SerializeField] private string _id = "attribute-id";
        [SerializeField] private AttributeData _data = new AttributeData();
        [SerializeField] private AttributeInfo _info = new AttributeInfo();

        public string ID => _id;
        public AttributeInfo Info => _info;
        public AttributeData Data => _data;
    }
}