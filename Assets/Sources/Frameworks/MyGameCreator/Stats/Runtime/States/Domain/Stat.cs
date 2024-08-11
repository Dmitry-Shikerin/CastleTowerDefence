using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime
{
    [CreateAssetMenu(
        fileName = "Stat", 
        menuName = "Game Creator/Stats/Stat",
        order = 50)]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoStat.png")]

    public class Stat : ScriptableObject
    {
        [SerializeField] private string _id = "stat-id";
        [SerializeField] private StatData _data = new StatData();
        [SerializeField] private StatInfo _info = new StatInfo();

        public string ID => _id;
        public double Value => this._data.BaseValue;

    }
}