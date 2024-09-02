using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.ECSBoundedContexts.SearchlightMoves.Domain.Components
{
    [Serializable]
    public struct SearchlightMoveComponent
    {
        public Transform Transform;
        [MinMaxSlider(-180, 180, true)]
        public Vector2 Angle;
        [Range(2, 8)]
        public float Speed;
        public bool IsFromPosition;
    }
}