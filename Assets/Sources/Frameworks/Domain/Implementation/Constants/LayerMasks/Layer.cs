using UnityEngine;

namespace Sources.Domain.Models.Constants.LayerMasks
{
    public static class Layer
    {
        public static readonly int Default = 0;
        public static readonly int Plane = 1 << LayerMask.NameToLayer("Plane");
        public static readonly int Enemy = 1 << LayerMask.NameToLayer("Enemy");
        public static readonly int Character = 1 << LayerMask.NameToLayer("Player");
        public static readonly int Obstacle = 1 << LayerMask.NameToLayer("Obstacle");
    }
}