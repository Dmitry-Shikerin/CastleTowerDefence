using UnityEngine;

namespace Sources.BoundedContexts.Layers.Domain
{
    public class LayerConst
    {
        public static readonly int Defaul = 0;
        public static readonly int Player = 1 << LayerMask.NameToLayer("Player");
        public static readonly int Enemy = 1 << LayerMask.NameToLayer("Enemy");
    }
}