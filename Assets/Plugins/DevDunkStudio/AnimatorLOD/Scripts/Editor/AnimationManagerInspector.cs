using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DevDunk.AutoLOD.EditorUtils
{
    [CustomEditor(typeof(AnimatorLODManager))]
    public class AnimationManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimatorLODManager animationManager = (AnimatorLODManager)target;

            if (GUILayout.Button("Order LODs by distance"))
            {
                // Sort the AnimationTiers array by Distance value
                System.Array.Sort(animationManager.LODs, (a, b) => a.Distance.CompareTo(b.Distance));

                //Mark dirty to update UI
                EditorUtility.SetDirty(target);
            }
        }
    }
}