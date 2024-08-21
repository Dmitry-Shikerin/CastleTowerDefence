using UnityEngine;

namespace DevDunk.AutoLOD
{
    [System.Serializable]
    public struct LODSettings
    {
        public float Distance;
        public int frameCount;
        public SkinQuality MaxBoneWeight;
    }
}