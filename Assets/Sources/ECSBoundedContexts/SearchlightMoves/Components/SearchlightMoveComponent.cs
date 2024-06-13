using System;
using UnityEngine;

namespace Sources.ECSBoundedContexts.SearchlightMoves.Components
{
    [Serializable]
    public struct SearchlightMoveComponent
    {
        public Transform Transform;
        public Transform FromAngle;
        public Transform ToAngle;
        public float Speed;
        public bool IsFromPosition;
    }
}