using Sources.Domain.Data;
using UnityEngine;

namespace Sources.Utils.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3Data ToVector3Data(this Vector3 vector)
        {
            return new Vector3Data { X = vector.x, Y = vector.y, Z = vector.z };
        }

        public static Vector3 ToVector3(this Vector3Data vector3Data)
        {
            return new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);
        }

    }
}