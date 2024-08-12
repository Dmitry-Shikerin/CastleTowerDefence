using System;
using Sources.Frameworks.MyGameCreator.Core.Runtime.Common;
using Sources.Frameworks.MyGameCreator.Stats.Runtime.Classes;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Stats.Runtime.Traitses
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Creator/Stats/Traits")]
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoTraits.png")]
    public class Traits : MonoBehaviour
    {
        private const string ERR_NO_CLASS = "Traits component has no Class reference";

        [SerializeField] private Class _class;

        private RuntimeClass _runtimeClass;

        private void Awake()
        {
            _runtimeClass = new RuntimeClass(_class);
        }
    }
}