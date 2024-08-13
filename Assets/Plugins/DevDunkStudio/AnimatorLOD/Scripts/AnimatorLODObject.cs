using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevDunk.AutoLOD
{
    [DefaultExecutionOrder(1)] //TODO SET LODMANAGER
    public class AnimatorLODObject : MonoBehaviour
    {
        [Tooltip("If true, changes the skin weights of the skinned mesh renderers based on LOD")]
        public bool ChangeSkinWeights = true;
        [Tooltip("The animator component of this object, if null, uses GetComponent")]
        public Animator TrackedAnimatorComponent;
        [Tooltip("The transform to track, if null, uses this transform")]
        public Transform TrackedTransform;
        [Tooltip("The skinned mesh renderers to disable/enable, if null, this gets all children")]
        public List<SkinnedMeshRenderer> SkinnedMeshRenderers;
        [System.NonSerialized] public int FrameCountdown;

        private bool currentState;
        private float currentSpeed;
        private SkinQuality currentQuality;

        private void Awake()
        {
            if (TrackedAnimatorComponent == null)
                TrackedAnimatorComponent = GetComponent<Animator>();
            
            if (TrackedTransform == null)
                TrackedTransform = transform;
            
            if (SkinnedMeshRenderers.Count == 0)
                SkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();

            FrameCountdown = Random.Range(0, 3);
        }

        private void OnEnable() =>
            AnimatorLODManager.Instance.AddAnimator(this);

        private void OnDisable() =>
            AnimatorLODManager.Instance.RemoveAnimator(this);

        /// <summary>
        /// Disable LOD System
        /// </summary>
        public void DisableLODSystem()
        {
            TrackedAnimatorComponent.enabled = currentState = true;
            TrackedAnimatorComponent.speed = currentSpeed = 1.0f;
        }

        /// <summary>
        /// Enable LOD System
        /// </summary>
        /// <param name="maxValue">Max randomized value. Use maximum of the max LOD (for frametime stability)</param>
        public void EnableLODSystem(int maxValue = 5)
        {
            FrameCountdown = Random.Range(0, maxValue);
        }

        /// <summary>
        /// Enable animator component (if not already enabled)
        /// </summary>
        /// <param name="newFrameCount">Frames to wait</param>
        /// <param name="speed">Speed of animator</param>
        /// <param name="quality">skin quality of animator</param>
        public void EnableAnimator(int newFrameCount, int speed, SkinQuality quality)
        {
            if(currentState == false)
                TrackedAnimatorComponent.enabled = currentState = true;
            
            if(currentSpeed != speed)
                TrackedAnimatorComponent.speed = currentSpeed = speed;

            FrameCountdown = newFrameCount;

            SetMeshQuality(quality);
        }


        /// <summary>
        /// Disable animator component (if not already disabled)
        /// </summary>
        public void DisableAnimator()
        {
            if(currentState)
                TrackedAnimatorComponent.enabled = currentState = false;
            
            FrameCountdown--;
        }

        /// <summary>
        /// Set the mesh quality of the skinned mesh renderers (if not already at that quality)
        /// </summary>
        /// <param name="quality">New Skin Quality</param>
        public void SetMeshQuality(SkinQuality quality)
        {
            if(ChangeSkinWeights == false || currentQuality == quality)
                return;

            for (int i = 0; i < SkinnedMeshRenderers.Count; i++)
            {
                SkinnedMeshRenderers[i].quality = currentQuality = quality;
            }
        }
    }
}