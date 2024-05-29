using UnityEngine;

namespace Sources.BoundedContexts.Layers.Domain
{
    public class LayerConst
    {
        public static readonly int s_defaul = 0;
        public static readonly int s_player = 1 << LayerMask.NameToLayer("Player");
        public static readonly int s_enemy = 1 << LayerMask.NameToLayer("Enemy");
    }
}