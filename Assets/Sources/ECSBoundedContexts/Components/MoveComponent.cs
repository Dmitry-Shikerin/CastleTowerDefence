using System;
using UnityEngine;

namespace Sources.ECSBoundedContexts.Components
{
    [Serializable]
    public struct MoveComponent
    {
        public Transform Transform;
        public Transform FromAngle;
        public Transform ToAngle;
        public float Speed;
        public bool IsFromPosition;
    }
}