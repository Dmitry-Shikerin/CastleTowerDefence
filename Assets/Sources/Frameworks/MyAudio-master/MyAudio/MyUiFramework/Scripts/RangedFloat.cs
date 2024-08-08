using System;

namespace MyAudios.Scripts
{
    /// <summary>
    ///     Struct that contains a minimum and a maximum values that define a float interval
    /// </summary>
    [Serializable]
    public struct RangedFloat
    {
        /// <summary>
        ///     Minimum value for the float interval
        /// </summary>
        public float MinValue;

        /// <summary>
        ///     Maximum value for the float interval
        /// </summary>
        public float MaxValue;
    }
}