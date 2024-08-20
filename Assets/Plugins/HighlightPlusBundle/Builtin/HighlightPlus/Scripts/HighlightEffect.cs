/// <summary>
/// Highlight Plus - (c) Kronnect Technologies SL
/// </summary>

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

namespace HighlightPlus {

    public delegate bool OnObjectHighlightEvent(GameObject obj);
    public delegate bool OnObjectHighlightStateEvent(GameObject obj, bool state);
    public delegate bool OnObjectSelectionEvent(GameObject obj);
    public delegate bool OnRendererHighlightEvent(Renderer renderer);

    /// <summary>
    /// Triggers when target effect animation occurs
    /// </summary>
    /// <param name="t">A value from 0 to 1 that represent the animation time from start to end, based on target duration and start time</param>
    public delegate void OnTargetAnimatesEvent(ref Vector3 center, ref Quaternion rotation, ref Vector3 scale, float t);

    public enum NormalsOption {
        Smooth = 0,
        PreserveOriginal = 1,
        Reorient = 2,
        Planar = 3
    }

    public enum SeeThroughMode {
        WhenHighlighted = 0,
        AlwaysWhenOccluded = 1,
        Never = 2
    }

    public enum SeeThroughSortingMode {
        Default = 0,
        SortByMaterialsRenderQueue = 1,
        SortByMaterialsRenderQueueInverted = 2
    }

    public enum OverlayMode {
        WhenHighlighted = 0,
        Always = 10
    }

    public enum TextureUVSpace {
        Triplanar = 0,
        ObjectSpace = 1,
        ScreenSpace = 2
    }

    public enum QualityLevel {
        Fastest = 0,
        High = 1,
        Highest = 2,
        Medium = 3
    }

    public enum TargetOptions {
        Children,
        OnlyThisObject,
        RootToChildren,
        LayerInScene,
        LayerInChildren,
        Scripting
    }

    public enum Visibility {
        Normal,
        AlwaysOnTop,
        OnlyWhenOccluded
    }

    public enum ColorStyle {
        SingleColor,
        Gradient
    }

    public enum OutlineEdgeMode {
        Exterior,
        Any
    }

    public enum ContourStyle {
        AroundVisibleParts,
        AroundObjectShape
    }

    public enum GlowBlendMode {
        Additive,
        AlphaBlending
    }

    public enum GlowDitheringStyle {
        Pattern,
        Noise
    }

    public enum InnerGlowBlendMode {
        Additive,
        AlphaBlending
    }

    public enum BlurMethod {
        Gaussian,
        Kawase
    }

    public static class QualityLevelExtensions {
        public static bool UsesMultipleOffsets(this QualityLevel qualityLevel) {
            return qualityLevel == QualityLevel.Medium || qualityLevel == QualityLevel.High;
        }
    }

    [Serializable]
    public struct GlowPassData {
        public float offset;
        public float alpha;
        public Color color;
    }

    [ExecuteAlways]
    [HelpURL("https://kronnect.com/guides/highlight-plus-introduction/")]
    public partial class HighlightEffect : MonoBehaviour {

        /// <summary>
        /// Gets or sets the current profile. To load a profile and apply its settings at runtime, please use ProfileLoad() method.
        /// </summary>
        [Tooltip("The current profile (optional). A profile let you store Highlight Plus settings and apply those settings easily to many objects. You can also load a profile and apply its settings at runtime, using the ProfileLoad() method of the Highlight Effect component.")]
        public HighlightProfile profile;

        /// <summary>
        /// Sets if changes to the original profile should propagate to this effect.
        /// </summary>
        [Tooltip("If enabled, settings from the profile will be applied to this component automatically when game starts or when any profile setting is updated.")]
        public bool profileSync;

        /// <summary>
        /// Makes the effects visible in the SceneView.
        /// </summary>
        [Tooltip("If enabled, effects will be visible also when not in Play mode.")]
        public bool previewInEditor = true;

        /// <summary>
        /// Which cameras can render the effects
        /// </summary>
        [Tooltip("Which cameras can render the effect.")]
        public LayerMask camerasLayerMask = -1;

        /// <summary>
        /// Specifies which objects are affected by this effect.
        /// </summary>
        [Tooltip("Different options to specify which objects are affected by this Highlight Effect component.")]
        public TargetOptions effectGroup = TargetOptions.Children;

        /// <summary>
        /// The layer that contains the affected objects by this effect when effectGroup is set to LayerMask.
        /// </summary>
        [Tooltip("The layer that contains the affected objects by this effect when effectGroup is set to LayerMask.")]
        public LayerMask effectGroupLayer = -1;

        /// <summary>
        /// Optional object name filter
        /// </summary>
        [Tooltip("Only include objects whose names contains this text.")]
        public string effectNameFilter;

        /// <summary>
        /// Optional object name exclusion filter
        /// </summary>
        [Tooltip("Use RegEx to determine if an object name matches the effectNameFilter.")]
        public bool effectNameUseRegEx;

        /// <summary>
        /// Combine objects into a single mesh
        /// </summary>
        [Tooltip("Combine meshes of all objects in this group affected by Highlight Effect reducing draw calls.")]
        public bool combineMeshes;

        /// <summary>
        /// The alpha threshold for transparent cutout objects. Pixels with alpha below this value will be discarded.
        /// </summary>
        [Tooltip("The alpha threshold for transparent cutout objects. Pixels with alpha below this value will be discarded.")]
        [Range(0, 1)]
        public float alphaCutOff;

        /// <summary>
        /// If back facing triangles are ignored. Backfaces triangles are not visible but you may set this property to false to force highlight effects to act on those triangles as well.
        /// </summary>
        [Tooltip("If back facing triangles are ignored.Backfaces triangles are not visible but you may set this property to false to force highlight effects to act on those triangles as well.")]
        public bool cullBackFaces = true;

        [Tooltip("Adds a empty margin between the mesh and the effects")]
        public float padding;

        /// <summary>
        /// Show highlight effects even if the object is currently not visible. This option is useful if the affected objects are rendered using GPU instancing tools which render directly to the GPU without creating real game object geometry in CPU.
        /// </summary>
        [Tooltip("Show highlight effects even if the object is not visible. If this object or its children use GPU Instancing tools, the MeshRenderer can be disabled although the object is visible. In this case, this option is useful to enable highlighting.")]
        public bool ignoreObjectVisibility;

        /// <summary>
        /// Enable to support reflection probes
        /// </summary>
        [Tooltip("Support reflection probes. Enable only if you want the effects to be visible in reflections.")]
        public bool reflectionProbes;

        /// <summary>
        /// Enable to support reflection probes
        /// </summary>
        [Tooltip("Enables GPU instancing. Reduces draw calls in outline and outer glow effects on platforms that support GPU instancing. Should be enabled by default.")]
        public bool GPUInstancing = true;

        /// <summary>
        /// Enable to support reflection probes
        /// </summary>
        [Tooltip("Bakes skinned mesh to leverage GPU instancing when using outline/outer glow with mesh-based rendering. Reduces draw calls significantly on skinned meshes.")]
        public bool optimizeSkinnedMesh = true;

        /// <summary>
        /// Enabled depth buffer flip in HQ
        /// </summary>
        [Tooltip("Enables depth buffer clipping. Only applies to outline or outer glow in High Quality mode.")]
        public bool depthClip;

        [Tooltip("Fades out effects based on distance to camera")]
        public bool cameraDistanceFade;

        [Tooltip("The closest distance particles can get to the camera before they fade from the camera’s view.")]
        public float cameraDistanceFadeNear;

        [Tooltip("The farthest distance particles can get away from the camera before they fade from the camera’s view.")]
        public float cameraDistanceFadeFar = 1000;

        [Tooltip("Normals handling option:\nPreserve original: use original mesh normals.\nSmooth: average normals to produce a smoother outline/glow mesh based effect.\nReorient: recomputes normals based on vertex direction to centroid.\nPlanar: same than reorient but renders outline and glow in an optimized way for 2D or planar meshes like quads or planes.")]
        public NormalsOption normalsOption;

        /// <summary>
        /// Ignores highlight effects on this object.
        /// </summary>
        [Tooltip("Ignore highlighting on this object.")]
        public bool ignore;

        [SerializeField]
        bool _highlighted;

        public bool highlighted { get { return _highlighted; } set { SetHighlighted(value); } }

        public float fadeInDuration;
        public float fadeOutDuration;

        public bool flipY;

        [Tooltip("Keeps the outline/glow size unaffected by object distance.")]
        public bool constantWidth = true;

        [Tooltip("Minimum width when the constant width option is not used")]
        [Range(0, 1)]
        public float minimumWidth;

        [Tooltip("Mask to include or exclude certain submeshes. By default, all submeshes are included.")]
        public int subMeshMask = -1;

        [Range(0, 1)]
        [Tooltip("Intensity of the overlay effect. A value of 0 disables the overlay completely.")]
        public float overlay;
        public OverlayMode overlayMode = OverlayMode.WhenHighlighted;
        [ColorUsage(true, true)] public Color overlayColor = Color.yellow;
        public float overlayAnimationSpeed = 1f;
        [Range(0, 1)]
        public float overlayMinIntensity = 0.5f;
        [Range(0, 1)]
        [Tooltip("Controls the blending or mix of the overlay color with the natural colors of the object.")]
        public float overlayBlending = 1.0f;
        [Tooltip("Optional overlay texture.")]
        public Texture2D overlayTexture;
        public TextureUVSpace overlayTextureUVSpace;
        public float overlayTextureScale = 1f;
        public Vector2 overlayTextureScrolling;
        public Visibility overlayVisibility = Visibility.Normal;

        [Range(0, 1)]
        [Tooltip("Intensity of the outline. A value of 0 disables the outline completely.")]
        public float outline = 1f;
        [ColorUsage(true, true)] public Color outlineColor = Color.black;
        public ColorStyle outlineColorStyle = ColorStyle.SingleColor;
        [GradientUsage(hdr: true, ColorSpace.Linear)] public Gradient outlineGradient;
        public bool outlineGradientInLocalSpace;
        public float outlineWidth = 0.45f;
        [Range(1, 3)]
        public int outlineBlurPasses = 2;
        public QualityLevel outlineQuality = QualityLevel.Medium;
        public OutlineEdgeMode outlineEdgeMode = OutlineEdgeMode.Exterior;
        public float outlineEdgeThreshold = 0.995f;
        public float outlineSharpness = 1f;
        [Range(1, 8)]
        [Tooltip("Reduces the quality of the outline but improves performance a bit.")]
        public int outlineDownsampling = 1;
        public Visibility outlineVisibility = Visibility.Normal;
        public GlowBlendMode glowBlendMode = GlowBlendMode.Additive;
        public bool outlineOptimalBlit = true;
        public bool outlineBlitDebug;
        [Tooltip("If enabled, this object won't combine the outline with other objects.")]
        public bool outlineIndependent;
        public ContourStyle outlineContourStyle = ContourStyle.AroundVisibleParts;

        [Range(0, 5)]
        [Tooltip("The intensity of the outer glow effect. A value of 0 disables the glow completely.")]
        public float glow;
        public float glowWidth = 0.4f;
        public QualityLevel glowQuality = QualityLevel.Medium;
        public BlurMethod glowBlurMethod = BlurMethod.Gaussian;
        [Range(1, 8)]
        [Tooltip("Reduces the quality of the glow but improves performance a bit.")]
        public int glowDownsampling = 2;
        [ColorUsage(true, true)] public Color glowHQColor = new Color(0.64f, 1f, 0f, 1f);
        [Tooltip("When enabled, outer glow renders with dithering. When disabled, glow appears as a solid color.")]
        [Range(0, 1)]
        public float glowDithering = 1f;
        public GlowDitheringStyle glowDitheringStyle = GlowDitheringStyle.Pattern;
        [Tooltip("Seed for the dithering effect")]
        public float glowMagicNumber1 = 0.75f;
        [Tooltip("Another seed for the dithering effect that combines with first seed to create different patterns")]
        public float glowMagicNumber2 = 0.5f;
        public float glowAnimationSpeed = 1f;
        public Visibility glowVisibility = Visibility.Normal;
        [Tooltip("Performs a blit to screen only over the affected area, instead of a full-screen pass")]
        public bool glowOptimalBlit = true;
        public bool glowBlitDebug;
        [Tooltip("Blends glow passes one after another. If this option is disabled, glow passes won't overlap (in this case, make sure the glow pass 1 has a smaller offset than pass 2, etc.)")]
        public bool glowBlendPasses = true;
#if UNITY_2020_2_OR_NEWER
        [NonReorderable]
#endif
        public GlowPassData[] glowPasses;
        [Tooltip("If enabled, glow effect will not use a stencil mask. This can be used to render the glow effect alone.")]
        public bool glowIgnoreMask;

        [Range(0, 5f)]
        [Tooltip("The intensity of the inner glow effect. A value of 0 disables the glow completely.")]
        public float innerGlow;
        [Range(0, 2)]
        public float innerGlowWidth = 1f;
        [ColorUsage(true, true)] public Color innerGlowColor = Color.white;
        public InnerGlowBlendMode innerGlowBlendMode = InnerGlowBlendMode.Additive;
        public Visibility innerGlowVisibility = Visibility.Normal;

        [Tooltip("Enables the targetFX effect. This effect draws an animated sprite over the object.")]
        public bool targetFX;
        public Texture2D targetFXTexture;
        [ColorUsage(true, true)] public Color targetFXColor = Color.white;
        public Transform targetFXCenter;
        public float targetFXRotationSpeed = 50f;
        public float targetFXInitialScale = 4f;
        public float targetFXEndScale = 1.5f;
        [Tooltip("Makes target scale relative to object renderer bounds")]
        public bool targetFXScaleToRenderBounds = true;
        [Tooltip("Enable to render a single target FX effect at the center of the enclosing bounds")]
        public bool targetFXUseEnclosingBounds;
        [Tooltip("Places target FX sprite at the bottom of the highlighted object.")]
        public bool targetFXAlignToGround;
        [Tooltip("Optional worlds space offset for the position of the targetFX effect")]
        public Vector3 targetFXOffset;
        [Tooltip("Fade out effect with altitude")]
        public float targetFXFadePower = 32;
        public float targetFXGroundMaxDistance = 10f;
        public LayerMask targetFXGroundLayerMask = -1;
        public float targetFXTransitionDuration = 0.5f;
        [Tooltip("The duration of the effect. A value of 0 will keep the target sprite on screen while object is highlighted.")]
        public float targetFXStayDuration = 1.5f;
        public Visibility targetFXVisibility = Visibility.AlwaysOnTop;

        public event OnObjectSelectionEvent OnObjectSelected;
        public event OnObjectSelectionEvent OnObjectUnSelected;
        public event OnObjectHighlightEvent OnObjectHighlightStart;
        public event OnObjectHighlightEvent OnObjectHighlightEnd;
        public event OnObjectHighlightStateEvent OnObjectHighlightStateChange;
        public event OnRendererHighlightEvent OnRendererHighlightStart;
        public event OnTargetAnimatesEvent OnTargetAnimates;

        [Tooltip("See-through mode for this Highlight Effect component.")]
        public SeeThroughMode seeThrough = SeeThroughMode.Never;
        [Tooltip("This mask setting let you specify which objects will be considered as occluders and cause the see-through effect for this Highlight Effect component. For example, you assign your walls to a different layer and specify that layer here, so only walls and not other objects, like ground or ceiling, will trigger the see-through effect.")]
        public LayerMask seeThroughOccluderMask = -1;
        [Tooltip("A multiplier for the occluder volume size which can be used to reduce the actual size of occluders when Highlight Effect checks if they're occluding this object.")]
        [Range(0.01f, 0.6f)] public float seeThroughOccluderThreshold = 0.3f;
        [Tooltip("Uses stencil buffers to ensure pixel-accurate occlusion test. If this option is disabled, only physics raycasting is used to test for occlusion.")]
        public bool seeThroughOccluderMaskAccurate;
        [Tooltip("The interval of time between occlusion tests.")]
        public float seeThroughOccluderCheckInterval = 1f;
        [Tooltip("If enabled, occlusion test is performed for each children element. If disabled, the bounds of all children is combined and a single occlusion test is performed for the combined bounds.")]
        public bool seeThroughOccluderCheckIndividualObjects;
        [Tooltip("Shows the see-through effect only if the occluder if at this 'offset' distance from the object.")]
        public float seeThroughDepthOffset;
        [Tooltip("Hides the see-through effect if the occluder is further than this distance from the object (0 = infinite)")]
        public float seeThroughMaxDepth;
        [Range(0, 5f)] public float seeThroughIntensity = 0.8f;
        [Range(0, 1)] public float seeThroughTintAlpha = 0.5f;
        [ColorUsage(true, true)] public Color seeThroughTintColor = Color.red;
        [Range(0, 1)] public float seeThroughNoise = 1f;
        [Range(0, 1)] public float seeThroughBorder;
        public Color seeThroughBorderColor = Color.black;
        [Tooltip("Only display the border instead of the full see-through effect.")]
        public bool seeThroughBorderOnly;
        public float seeThroughBorderWidth = 0.45f;
        [Tooltip("Renders see-through effect on overlapping objects in a sequence that's relative to the distance to the camera")]
        public bool seeThroughOrdered;
        [Tooltip("Optional see-through mask effect texture.")]
        public Texture2D seeThroughTexture;
        public TextureUVSpace seeThroughTextureUVSpace;
        public float seeThroughTextureScale = 1f;
        [Tooltip("The order by which children objects are rendered by the see-through effect")]
        public SeeThroughSortingMode seeThroughChildrenSortingMode = SeeThroughSortingMode.Default;

        struct ModelMaterials {
            public bool render; // if this object can render this frame
            public Transform transform;
            public bool bakedTransform;
            public Vector3 currentPosition, currentRotation, currentScale;
            public bool renderWasVisibleDuringSetup;
            public Mesh mesh, originalMesh, bakedSkinnedMesh;
            public Renderer renderer;
            public bool isSkinnedMesh;
            public NormalsOption normalsOption;
            public Material[] fxMatMask, fxMatSolidColor, fxMatSeeThroughInner, fxMatSeeThroughBorder, fxMatOverlay, fxMatInnerGlow;
            public Matrix4x4 renderingMatrix;
            public bool isCombined;
            public bool preserveOriginalMesh { get { return !isCombined && normalsOption == NormalsOption.PreserveOriginal; } }

            public void Init() {
                render = false;
                transform = null;
                bakedTransform = false;
                currentPosition = currentRotation = currentScale = Vector3.zero;
                mesh = originalMesh = null;
                if (bakedSkinnedMesh != null) DestroyImmediate(bakedSkinnedMesh);
                renderer = null;
                isSkinnedMesh = false;
                normalsOption = NormalsOption.Smooth;
                isCombined = false;
            }
        }

        public enum FadingState {
            FadingOut = -1,
            NoFading = 0,
            FadingIn = 1
        }

        [SerializeField, HideInInspector]
        ModelMaterials[] rms;
        [SerializeField, HideInInspector]
        int rmsCount;

        /// <summary>
        /// Number of objects affected by this highlight effect script
        /// </summary>
        public int includedObjectsCount => rmsCount;

#if UNITY_EDITOR
        /// <summary>
        /// True if there's some static children
        /// </summary>
        [NonSerialized]
        public bool staticChildren;

#endif

        [NonSerialized]
        public Transform target;

        /// <summary>
        /// Time in which the highlight started
        /// </summary>
        [NonSerialized]
        public float highlightStartTime;

        /// <summary>
        /// Time in which the target fx started
        /// </summary>
        [NonSerialized]
        public float targetFxStartTime;

        bool _isSelected;
        /// <summary>
        /// True if this object is selected (if selectOnClick is used)
        /// </summary>
        public bool isSelected {
            get { return _isSelected; }
            set {
                if (_isSelected != value) {
                    if (value) {
                        if (OnObjectSelected != null) OnObjectSelected(gameObject);
                    } else {
                        if (OnObjectUnSelected != null) OnObjectUnSelected(gameObject);
                    }
                    _isSelected = value;
                    if (_isSelected) lastSelected = this;
                }
            }
        }

        /// <summary>
        /// If a sprite is used with this script, spriteMode = true. Certain mesh-only options will be disabled.
        /// </summary>
        [NonSerialized]
        public bool spriteMode;

        [NonSerialized]
        public HighlightProfile previousSettings;

        public void RestorePreviousHighlightEffectSettings() {
            previousSettings.Load(this);
        }

        const float TAU = 0.70711f;

        // Reference materials. These are instanced per object (rms).
        static Material fxMatMask, fxMatSolidColor, fxMatSeeThroughInner, fxMatSeeThroughBorder, fxMatOverlay, fxMatClearStencil;
        static Material fxMatGlowRef, fxMatInnerGlow, fxMatOutlineRef, fxMatTargetRef;
        static Material fxMatComposeGlowRef, fxMatComposeOutlineRef, fxMatBlurGlowRef, fxMatBlurOutlineRef;
        static Material fxMatSeeThroughMask;

        // Per-object materials
        Material _fxMatOutline, _fxMatGlow, _fxMatTarget;
        Material _fxMatComposeGlow, _fxMatComposeOutline, _fxMatBlurGlow, _fxMatBlurOutline;

        Material fxMatOutline {
            get {
                if (_fxMatOutline == null && fxMatOutlineRef != null) {
                    _fxMatOutline = Instantiate(fxMatOutlineRef);
                    if (useGPUInstancing) _fxMatOutline.enableInstancing = true; else _fxMatOutline.enableInstancing = false;
                }
                return _fxMatOutline;
            }
        }

        Material fxMatGlow {
            get {
                if (_fxMatGlow == null && fxMatGlowRef != null) {
                    _fxMatGlow = Instantiate(fxMatGlowRef);
                    if (useGPUInstancing) _fxMatGlow.enableInstancing = true; else _fxMatGlow.enableInstancing = false;
                }
                return _fxMatGlow;
            }
        }

        Material fxMatTarget {
            get {
                if (_fxMatTarget == null && fxMatTargetRef != null) _fxMatTarget = Instantiate(fxMatTargetRef);
                return _fxMatTarget;
            }
        }

        Material fxMatComposeGlow {
            get {
                if (_fxMatComposeGlow == null && fxMatComposeGlowRef != null) _fxMatComposeGlow = Instantiate(fxMatComposeGlowRef);
                return _fxMatComposeGlow;
            }
        }

        Material fxMatComposeOutline {
            get {
                if (_fxMatComposeOutline == null && fxMatComposeOutlineRef != null) _fxMatComposeOutline = Instantiate(fxMatComposeOutlineRef);
                return _fxMatComposeOutline;
            }
        }

        Material fxMatBlurGlow {
            get {
                if (_fxMatBlurGlow == null && fxMatBlurGlowRef != null) _fxMatBlurGlow = Instantiate(fxMatBlurGlowRef);
                return _fxMatBlurGlow;
            }
        }

        Material fxMatBlurOutline {
            get {
                if (_fxMatBlurOutline == null && fxMatBlurOutlineRef != null) _fxMatBlurOutline = Instantiate(fxMatBlurOutlineRef);
                return _fxMatBlurOutline;
            }
        }

        static Vector4[] offsets;

        float fadeStartTime;
        [NonSerialized] public FadingState fading = FadingState.NoFading;
        CommandBuffer cbMask, cbSeeThrough, cbGlow, cbOutline, cbOverlay, cbInnerGlow;
        CommandBuffer cbSmoothBlend;
        int[] mipGlowBuffers, mipOutlineBuffers;
        static Mesh quadMesh, cubeMesh;
        int sourceRT;
        Matrix4x4 quadGlowMatrix, quadOutlineMatrix;
        Vector4[] corners;
        RenderTextureDescriptor sourceDesc;
        Color debugColor, blackColor;
        Visibility lastOutlineVisibility;
        bool requireUpdateMaterial;
        bool useGPUInstancing;
        bool usesReversedZBuffer;
        bool usesSeeThrough;

        class PerCameraOcclusionData {
            public float checkLastTime = -10000;
            public int occlusionRenderFrame;
            public bool lastOcclusionTestResult;
            public readonly List<Renderer> cachedOccluders = new List<Renderer>();
            public Collider cachedOccluderCollider;
        }
        readonly Dictionary<Camera, PerCameraOcclusionData> perCameraOcclusionData = new Dictionary<Camera, PerCameraOcclusionData>();


        MaterialPropertyBlock glowPropertyBlock, outlinePropertyBlock;
        static readonly List<Vector4> matDataDirection = new List<Vector4>();
        static readonly List<Vector4> matDataGlow = new List<Vector4>();
        static readonly List<Vector4> matDataColor = new List<Vector4>();
        static Matrix4x4[] matrices;
        public static readonly List<HighlightEffect> effects = new List<HighlightEffect>();
        public static bool customSorting;
        static int customSortingFrame;
        static Camera customSortingCamera;

        int skipThisFrame = -1;
        int outlineOffsetsMin, outlineOffsetsMax;
        int glowOffsetsMin, glowOffsetsMax;
        static CombineInstance[] combineInstances;
        bool maskRequired;

        Texture2D outlineGradientTex;
        Color[] outlineGradientColors;

        bool shouldBakeSkinnedMesh;

        /// <summary>
        /// Returns a reference to the last highlighted object
        /// </summary>
        public static HighlightEffect lastHighlighted;

        /// <summary>
        /// Returns a reference to the last selected object (when selection is managed by Highlight Manager or Trigger)
        /// </summary>
        public static HighlightEffect lastSelected;

        [NonSerialized]
        public string lastRegExError;

        bool isInitialized;

        [RuntimeInitializeOnLoadMethod]
        static void DomainReloadDisabledSupport() {
            lastHighlighted = lastSelected = null;
            effects.RemoveAll(i => i == null);
        }

        void OnEnable() {
            InitIfNeeded();
        }

        void InitIfNeeded() {
            VRCheck.Init();
            if (rms == null || !isInitialized) {
                Init();
            }
            if (!effects.Contains(this)) {
                effects.Add(this);
            }
        }

        void Init() {
            lastOutlineVisibility = outlineVisibility;
            debugColor = new Color(1f, 0f, 0f, 0.5f);
            blackColor = new Color(0, 0, 0, 0);
            if (offsets == null || offsets.Length != 8) {
                offsets = new Vector4[] {
                    new Vector4(0,1),
                    new Vector4(1,0),
                    new Vector4(0,-1),
                    new Vector4(-1,0),
                    new Vector4 (-TAU, TAU),
                    new Vector4 (TAU, TAU),
                    new Vector4 (TAU, -TAU),
                    new Vector4 (-TAU, -TAU)
                };
            }
            if (corners == null || corners.Length != 8) {
                corners = new Vector4[8];
            }
            if (quadMesh == null) {
                BuildQuad();
            }
            if (cubeMesh == null) {
                BuildCube();
            }
            if (target == null) {
                target = transform;
            }
            if (glowPasses == null || glowPasses.Length == 0) {
                glowPasses = new GlowPassData[4];
                glowPasses[0] = new GlowPassData() { offset = 4, alpha = 0.1f, color = new Color(0.64f, 1f, 0f, 1f) };
                glowPasses[1] = new GlowPassData() { offset = 3, alpha = 0.2f, color = new Color(0.64f, 1f, 0f, 1f) };
                glowPasses[2] = new GlowPassData() { offset = 2, alpha = 0.3f, color = new Color(0.64f, 1f, 0f, 1f) };
                glowPasses[3] = new GlowPassData() { offset = 1, alpha = 0.4f, color = new Color(0.64f, 1f, 0f, 1f) };
            }
            sourceRT = Shader.PropertyToID("_HPSourceRT");
            useGPUInstancing = GPUInstancing && SystemInfo.supportsInstancing;
            usesReversedZBuffer = SystemInfo.usesReversedZBuffer;

            if (useGPUInstancing) {
                if (glowPropertyBlock == null) {
                    glowPropertyBlock = new MaterialPropertyBlock();
                }
                if (outlinePropertyBlock == null) {
                    outlinePropertyBlock = new MaterialPropertyBlock();
                }
            }

            CheckGeometrySupportDependencies();

            if (profileSync && profile != null) {
                profile.Load(this);
            }

            isInitialized = true;
        }

        private void Start() {
            SetupMaterial();
        }

        public void OnDidApplyAnimationProperties() {   // support for animating property based fields
            UpdateMaterialProperties();
        }

        void OnDisable() {
            UpdateMaterialProperties();
            RemoveEffect();
        }

        void Reset() {
            SetupMaterial();
        }

        void DestroyMaterial(Material mat) {
            if (mat != null) DestroyImmediate(mat);
        }

        void DestroyMaterialArray(Material[] mm) {
            if (mm == null) return;
            for (int k = 0; k < mm.Length; k++) {
                DestroyMaterial(mm[k]);
            }
        }

        void RemoveEffect() {
            if (effects != null) {
                int k = effects.IndexOf(this);
                if (k >= 0) {
                    effects.RemoveAt(k);
                }
            }
        }

        void OnDestroy() {
            RemoveEffect();
            if (rms != null) {
                for (int k = 0; k < rms.Length; k++) {
                    DestroyMaterialArray(rms[k].fxMatMask);
                    DestroyMaterialArray(rms[k].fxMatSolidColor);
                    DestroyMaterialArray(rms[k].fxMatSeeThroughInner);
                    DestroyMaterialArray(rms[k].fxMatSeeThroughBorder);
                    DestroyMaterialArray(rms[k].fxMatOverlay);
                    DestroyMaterialArray(rms[k].fxMatInnerGlow);
                }
            }

            DestroyMaterial(fxMatGlow);
            DestroyMaterial(fxMatOutline);
            DestroyMaterial(fxMatTarget);
            DestroyMaterial(fxMatComposeGlow);
            DestroyMaterial(fxMatComposeOutline);
            DestroyMaterial(fxMatBlurGlow);
            DestroyMaterial(fxMatBlurOutline);

            if (effects.Contains(this)) {
                effects.Remove(this);
            }

            if (combinedMeshes.TryGetValue(combinedMeshesHashId, out _)) {
                combinedMeshes.Remove(combinedMeshesHashId);
            }

            foreach (Mesh instancedMesh in instancedMeshes) {
                if (instancedMesh == null) continue;
                int usageCount;
                if (sharedMeshUsage.TryGetValue(instancedMesh, out usageCount)) {
                    if (usageCount <= 1) {
                        sharedMeshUsage.Remove(instancedMesh);
                        DestroyImmediate(instancedMesh);
                    } else {
                        sharedMeshUsage[instancedMesh] = usageCount - 1;
                    }
                }
            }
        }


        public static void DrawEffectsNow(Camera cam = null) {
            if (cam == null) {
                cam = Camera.current;
                if (cam == null) return;
            }
            int effectsCount = effects.Count;
            int thisFrame = Time.frameCount;
            for (int k = 0; k < effectsCount; k++) {
                HighlightEffect effect = effects[k];
                if (effect != null) {
                    effect.DoOnRenderObject(cam);
                    effect.skipThisFrame = thisFrame;
                }
            }
        }

        void OnRenderObject() {
            DoOnRenderObject(Camera.current);
        }

        /// <summary>
        /// Loads a profile into this effect
        /// </summary>
        public void ProfileLoad(HighlightProfile profile) {
            if (profile != null) {
                this.profile = profile;
                profile.Load(this);
            }
        }

        /// <summary>
        /// Reloads currently assigned profile
        /// </summary>
        public void ProfileReload() {
            if (profile != null) {
                profile.Load(this);
            }
        }


        /// <summary>
        /// Save current settings into given profile
        /// </summary>
        public void ProfileSaveChanges(HighlightProfile profile) {
            if (profile != null) {
                profile.Save(this);
            }
        }

        /// <summary>
        /// Save current settings into current profile
        /// </summary>
        public void ProfileSaveChanges() {
            if (profile != null) {
                profile.Save(this);
            }
        }


        public void Refresh(bool discardCachedMeshes = false) {
            if (discardCachedMeshes) {
                RefreshCachedMeshes();
            }
            InitIfNeeded();
            if (enabled) {
                SetupMaterial();
            }
        }


        void DoOnRenderObject(Camera cam) {

            if (customSorting) {
                int frameCount = Time.frameCount;
                if (customSortingFrame != frameCount || customSortingCamera != cam) {
                    customSortingFrame = frameCount;
                    customSortingCamera = cam;
                    int effectsCount = effects.Count;
                    for (int k = 0; k < effectsCount; k++) {
                        HighlightEffect effect = effects[k];
                        effect.skipThisFrame = -1;
                        effect.RenderEffect(cam);
                        effect.skipThisFrame = frameCount;
                    }
                }
            } else {
                RenderEffect(cam);
            }
        }

        void RenderEffect(Camera cam) {

            if (cam == null || ((1 << cam.gameObject.layer) & camerasLayerMask) == 0) return;

            if (!reflectionProbes && cam.cameraType == CameraType.Reflection)
                return;

#if UNITY_EDITOR
            if (!previewInEditor && !Application.isPlaying)
                return;
#endif

            if (requireUpdateMaterial) {
                requireUpdateMaterial = false;
                UpdateMaterialPropertiesNow();
            }

            bool seeThroughReal = usesSeeThrough;
            if (seeThroughReal) {
                seeThroughReal = RenderSeeThroughOccluders(cam);
                if (seeThroughReal && seeThroughOccluderMask != -1) {
                    if (seeThroughOccluderMaskAccurate) {
                        CheckOcclusionAccurate(cam);
                    } else {
                        seeThroughReal = CheckOcclusion(cam);
                    }
                }
            }

            bool showOverlay = hitActive || overlayMode == OverlayMode.Always;
            if (!_highlighted && !seeThroughReal && !showOverlay) {
                return;
            }

            if (skipThisFrame == Time.frameCount) {
                return;
            }

            if (rms == null) {
                SetupMaterial();
                if (rms == null) return;
            }

            // Check camera culling mask
            int cullingMask = cam.cullingMask;

            // Ensure renderers are valid and visible (in case LODgroup has changed active renderer)
            if (!ignoreObjectVisibility) {
                for (int k = 0; k < rmsCount; k++) {
                    if (rms[k].renderer != null && rms[k].renderer.isVisible != rms[k].renderWasVisibleDuringSetup) {
                        SetupMaterial();
                        break;
                    }
                }
            }

            if (fxMatMask == null)
                return;

            // Apply effect
            float glowReal = _highlighted ? this.glow : 0;
            float now = GetTime();

            // Check smooth blend ztesting capability
            bool useSmoothGlow = glow > 0 && glowWidth > 0 && glowQuality == QualityLevel.Highest;
            bool useSmoothOutline = outline > 0 && outlineWidth > 0 && outlineQuality == QualityLevel.Highest;
            bool useSmoothBlend = useSmoothGlow || useSmoothOutline;
            if (useSmoothBlend) {
                if (useSmoothGlow && useSmoothOutline) {
                    outlineVisibility = glowVisibility;
                }
            }
            Visibility smoothGlowVisibility = glowVisibility;
            Visibility smoothOutlineVisibility = outlineVisibility;
#if UNITY_EDITOR
            if (useSmoothBlend && cam.cameraType == CameraType.SceneView) {
                smoothGlowVisibility = smoothOutlineVisibility = Visibility.AlwaysOnTop;
            }
#endif
            if (useSmoothBlend) {
                if (depthClip || outlineEdgeMode == OutlineEdgeMode.Any) {
                    cam.depthTextureMode |= DepthTextureMode.Depth;
                }
                if (Application.isMobilePlatform || (cam.allowMSAA && QualitySettings.antiAliasing > 1)) {
                    smoothGlowVisibility = smoothOutlineVisibility = Visibility.AlwaysOnTop;
                } else if (VRCheck.isVrRunning) {
                    smoothGlowVisibility = smoothOutlineVisibility = Visibility.AlwaysOnTop;
                }
            }

            // First create masks
            float aspect = cam.aspect;
            bool independentFullScreenNotExecuted = true;
            bool somePartVisible = false;

            for (int k = 0; k < rmsCount; k++) {
                rms[k].render = false;

                Transform t = rms[k].transform;
                if (t == null)
                    continue;

                if (rms[k].isSkinnedMesh && shouldBakeSkinnedMesh) {
                    SkinnedMeshRenderer smr = (SkinnedMeshRenderer)rms[k].renderer;
                    if (rms[k].bakedSkinnedMesh == null) {
                        rms[k].bakedSkinnedMesh = new Mesh();
                    }
                    smr.BakeMesh(rms[k].bakedSkinnedMesh, true);
                    rms[k].mesh = rms[k].bakedSkinnedMesh;
                    rms[k].normalsOption = NormalsOption.Smooth;
                }

                Mesh mesh = rms[k].mesh;
                if (mesh == null)
                    continue;

                if (!ignoreObjectVisibility) {
                    int layer = t.gameObject.layer;
                    if (((1 << layer) & cullingMask) == 0)
                        continue;
                    if (!rms[k].renderer.isVisible)
                        continue;
                }

                rms[k].render = true;
                somePartVisible = true;

                if (rms[k].isCombined) {
                    rms[k].renderingMatrix = t.localToWorldMatrix;
                } else if (!rms[k].preserveOriginalMesh) {
                    Vector3 lossyScale = t.lossyScale;
                    Vector3 position = t.position;
                    if (rms[k].bakedTransform) {
                        if (rms[k].currentPosition != t.position || rms[k].currentRotation != t.eulerAngles || rms[k].currentScale != t.lossyScale) {
                            BakeTransform(k, true);
                        }
                        rms[k].renderingMatrix = Matrix4x4.identity;
                    } else {
                        rms[k].renderingMatrix = Matrix4x4.TRS(position, t.rotation, lossyScale);
                    }
                }

                // Outline
                if (outlineIndependent) {
                    if (useSmoothBlend) {
                        if (independentFullScreenNotExecuted) {
                            independentFullScreenNotExecuted = false;
                            fxMatClearStencil.SetPass(0);
                            if (subMeshMask > 0) {
                                for (int l = 0; l < mesh.subMeshCount; l++) {
                                    if (((1 << l) & subMeshMask) != 0) {
                                        Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity, l);
                                    }
                                }
                            } else {
                                Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity);
                            }
                        }
                    } else if (outline > 0 || glow > 0) {
                        float width = outlineWidth;
                        if (glow > 0) {
                            width = Mathf.Max(width, glowWidth);
                        }
                        Material mat = fxMatOutline;
                        bool usesMultipleOffsets = normalsOption != NormalsOption.Planar && outlineQuality.UsesMultipleOffsets();
                        for (int l = 0; l < mesh.subMeshCount; l++) {
                            if (((1 << l) & subMeshMask) == 0) continue;
                            if (usesMultipleOffsets) {
                                for (int o = outlineOffsetsMin; o <= outlineOffsetsMax; o++) {
                                    Vector3 direction = offsets[o] * (width / 100f);
                                    direction.y *= aspect;
                                    mat.SetVector(ShaderParams.OutlineDirection, direction);
                                    if (rms[k].preserveOriginalMesh) {
                                        cbOutline.Clear();
                                        cbOutline.DrawRenderer(rms[k].renderer, mat, l, 1);
                                        Graphics.ExecuteCommandBuffer(cbOutline);
                                    } else {
                                        mat.SetPass(1);
                                        Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                                    }
                                }
                            } else {
                                if (rms[k].preserveOriginalMesh) {
                                    cbOutline.Clear();
                                    cbOutline.DrawRenderer(rms[k].renderer, mat, l, 1);
                                    Graphics.ExecuteCommandBuffer(cbOutline);
                                } else {
                                    mat.SetPass(1);
                                    Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                                }
                            }
                        }
                    }
                }

            }

            if (!somePartVisible) return;

            bool renderMaskOnTop = _highlighted && ((outline > 0 && smoothOutlineVisibility != Visibility.Normal) || (glow > 0 && smoothGlowVisibility != Visibility.Normal) || (innerGlow > 0 && innerGlowVisibility != Visibility.Normal));
            renderMaskOnTop = renderMaskOnTop | (useSmoothBlend && outlineContourStyle == ContourStyle.AroundObjectShape);
            if (maskRequired) {
                for (int k = 0; k < rmsCount; k++) {
                    if (rms[k].render) {
                        RenderMask(k, rms[k].mesh, renderMaskOnTop);
                    }
                }
            }

            // Compute tweening
            float fadeGroup = 1f;
            float fade = 1f;
            if (fading != FadingState.NoFading) {
                if (fading == FadingState.FadingIn) {
                    if (fadeInDuration > 0) {
                        fadeGroup = (now - fadeStartTime) / fadeInDuration;
                        if (fadeGroup > 1f) {
                            fadeGroup = 1f;
                            fading = FadingState.NoFading;
                        }
                    }
                } else if (fadeOutDuration > 0) {
                    fadeGroup = 1f - (now - fadeStartTime) / fadeOutDuration;
                    if (fadeGroup < 0f) {
                        fadeGroup = 0f;
                        fading = FadingState.NoFading;
                        _highlighted = false;
                        requireUpdateMaterial = true;
                        if (OnObjectHighlightEnd != null) {
                            OnObjectHighlightEnd(gameObject);
                        }
                        SendMessage("HighlightEnd", null, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            if (glowQuality == QualityLevel.High) {
                glowReal *= 0.25f;
            } else if (glowQuality == QualityLevel.Medium) {
                glowReal *= 0.5f;
            }

            int smoothRTWidth = 0;
            int smoothRTHeight = 0;
            Bounds smoothBounds = new Bounds();

            if (useSmoothBlend) {
                // Prepare smooth outer glow / outline target
                if (cbSmoothBlend == null) {
                    CheckBlurCommandBuffer();
                }
                cbSmoothBlend.Clear();
                smoothRTWidth = cam.pixelWidth;
                smoothRTHeight = cam.pixelHeight;
                if (smoothRTHeight <= 0) {
                    smoothRTHeight = 1;
                }
                if (VRCheck.isVrRunning) {
                    sourceDesc = VRCheck.vrTextureDescriptor;
                } else {
                    sourceDesc = new RenderTextureDescriptor(smoothRTWidth, smoothRTHeight); // Application.isMobilePlatform ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
                    sourceDesc.volumeDepth = 1;
                }
                sourceDesc.colorFormat = useSmoothOutline && outlineEdgeMode == OutlineEdgeMode.Any ? RenderTextureFormat.ARGB32 : RenderTextureFormat.R8;
                sourceDesc.msaaSamples = 1;
                sourceDesc.useMipMap = false;
                sourceDesc.depthBufferBits = 0;

                cbSmoothBlend.GetTemporaryRT(sourceRT, sourceDesc, FilterMode.Bilinear);
                RenderTargetIdentifier sourceDestination = new RenderTargetIdentifier(sourceRT, 0, CubemapFace.Unknown, -1);
                if ((glow > 0 && smoothGlowVisibility == Visibility.AlwaysOnTop) || (outline > 0 && smoothOutlineVisibility == Visibility.AlwaysOnTop)) {
                    cbSmoothBlend.SetRenderTarget(sourceDestination);
                } else {
                    RenderTargetIdentifier targetDestination = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget, 0, CubemapFace.Unknown, -1);
                    if (Application.isMobilePlatform) {
                        cbSmoothBlend.SetRenderTarget(sourceDestination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare, targetDestination, RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare);
                    } else {
                        cbSmoothBlend.SetRenderTarget(sourceDestination, targetDestination);
                    }
                }
                cbSmoothBlend.ClearRenderTarget(false, true, new Color(0, 0, 0, 0));
            }

            bool targetEffectRendered = false;

            Bounds enclosingBounds = new Bounds();
            if (useSmoothBlend || (targetFX && targetFXUseEnclosingBounds)) {
                for (int k = 0; k < rmsCount; k++) {
                    if (!rms[k].render)
                        continue;
                    if (k == 0) {
                        enclosingBounds = rms[k].renderer.bounds;
                    } else {
                        enclosingBounds.Encapsulate(rms[k].renderer.bounds);
                    }
                }
            }

            cbSeeThrough.Clear();

            // Add effects
            if (seeThroughReal && renderMaskOnTop) {
                for (int k = 0; k < rmsCount; k++) {
                    if (!rms[k].render)
                        continue;
                    Mesh mesh = rms[k].mesh;
                    RenderSeeThroughClearStencil(k, mesh);
                }
                for (int k = 0; k < rmsCount; k++) {
                    if (!rms[k].render)
                        continue;
                    Mesh mesh = rms[k].mesh;
                    RenderSeeThroughMask(k, mesh);
                }
            }

            for (int k = 0; k < rmsCount; k++) {
                if (!rms[k].render)
                    continue;
                Mesh mesh = rms[k].mesh;

                fade = fadeGroup;
                // Distance fade
                if (cameraDistanceFade) {
                    fade *= ComputeCameraDistanceFade(rms[k].transform.position, cam.transform);
                }
                cbSeeThrough.SetGlobalFloat(ShaderParams.FadeFactor, fade);

                // See-Through
                if (seeThroughReal) {
                    if (seeThroughDepthOffset > 0) {
                        cam.depthTextureMode |= DepthTextureMode.Depth;
                    }
                    bool usesSeeThroughBorder = (seeThroughBorder * seeThroughBorderWidth) > 0;
                    for (int l = 0; l < mesh.subMeshCount; l++) {
                        if (((1 << l) & subMeshMask) == 0) continue;
                        if (l < rms[k].fxMatSeeThroughInner.Length && rms[k].fxMatSeeThroughInner[l] != null) {
                            if (rms[k].preserveOriginalMesh) {
                                cbSeeThrough.DrawRenderer(rms[k].renderer, rms[k].fxMatSeeThroughInner[l], l);
                            } else {
                                cbSeeThrough.DrawMesh(mesh, rms[k].renderingMatrix, rms[k].fxMatSeeThroughInner[l], l);
                            }
                            if (usesSeeThroughBorder) {
                                cbSeeThrough.DrawRenderer(rms[k].renderer, rms[k].fxMatSeeThroughBorder[l], l);
                            }
                        }
                    }
                    if (usesSeeThroughBorder) {
                        for (int l = 0; l < mesh.subMeshCount; l++) {
                            if (((1 << l) & subMeshMask) == 0) continue;
                            if (l < rms[k].fxMatSeeThroughBorder.Length && rms[k].fxMatSeeThroughBorder[l] != null) {
                                if (rms[k].preserveOriginalMesh) {
                                    cbSeeThrough.DrawRenderer(rms[k].renderer, rms[k].fxMatSeeThroughBorder[l], l);
                                } else {
                                    cbSeeThrough.DrawMesh(mesh, rms[k].renderingMatrix, rms[k].fxMatSeeThroughBorder[l], l);
                                }
                            }
                        }
                    }
                }

                if (_highlighted || showOverlay) {
                    // Hit FX
                    Color overlayColor = this.overlayColor;
                    float overlayMinIntensity = this.overlayMinIntensity;
                    float overlayBlending = this.overlayBlending;

                    Color innerGlowColorA = this.innerGlowColor;
                    float innerGlow = this.innerGlow;

                    if (hitActive) {
                        overlayColor.a = _highlighted ? overlay : 0;
                        innerGlowColorA.a = _highlighted ? innerGlow : 0;
                        float t = hitFadeOutDuration > 0 ? (now - hitStartTime) / hitFadeOutDuration : 1f;
                        if (t >= 1f) {
                            hitActive = false;
                        } else {
                            if (hitFxMode == HitFxMode.InnerGlow) {
                                bool lerpToCurrentInnerGlow = _highlighted && innerGlow > 0;
                                innerGlowColorA = lerpToCurrentInnerGlow ? Color.Lerp(hitColor, innerGlowColor, t) : hitColor;
                                innerGlowColorA.a = lerpToCurrentInnerGlow ? Mathf.Lerp(1f - t, innerGlow, t) : 1f - t;
                                innerGlowColorA.a *= hitInitialIntensity;
                            } else {
                                bool lerpToCurrentOverlay = _highlighted && overlay > 0;
                                overlayColor = lerpToCurrentOverlay ? Color.Lerp(hitColor, overlayColor, t) : hitColor;
                                overlayColor.a = lerpToCurrentOverlay ? Mathf.Lerp(1f - t, overlay, t) : 1f - t;
                                overlayColor.a *= hitInitialIntensity;
                                overlayMinIntensity = 1f;
                                overlayBlending = 0;
                            }
                        }
                    } else {
                        overlayColor.a = overlay * fade;
                        innerGlowColorA.a = innerGlow * fade;
                    }

                    for (int l = 0; l < mesh.subMeshCount; l++) {
                        if (((1 << l) & subMeshMask) == 0) continue;

                        // Overlay
                        if (overlayColor.a > 0) {
                            Material fxMat = rms[k].fxMatOverlay[l];
                            fxMat.SetColor(ShaderParams.OverlayColor, overlayColor);
                            fxMat.SetVector(ShaderParams.OverlayData, new Vector4(overlayAnimationSpeed, overlayMinIntensity, overlayBlending, overlayTextureScale));
                            if (hitActive && hitFxMode == HitFxMode.LocalHit) {
                                fxMat.SetVector(ShaderParams.OverlayHitPosData, new Vector4(hitPosition.x, hitPosition.y, hitPosition.z, hitRadius));
                                fxMat.SetFloat(ShaderParams.OverlayHitStartTime, hitStartTime);
                            } else {
                                fxMat.SetVector(ShaderParams.OverlayHitPosData, Vector4.zero);
                            }
                            if (rms[k].preserveOriginalMesh) {
                                cbOverlay.Clear();
                                cbOverlay.DrawRenderer(rms[k].renderer, fxMat, l);
                                Graphics.ExecuteCommandBuffer(cbOverlay);
                            } else {
                                fxMat.SetPass(0);
                                Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                            }
                        }

                        // Inner Glow
                        if (innerGlowColorA.a > 0) {
                            rms[k].fxMatInnerGlow[l].SetColor(ShaderParams.InnerGlowColor, innerGlowColorA);

                            if (rms[k].preserveOriginalMesh) {
                                cbInnerGlow.Clear();
                                cbInnerGlow.DrawRenderer(rms[k].renderer, rms[k].fxMatInnerGlow[l], l);
                                Graphics.ExecuteCommandBuffer(cbInnerGlow);
                            } else {
                                rms[k].fxMatInnerGlow[l].SetPass(0);
                                Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                            }
                        }
                    }
                }

                if (!_highlighted)
                    continue;

                if (useSmoothBlend) {
                    if (k == 0) {
                        smoothBounds = rms[k].renderer.bounds;
                    } else {
                        smoothBounds.Encapsulate(rms[k].renderer.bounds);
                    }
                }

                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;

                    // Render object body for glow/outline highest quality
                    if (useSmoothBlend) {
                        if (l < rms[k].fxMatSolidColor.Length) {
                            if (rms[k].preserveOriginalMesh) {
                                cbSmoothBlend.DrawRenderer(rms[k].renderer, rms[k].fxMatSolidColor[l], l);
                            } else {
                                cbSmoothBlend.DrawMesh(mesh, rms[k].renderingMatrix, rms[k].fxMatSolidColor[l], l);
                            }
                        }
                    }

                    // Glow
                    if (glow > 0 && glowQuality != QualityLevel.Highest) {
                        matDataGlow.Clear();
                        matDataColor.Clear();
                        matDataDirection.Clear();
                        Material mat = fxMatGlow;
                        Vector4 directionZero = normalsOption == NormalsOption.Planar ? new Vector4(0, 0, glowWidth / 100f, 0) : Vector4.zero;
                        mat.SetVector(ShaderParams.GlowDirection, directionZero);
                        for (int j = 0; j < glowPasses.Length; j++) {
                            Color dataColor = glowPasses[j].color;
                            mat.SetColor(ShaderParams.GlowColor, dataColor);
                            Vector4 dataGlow = new Vector4(fade * glowReal * glowPasses[j].alpha, normalsOption == NormalsOption.Planar ? 0 : glowPasses[j].offset * glowWidth / 100f, glowMagicNumber1, glowMagicNumber2);
                            mat.SetVector(ShaderParams.Glow, dataGlow);
                            if (normalsOption != NormalsOption.Planar && glowQuality.UsesMultipleOffsets()) {
                                for (int o = glowOffsetsMin; o <= glowOffsetsMax; o++) {
                                    Vector4 direction = offsets[o];
                                    direction.y *= aspect;
                                    mat.SetVector(ShaderParams.GlowDirection, direction);
                                    if (rms[k].preserveOriginalMesh) {
                                        cbGlow.Clear();
                                        cbGlow.DrawRenderer(rms[k].renderer, mat, l);
                                        Graphics.ExecuteCommandBuffer(cbGlow);
                                    } else {
                                        if (useGPUInstancing) {
                                            matDataDirection.Add(direction);
                                            matDataGlow.Add(dataGlow);
                                            matDataColor.Add(new Vector4(dataColor.r, dataColor.g, dataColor.b, dataColor.a));
                                        } else {
                                            mat.SetPass(0);
                                            Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                                        }
                                    }
                                }
                            } else {
                                if (rms[k].preserveOriginalMesh) {
                                    cbGlow.Clear();
                                    cbGlow.DrawRenderer(rms[k].renderer, mat, l);
                                    Graphics.ExecuteCommandBuffer(cbGlow);
                                } else {
                                    if (useGPUInstancing) {
                                        matDataDirection.Add(directionZero);
                                        matDataGlow.Add(dataGlow);
                                        matDataColor.Add(new Vector4(dataColor.r, dataColor.g, dataColor.b, dataColor.a));
                                    } else {
                                        mat.SetPass(0);
                                        Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                                    }
                                }
                            }
                        }
                        if (useGPUInstancing) {
                            int instanceCount = matDataDirection.Count;
                            if (instanceCount > 0) {
                                glowPropertyBlock.Clear();
                                glowPropertyBlock.SetVectorArray(ShaderParams.GlowDirection, matDataDirection);
                                glowPropertyBlock.SetVectorArray(ShaderParams.GlowColor, matDataColor);
                                glowPropertyBlock.SetVectorArray(ShaderParams.Glow, matDataGlow);
                                if (matrices == null || matrices.Length < instanceCount) {
                                    matrices = new Matrix4x4[instanceCount];
                                }
                                for (int m = 0; m < instanceCount; m++) {
                                    matrices[m] = rms[k].renderingMatrix;
                                }
                                cbGlow.Clear();
                                cbGlow.DrawMeshInstanced(mesh, l, mat, 0, matrices, instanceCount, glowPropertyBlock);
                                Graphics.ExecuteCommandBuffer(cbGlow);
                            }
                        }
                    }

                    // Outline
                    if (outline > 0 && outlineQuality != QualityLevel.Highest) {
                        Material mat = fxMatOutline;
                        Color outlineColor = this.outlineColor;
                        if (outlineColorStyle == ColorStyle.Gradient) {
                            outlineColor.a *= outline * fade;
                            Bounds bounds = outlineGradientInLocalSpace ? mesh.bounds : rms[k].renderer.bounds;
                            mat.SetVector(ShaderParams.OutlineVertexData, new Vector4(bounds.min.y, bounds.size.y + 0.0001f, 0));
                        } else {
                            outlineColor.a = outline * fade;
                            mat.SetVector(ShaderParams.OutlineVertexData, new Vector4(-1e6f, 1f, 1f, 0));
                        }
                        mat.SetColor(ShaderParams.OutlineColor, outlineColor);
                        if (normalsOption != NormalsOption.Planar && outlineQuality.UsesMultipleOffsets()) {
                            matDataDirection.Clear();
                            for (int o = outlineOffsetsMin; o <= outlineOffsetsMax; o++) {
                                Vector4 direction = offsets[o] * (outlineWidth / 100f);
                                direction.y *= aspect;
                                mat.SetVector(ShaderParams.OutlineDirection, direction);
                                if (rms[k].preserveOriginalMesh) {
                                    cbOutline.Clear();
                                    cbOutline.DrawRenderer(rms[k].renderer, mat, l, 0);
                                    Graphics.ExecuteCommandBuffer(cbOutline);
                                } else {
                                    if (useGPUInstancing) {
                                        matDataDirection.Add(direction);
                                    } else {
                                        mat.SetPass(0);
                                        Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                                    }
                                }
                            }
                            if (useGPUInstancing) {
                                int instanceCount = matDataDirection.Count;
                                if (instanceCount > 0) {
                                    outlinePropertyBlock.Clear();
                                    outlinePropertyBlock.SetVectorArray(ShaderParams.OutlineDirection, matDataDirection);
                                    if (matrices == null || matrices.Length < instanceCount) {
                                        matrices = new Matrix4x4[instanceCount];
                                    }
                                    for (int m = 0; m < instanceCount; m++) {
                                        matrices[m] = rms[k].renderingMatrix;
                                    }
                                    cbGlow.Clear(); // we reuse the same commandbuffer for glow
                                    cbGlow.DrawMeshInstanced(mesh, l, mat, 0, matrices, instanceCount, outlinePropertyBlock);
                                    Graphics.ExecuteCommandBuffer(cbGlow);
                                }
                            }
                        } else {
                            if (rms[k].preserveOriginalMesh) {
                                cbOutline.Clear();
                                cbOutline.DrawRenderer(rms[k].renderer, mat, l, 0);
                                Graphics.ExecuteCommandBuffer(cbOutline);
                            } else {
                                mat.SetPass(0);
                                Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                            }
                        }
                    }
                }

                // Target
                if (targetFX) {
                    float fadeOut = 1f;
                    if (targetFXStayDuration > 0 && Application.isPlaying) {
                        fadeOut = (now - targetFxStartTime);
                        if (fadeOut >= targetFXStayDuration) {
                            fadeOut -= targetFXStayDuration;
                            fadeOut = 1f - fadeOut;
                        }
                        if (fadeOut > 1f) {
                            fadeOut = 1f;
                        }
                    }
                    bool usesTarget = targetFXCenter != null;
                    if (fadeOut > 0 && !(targetEffectRendered && usesTarget)) {
                        targetEffectRendered = true;
                        float scaleT = 1f;
                        float time;
                        float normalizedTime = 0;
                        if (Application.isPlaying) {
                            time = now;
                            normalizedTime = (time - targetFxStartTime) / targetFXTransitionDuration;
                            if (normalizedTime > 1f) {
                                normalizedTime = 1f;
                            }
                            scaleT = Mathf.Sin(normalizedTime * Mathf.PI * 0.5f);
                        } else {
                            time = (float)DateTime.Now.Subtract(DateTime.Today).TotalSeconds;
                        }
                        Bounds bounds = targetFXUseEnclosingBounds ? enclosingBounds : rms[k].renderer.bounds;
                        if (!targetFXScaleToRenderBounds) {
                            bounds.size = Vector3.one;
                        }
                        Vector3 scale = bounds.size;
                        float minSize = scale.x;
                        if (scale.y < minSize) {
                            minSize = scale.y;
                        }
                        if (scale.z < minSize) {
                            minSize = scale.z;
                        }
                        scale.x = scale.y = scale.z = minSize;
                        scale = Vector3.Lerp(scale * targetFXInitialScale, scale * targetFXEndScale, scaleT);
                        Vector3 center = usesTarget ? targetFXCenter.position : bounds.center;
                        center += targetFXOffset;
                        Quaternion rotation;
                        if (targetFXAlignToGround) {
                            rotation = Quaternion.Euler(90, 0, 0);
                            center.y += 0.5f; // a bit of offset in case it's in contact with ground
                            if (Physics.Raycast(center, Vector3.down, out RaycastHit groundHitInfo, targetFXGroundMaxDistance, targetFXGroundLayerMask)) {
                                cam.depthTextureMode |= DepthTextureMode.Depth;
                                center = groundHitInfo.point;
                                center.y += 0.01f;
                                Vector4 renderData = groundHitInfo.normal;
                                renderData.w = targetFXFadePower;
                                fxMatTarget.SetVector(ShaderParams.TargetFXRenderData, renderData);
                                rotation = Quaternion.Euler(0, time * targetFXRotationSpeed, 0);
                                if (OnTargetAnimates != null) {
                                    OnTargetAnimates(ref center, ref rotation, ref scale, normalizedTime);
                                }
                                Matrix4x4 m = Matrix4x4.TRS(center, rotation, scale);
                                Color color = targetFXColor;
                                color.a *= fade * fadeOut;
                                Material mat = fxMatTarget;
                                mat.color = color;
                                mat.SetPass(0);
                                Graphics.DrawMeshNow(cubeMesh, m);
                            }
                        } else {
                            rotation = Quaternion.LookRotation(cam.transform.position - rms[k].transform.position);
                            Quaternion animationRot = Quaternion.Euler(0, 0, time * targetFXRotationSpeed);
                            rotation *= animationRot;
                            if (OnTargetAnimates != null) {
                                OnTargetAnimates(ref center, ref rotation, ref scale, normalizedTime);
                            }
                            Matrix4x4 m = Matrix4x4.TRS(center, rotation, scale);
                            Color color = targetFXColor;
                            color.a *= fade * fadeOut;
                            Material mat = fxMatTarget;
                            mat.color = color;
                            mat.SetPass(1);
                            Graphics.DrawMeshNow(quadMesh, m);
                        }
                    }
                }
            }

            if (useSmoothBlend) {
                if (ComputeSmoothQuadMatrix(cam, smoothBounds)) {
                    // Smooth Glow
                    if (useSmoothGlow) {
                        float intensity = glow * fade;
                        fxMatComposeGlow.color = new Color(glowHQColor.r * intensity, glowHQColor.g * intensity, glowHQColor.b * intensity, glowHQColor.a * intensity);
                        SmoothGlow(smoothRTWidth / glowDownsampling, smoothRTHeight / glowDownsampling);
                    }

                    // Smooth Outline
                    if (useSmoothOutline) {
                        fxMatComposeOutline.color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, 5f * outlineColor.a * outline * fade);
                        SmoothOutline(smoothRTWidth / outlineDownsampling, smoothRTHeight / outlineDownsampling);
                    }

                    // Bit result
                    ComposeSmoothBlend(smoothGlowVisibility, smoothOutlineVisibility);
                }
            }

            if (seeThroughReal && seeThroughOrdered) { // Ordered for see-through
                for (int k = 0; k < rmsCount; k++) {
                    if (!rms[k].render)
                        continue;
                    Mesh mesh = rms[k].mesh;
                    for (int l = 0; l < mesh.subMeshCount; l++) {
                        if (((1 << l) & subMeshMask) == 0) continue;
                        if (rms[k].isCombined) {
                            cbSeeThrough.DrawMesh(mesh, rms[k].renderingMatrix, fxMatClearStencil, l, 1);
                        } else {
                            cbSeeThrough.DrawRenderer(rms[k].renderer, fxMatClearStencil, l, 1);
                        }
                    }
                }
            }

            Graphics.ExecuteCommandBuffer(cbSeeThrough);
        }


        void RenderMask(int k, Mesh mesh, bool alwaysOnTop) {
            if (rms[k].preserveOriginalMesh) {
                cbMask.Clear();
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    if (alwaysOnTop) {
                        rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, (int)CompareFunction.Always);
                    } else {
                        rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, (int)CompareFunction.LessEqual);
                    }
                    cbMask.DrawRenderer(rms[k].renderer, rms[k].fxMatMask[l], l, 0);
                }
                Graphics.ExecuteCommandBuffer(cbMask);
            } else {
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    if (alwaysOnTop) {
                        rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, (int)CompareFunction.Always);
                    } else {
                        rms[k].fxMatMask[l].SetInt(ShaderParams.ZTest, (int)CompareFunction.LessEqual);
                    }
                    rms[k].fxMatMask[l].SetPass(0);
                    Graphics.DrawMeshNow(mesh, rms[k].renderingMatrix, l);
                }
            }


        }

        void RenderSeeThroughClearStencil(int k, Mesh mesh) {
            if (rms[k].preserveOriginalMesh) {
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    cbSeeThrough.DrawRenderer(rms[k].renderer, fxMatClearStencil, l, 1);
                }
            } else {
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    cbSeeThrough.DrawMesh(mesh, rms[k].renderingMatrix, fxMatClearStencil, l, 1);
                }
            }
        }

        void RenderSeeThroughMask(int k, Mesh mesh) {
            if (rms[k].preserveOriginalMesh) {
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    cbSeeThrough.DrawRenderer(rms[k].renderer, rms[k].fxMatMask[l], l, 1);
                }
            } else {
                for (int l = 0; l < mesh.subMeshCount; l++) {
                    if (((1 << l) & subMeshMask) == 0) continue;
                    cbSeeThrough.DrawMesh(mesh, rms[k].renderingMatrix, rms[k].fxMatMask[l], l, 1);
                }
            }
        }


        void WorldToViewportPoint(ref Matrix4x4 m, ref Vector4 p, bool perspectiveProjection, float zBufferParamsZ, float zBufferParamsW) {
            p = m * p;
            p.x = (p.x / p.w + 1f) * 0.5f;
            p.y = (p.y / p.w + 1f) * 0.5f;

            if (perspectiveProjection) {
                p.z /= p.w;
                p.z = 1.0f / (zBufferParamsZ * p.z + zBufferParamsW);
            } else {
                if (usesReversedZBuffer) {
                    p.z = 1f - p.z;
                }
                p.z = (zBufferParamsW - zBufferParamsZ) * p.z + zBufferParamsZ;
            }
        }


        bool ComputeSmoothQuadMatrix(Camera cam, Bounds bounds) {
            // Compute bounds in screen space and enlarge for glow space
            bool res;
            if (VRCheck.isVrRunning) {
                Vector3 shift = Vector3.zero;
                res = ComputeSmoothQuadMatrixOriginShifted(cam, ref bounds, ref shift);
            } else {
                Vector3 shift = cam.transform.position;
                cam.transform.position = Vector3.zero;
                cam.ResetWorldToCameraMatrix();
                bounds.center -= shift;
                res = ComputeSmoothQuadMatrixOriginShifted(cam, ref bounds, ref shift);
                cam.transform.position = shift;
            }
            return res;
        }

        bool ComputeSmoothQuadMatrixOriginShifted(Camera cam, ref Bounds bounds, ref Vector3 shift) {
            // Compute bounds in screen space and enlarge for glow space

            Matrix4x4 mat = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false) * cam.worldToCameraMatrix;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            corners[0] = new Vector4(min.x, min.y, min.z, 1f);
            corners[1] = new Vector4(min.x, min.y, max.z, 1f);
            corners[2] = new Vector4(max.x, min.y, min.z, 1f);
            corners[3] = new Vector4(max.x, min.y, max.z, 1f);
            corners[4] = new Vector4(min.x, max.y, min.z, 1f);
            corners[5] = new Vector4(min.x, max.y, max.z, 1f);
            corners[6] = new Vector4(max.x, max.y, min.z, 1f);
            corners[7] = new Vector4(max.x, max.y, max.z, 1f);
            Vector3 scrMin = new Vector3(float.MaxValue, float.MaxValue, 0);
            Vector3 scrMax = new Vector3(float.MinValue, float.MinValue, 0);
            float distanceMin = float.MaxValue;
            float distanceMax = float.MinValue;
            float nearClipPlane = cam.nearClipPlane;
            float farClipPlane = cam.farClipPlane;

            float x, y, z, w;
            bool isPerspectiveCamera = !cam.orthographic;
            if (isPerspectiveCamera) {
                if (usesReversedZBuffer) {
                    x = -1f + farClipPlane / nearClipPlane;
                    y = 1f;
                    z = x / farClipPlane;
                    w = 1f / farClipPlane;
                } else {
                    x = 1f - farClipPlane / nearClipPlane;
                    y = farClipPlane / nearClipPlane;
                    z = x / farClipPlane;
                    w = y / farClipPlane;
                }
            } else {
                z = nearClipPlane;
                w = farClipPlane;
            }

            for (int k = 0; k < 8; k++) {
                WorldToViewportPoint(ref mat, ref corners[k], isPerspectiveCamera, z, w);
                if (corners[k].x < scrMin.x) {
                    scrMin.x = corners[k].x;
                }
                if (corners[k].y < scrMin.y) {
                    scrMin.y = corners[k].y;
                }
                if (corners[k].x > scrMax.x) {
                    scrMax.x = corners[k].x;
                }
                if (corners[k].y > scrMax.y) {
                    scrMax.y = corners[k].y;
                }
                if (corners[k].z < distanceMin) {
                    distanceMin = corners[k].z;
                    if (distanceMin < nearClipPlane) {
                        distanceMin = distanceMax = 0.01f + nearClipPlane;
                        scrMin.x = scrMin.y = 0;
                        scrMax.x = 1f;
                        scrMax.y = 1f;
                        break;
                    }
                }
                if (corners[k].z > distanceMax) {
                    distanceMax = corners[k].z;
                }
            }
            if (scrMax.y == scrMin.y)
                return false;

            int pixelWidth = cam.pixelWidth;
            int pixelHeight = cam.pixelHeight;
            scrMin.x *= pixelWidth;
            scrMax.x *= pixelWidth;
            scrMin.y *= pixelHeight;
            scrMax.y *= pixelHeight;
            scrMin.x += cam.pixelRect.xMin;
            scrMax.x += cam.pixelRect.xMin;
            scrMin.y += cam.pixelRect.yMin;
            scrMax.y += cam.pixelRect.yMin;

            if (spriteMode) {
                scrMin.z = scrMax.z = (distanceMin + distanceMax) * 0.5f + nearClipPlane;
            } else {
                scrMin.z = scrMax.z = VRCheck.isVrRunning ? distanceMin : 0.05f + nearClipPlane; // small shift to avoid origin far issues
            }

            if (outline > 0) {
                BuildMatrix(cam, scrMin, scrMax, (int)(10 + 20 * outlineWidth + 5 * outlineDownsampling), ref quadOutlineMatrix, ref shift);
            }
            if (glow > 0) {
                BuildMatrix(cam, scrMin, scrMax, (int)(20 + 30 * glowWidth + 10 * glowDownsampling), ref quadGlowMatrix, ref shift);
            }
            return true;
        }

        void BuildMatrix(Camera cam, Vector3 scrMin, Vector3 scrMax, int border, ref Matrix4x4 quadMatrix, ref Vector3 shift) {

            // Insert padding to make room for effects
            scrMin.x -= border;
            scrMin.y -= border;
            scrMax.x += border;
            scrMax.y += border;

            // Back to world space
            Vector3 third = new Vector3(scrMax.x, scrMin.y, scrMin.z);
            scrMin = cam.ScreenToWorldPoint(scrMin);
            scrMax = cam.ScreenToWorldPoint(scrMax);
            third = cam.ScreenToWorldPoint(third);

            float width = Vector3.Distance(scrMin, third);
            float height = Vector3.Distance(scrMax, third);

            quadMatrix = Matrix4x4.TRS((scrMin + scrMax) * 0.5f + shift, cam.transform.rotation, new Vector3(width, height, 1f));
        }

        void SmoothGlow(int rtWidth, int rtHeight) {

            Material matBlur = fxMatBlurGlow;
            RenderTextureDescriptor glowDesc = sourceDesc;
            glowDesc.depthBufferBits = 0;

            if (glowBlurMethod == BlurMethod.Gaussian) {

                const int blurPasses = 4;

                int bufferCount = blurPasses * 2;
                if (mipGlowBuffers == null || mipGlowBuffers.Length != bufferCount) {
                    mipGlowBuffers = new int[bufferCount];
                    for (int k = 0; k < bufferCount; k++) {
                        mipGlowBuffers[k] = Shader.PropertyToID("_HPSmoothGlowTemp" + k);
                    }
                    mipGlowBuffers[bufferCount - 2] = ShaderParams.GlowRT;
                }

                for (int k = 0; k < bufferCount; k++) {
                    float reduction = k / 2 + 2;
                    int reducedWidth = (int)(rtWidth / reduction);
                    int reducedHeight = (int)(rtHeight / reduction);
                    if (reducedWidth <= 0) {
                        reducedWidth = 1;
                    }
                    if (reducedHeight <= 0) {
                        reducedHeight = 1;
                    }
                    glowDesc.width = reducedWidth;
                    glowDesc.height = reducedHeight;
                    cbSmoothBlend.GetTemporaryRT(mipGlowBuffers[k], glowDesc, FilterMode.Bilinear);
                }

                for (int k = 0; k < bufferCount - 1; k += 2) {
                    if (k == 0) {
                        cbSmoothBlend.Blit(sourceRT, mipGlowBuffers[k + 1], matBlur, 0);
                    } else {
                        cbSmoothBlend.Blit(mipGlowBuffers[k], mipGlowBuffers[k + 1], matBlur, 0);
                    }
                    cbSmoothBlend.Blit(mipGlowBuffers[k + 1], mipGlowBuffers[k], matBlur, 1);

                    if (k < bufferCount - 2) {
                        cbSmoothBlend.Blit(mipGlowBuffers[k], mipGlowBuffers[k + 2], matBlur, 2);
                    }
                }
            } else {
                const int blurPasses = 5;

                int bufferCount = blurPasses;
                if (mipGlowBuffers == null || mipGlowBuffers.Length != bufferCount) {
                    mipGlowBuffers = new int[bufferCount];
                    for (int k = 0; k < bufferCount; k++) {
                        mipGlowBuffers[k] = Shader.PropertyToID("_HPSmoothGlowTemp" + k);
                    }
                    mipGlowBuffers[bufferCount - 2] = ShaderParams.GlowRT;
                }

                for (int k = 0; k < bufferCount; k++) {
                    float reduction = k + 2;
                    int reducedWidth = (int)(rtWidth / reduction);
                    int reducedHeight = (int)(rtHeight / reduction);
                    if (reducedWidth <= 0) {
                        reducedWidth = 1;
                    }
                    if (reducedHeight <= 0) {
                        reducedHeight = 1;
                    }
                    glowDesc.width = reducedWidth;
                    glowDesc.height = reducedHeight;
                    cbSmoothBlend.GetTemporaryRT(mipGlowBuffers[k], glowDesc, FilterMode.Bilinear);
                }
                cbSmoothBlend.Blit(sourceRT, mipGlowBuffers[0], matBlur, 3);
                for (int k = 0; k < bufferCount - 2; k++) {
                    cbSmoothBlend.SetGlobalFloat(ShaderParams.ResampleScale, k + 0.5f);
                    cbSmoothBlend.Blit(mipGlowBuffers[k], mipGlowBuffers[k + 1], matBlur, 3);
                }
            }
        }

        void SmoothOutline(int rtWidth, int rtHeight) {

            // Blur buffers
            int bufferCount = outlineBlurPasses * 2;
            if (mipOutlineBuffers == null || mipOutlineBuffers.Length != bufferCount) {
                mipOutlineBuffers = new int[bufferCount];
                for (int k = 0; k < bufferCount; k++) {
                    mipOutlineBuffers[k] = Shader.PropertyToID("_HPSmoothOutlineTemp" + k);
                }
                mipOutlineBuffers[bufferCount - 2] = ShaderParams.OutlineRT;
            }
            RenderTextureDescriptor outlineDesc = sourceDesc;
            outlineDesc.depthBufferBits = 0;

            for (int k = 0; k < bufferCount; k++) {
                float reduction = k / 2 + 2;
                int reducedWidth = (int)(rtWidth / reduction);
                int reducedHeight = (int)(rtHeight / reduction);
                if (reducedWidth <= 0) {
                    reducedWidth = 1;
                }
                if (reducedHeight <= 0) {
                    reducedHeight = 1;
                }
                outlineDesc.width = reducedWidth;
                outlineDesc.height = reducedHeight;
                cbSmoothBlend.GetTemporaryRT(mipOutlineBuffers[k], outlineDesc, FilterMode.Bilinear);
            }

            Material matBlur = fxMatBlurOutline;
            for (int k = 0; k < bufferCount - 1; k += 2) {
                if (k == 0) {
                    cbSmoothBlend.Blit(sourceRT, mipOutlineBuffers[k + 1], matBlur, 3);
                } else {
                    cbSmoothBlend.Blit(mipOutlineBuffers[k], mipOutlineBuffers[k + 1], matBlur, 0);
                }
                cbSmoothBlend.Blit(mipOutlineBuffers[k + 1], mipOutlineBuffers[k], matBlur, 1);

                if (k < bufferCount - 2) {
                    cbSmoothBlend.Blit(mipOutlineBuffers[k], mipOutlineBuffers[k + 2], matBlur, 2);
                }
            }
        }

        void ComposeSmoothBlend(Visibility smoothGlowVisibility, Visibility smoothOutlineVisibility) {
            bool renderSmoothGlow = glow > 0 && glowWidth > 0 && glowQuality == QualityLevel.Highest;
            RenderTargetIdentifier cameraTargetDestination = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget, 0, CubemapFace.Unknown, -1);
            if (renderSmoothGlow) {
                Material matComposeGlow = fxMatComposeGlow;
                matComposeGlow.SetVector(ShaderParams.Flip, (VRCheck.isVrRunning && flipY) ? new Vector3(1, -1, 0) : new Vector3(0, 1, 0));
                if (glowOptimalBlit) {
                    if (Application.isMobilePlatform && smoothGlowVisibility != Visibility.AlwaysOnTop) {
                        cbSmoothBlend.SetRenderTarget(cameraTargetDestination, RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare);
                    } else {
                        cbSmoothBlend.SetRenderTarget(cameraTargetDestination);
                    }
                    matComposeGlow.SetInt(ShaderParams.ZTest, GetZTestValue(smoothGlowVisibility));
                    matComposeGlow.SetColor(ShaderParams.Debug, glowBlitDebug ? debugColor : blackColor);
                    cbSmoothBlend.DrawMesh(quadMesh, quadGlowMatrix, matComposeGlow, 0, 0);
                } else {
                    cbSmoothBlend.Blit(ShaderParams.GlowRT, cameraTargetDestination, matComposeGlow, 1);
                }
            }
            bool renderSmoothOutline = outline > 0 && outlineWidth > 0 && outlineQuality == QualityLevel.Highest;
            if (renderSmoothOutline) {
                Material matComposeOutline = fxMatComposeOutline;
                matComposeOutline.SetVector(ShaderParams.Flip, (VRCheck.isVrRunning && flipY) ? new Vector3(1, -1, 0) : new Vector3(0, 1, 0));
                if (outlineOptimalBlit) {
                    if (Application.isMobilePlatform && smoothOutlineVisibility != Visibility.AlwaysOnTop) {
                        cbSmoothBlend.SetRenderTarget(cameraTargetDestination, RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare);
                    } else {
                        cbSmoothBlend.SetRenderTarget(cameraTargetDestination);
                    }
                    matComposeOutline.SetInt(ShaderParams.ZTest, GetZTestValue(smoothOutlineVisibility));
                    cbSmoothBlend.DrawMesh(quadMesh, quadOutlineMatrix, matComposeOutline, 0, 0);
                } else {
                    cbSmoothBlend.Blit(ShaderParams.OutlineRT, cameraTargetDestination, matComposeOutline, 1);
                }
            }
            // Release render textures
            if (renderSmoothGlow) {
                for (int k = 0; k < mipGlowBuffers.Length; k++) {
                    cbSmoothBlend.ReleaseTemporaryRT(mipGlowBuffers[k]);
                }
            }
            if (renderSmoothOutline) {
                for (int k = 0; k < mipOutlineBuffers.Length; k++) {
                    cbSmoothBlend.ReleaseTemporaryRT(mipOutlineBuffers[k]);
                }
            }

            cbSmoothBlend.ReleaseTemporaryRT(sourceRT);

            Graphics.ExecuteCommandBuffer(cbSmoothBlend);
        }

        void InitMaterial(ref Material material, string shaderName) {
            if (material == null) {
                Shader shaderFX = Shader.Find(shaderName);
                if (shaderFX == null) {
                    Debug.LogError("Shader " + shaderName + " not found.");
                    return;
                }
                material = new Material(shaderFX);
            }
        }

        /// <summary>
        /// Sets target for highlight effects
        /// </summary>
        public void SetTarget(Transform transform) {
            if (transform == target || transform == null)
                return;

            if (_highlighted) {
                ImmediateFadeOut();
            }

            target = transform;
            SetupMaterial();
        }


        /// <summary>
        /// Sets target for highlight effects and also specify a list of renderers to be included as well
        /// </summary>
        public void SetTargets(Transform transform, Renderer[] renderers) {
            if (transform == null)
                return;

            if (_highlighted) {
                ImmediateFadeOut();
            }

            effectGroup = TargetOptions.Scripting;
            target = transform;
            SetupMaterial(renderers);
        }


        /// <summary>
        /// Start or finish highlight on the object
        /// </summary>
        public void SetHighlighted(bool state) {

            if (state != _highlighted && OnObjectHighlightStateChange != null) {
                if (!OnObjectHighlightStateChange(gameObject, state)) return;
            }

            if (state) lastHighlighted = this;

            if (!Application.isPlaying) {
                _highlighted = state;
                return;
            }

            float now = GetTime();

            if (fading == FadingState.NoFading) {
                fadeStartTime = now;
            }

            if (state && !ignore) {
                if (_highlighted && fading == FadingState.NoFading) {
                    return;
                }
                if (OnObjectHighlightStart != null) {
                    if (!OnObjectHighlightStart(gameObject)) {
                        return;
                    }
                }
                SendMessage("HighlightStart", null, SendMessageOptions.DontRequireReceiver);
                highlightStartTime = targetFxStartTime = now;
                if (fadeInDuration > 0) {
                    if (fading == FadingState.FadingOut) {
                        float remaining = fadeOutDuration - (now - fadeStartTime);
                        fadeStartTime = now - remaining;
                        fadeStartTime = Mathf.Min(fadeStartTime, now);
                    }
                    fading = FadingState.FadingIn;
                } else {
                    fading = FadingState.NoFading;
                }
                _highlighted = true;
                requireUpdateMaterial = true;
            } else if (_highlighted) {
                if (fadeOutDuration > 0) {
                    if (fading == FadingState.FadingIn) {
                        float elapsed = now - fadeStartTime;
                        fadeStartTime = now + elapsed - fadeInDuration;
                        fadeStartTime = Mathf.Min(fadeStartTime, now);
                    }
                    fading = FadingState.FadingOut; // when fade out ends, highlighted will be set to false in OnRenderObject
                } else {
                    fading = FadingState.NoFading;
                    ImmediateFadeOut();
                    requireUpdateMaterial = true;
                }
            }
        }

        void ImmediateFadeOut() {
            fading = FadingState.NoFading;
            _highlighted = false;
            if (OnObjectHighlightEnd != null) {
                OnObjectHighlightEnd(gameObject);
            }
            SendMessage("HighlightEnd", null, SendMessageOptions.DontRequireReceiver);
        }

        void SetupMaterial() {

#if UNITY_EDITOR
            staticChildren = false;
#endif

            if (target == null || fxMatMask == null)
                return;

            Renderer[] rr = null;
            switch (effectGroup) {
                case TargetOptions.OnlyThisObject:
                    if (target.TryGetComponent(out Renderer renderer) && ValidRenderer(renderer)) {
                        rr = new Renderer[1];
                        rr[0] = renderer;
                    }
                    break;
                case TargetOptions.RootToChildren:
                    Transform root = target;
                    while (root.parent != null) {
                        root = root.parent;
                    }
                    rr = FindRenderersInChildren(root);
                    break;
                case TargetOptions.LayerInScene: {
                        HighlightEffect eg = this;
                        if (target != transform) {
                            if (target.TryGetComponent(out HighlightEffect targetEffect)) { 
                                eg = targetEffect;
                            }
                        }
                        rr = FindRenderersWithLayerInScene(eg.effectGroupLayer);
                    }
                    break;
                case TargetOptions.LayerInChildren: {
                        HighlightEffect eg = this;
                        if (target != transform) {
                            if (target.TryGetComponent(out HighlightEffect targetEffect)) { 
                                eg = targetEffect;
                            }
                        }
                        rr = FindRenderersWithLayerInChildren(eg.effectGroupLayer);
                    }
                    break;
                case TargetOptions.Children:
                    rr = FindRenderersInChildren(target);
                    break;
                case TargetOptions.Scripting:
                    UpdateMaterialProperties();
                    return;
            }

            SetupMaterial(rr);
        }

        void SetupMaterial(Renderer[] rr) {

            if (rr == null) {
                rr = new Renderer[0];
            }
            if (rms == null || rms.Length < rr.Length) {
                rms = new ModelMaterials[rr.Length];
            }

            spriteMode = false;
            rmsCount = 0;
            for (int k = 0; k < rr.Length; k++) {
                rms[rmsCount].Init();
                Renderer renderer = rr[k];
                if (renderer == null) continue;
                if (effectGroup != TargetOptions.OnlyThisObject && !string.IsNullOrEmpty(effectNameFilter)) {
                    if (effectNameUseRegEx) {
                        try {
                            lastRegExError = "";
                            if (!Regex.IsMatch(renderer.name, effectNameFilter)) continue;
                        } catch (Exception ex) {
                            lastRegExError = ex.Message;
                            continue;
                        }
                    } else {
                        if (!renderer.name.Contains(effectNameFilter)) continue;
                    }
                }
                rms[rmsCount].renderer = renderer;
                rms[rmsCount].renderWasVisibleDuringSetup = renderer.isVisible;

                if (renderer.transform != target) {
                    if (renderer.TryGetComponent(out HighlightEffect otherEffect) && otherEffect.enabled && otherEffect.ignore) {
                        continue; // ignore this object
                    }
                }

                if (OnRendererHighlightStart != null) {
                    if (!OnRendererHighlightStart(renderer)) {
                        rmsCount++;
                        continue;
                    }
                }

                rms[rmsCount].isCombined = false;
                bool isSkinnedMesh = renderer is SkinnedMeshRenderer;
                rms[rmsCount].isSkinnedMesh = isSkinnedMesh;
                bool isSpriteRenderer = renderer is SpriteRenderer;
                rms[rmsCount].normalsOption = isSkinnedMesh || isSpriteRenderer ? NormalsOption.PreserveOriginal : normalsOption;
                if (rms[rmsCount].preserveOriginalMesh || combineMeshes) {
                    CheckCommandBuffers();
                }
                if (isSpriteRenderer) {
                    rms[rmsCount].mesh = quadMesh;
                    spriteMode = true;
                } else if (isSkinnedMesh) {
                    // ignore cloth skinned renderers
                    rms[rmsCount].mesh = ((SkinnedMeshRenderer)renderer).sharedMesh;
                } else if (Application.isPlaying && renderer.isPartOfStaticBatch) {
                    // static batched objects need to have a mesh collider in order to use its original mesh
                    if (renderer.TryGetComponent(out MeshCollider mc)) {
                        rms[rmsCount].mesh = mc.sharedMesh;
                    }
                } else {
                    if (renderer.TryGetComponent(out MeshFilter mf)) {
                        rms[rmsCount].mesh = mf.sharedMesh;

#if UNITY_EDITOR
                        if (renderer.gameObject.isStatic && renderer.GetComponent<MeshCollider>() == null) {
                            staticChildren = true;
                        }
#endif

                    }
                }

                if (rms[rmsCount].mesh == null) {
                    continue;
                }

                rms[rmsCount].transform = renderer.transform;
                Fork(fxMatMask, ref rms[rmsCount].fxMatMask, rms[rmsCount].mesh);
                Fork(fxMatSeeThroughInner, ref rms[rmsCount].fxMatSeeThroughInner, rms[rmsCount].mesh);
                Fork(fxMatSeeThroughBorder, ref rms[rmsCount].fxMatSeeThroughBorder, rms[rmsCount].mesh);
                Fork(fxMatOverlay, ref rms[rmsCount].fxMatOverlay, rms[rmsCount].mesh);
                Fork(fxMatInnerGlow, ref rms[rmsCount].fxMatInnerGlow, rms[rmsCount].mesh);
                Fork(fxMatSolidColor, ref rms[rmsCount].fxMatSolidColor, rms[rmsCount].mesh);
                rms[rmsCount].originalMesh = rms[rmsCount].mesh;
                if (!rms[rmsCount].preserveOriginalMesh) {
                    if (innerGlow > 0 || (glow > 0 && glowQuality != QualityLevel.Highest) || (outline > 0 && outlineQuality != QualityLevel.Highest)) {
                        if (normalsOption == NormalsOption.Reorient || normalsOption == NormalsOption.Planar) {
                            ReorientNormals(rmsCount);
                        } else {
                            AverageNormals(rmsCount);
                        }
                    }
                    // check if scale is negative
                    BakeTransform(rmsCount, true);
                }
                rmsCount++;
            }

#if UNITY_EDITOR
            // Avoids command buffer issue when refreshing asset inside the Editor
            if (!Application.isPlaying) {
                mipGlowBuffers = null;
                mipOutlineBuffers = null;
            }
#endif

            if (spriteMode) {
                outlineIndependent = false;
                outlineQuality = QualityLevel.Highest;
                glowQuality = QualityLevel.Highest;
                innerGlow = 0;
                cullBackFaces = false;
                seeThrough = SeeThroughMode.Never;
                if (alphaCutOff <= 0) {
                    alphaCutOff = 0.5f;
                }
            } else {
                if (combineMeshes) {
                    CombineMeshes();
                }
            }

            UpdateMaterialPropertiesNow();
        }

        readonly List<Renderer> tempRR = new List<Renderer>();

        bool ValidRenderer(Renderer r) {
            return r is MeshRenderer || r is SpriteRenderer || r is SkinnedMeshRenderer;
        }

        Renderer[] FindRenderersWithLayerInScene(LayerMask layer) {
            Renderer[] rr = Misc.FindObjectsOfType<Renderer>();
            tempRR.Clear();
            for (var i = 0; i < rr.Length; i++) {
                Renderer r = rr[i];
                if (((1 << r.gameObject.layer) & layer) != 0) {
                    if (ValidRenderer(r)) {
                        tempRR.Add(r);
                    }
                }
            }
            return tempRR.ToArray();
        }

        Renderer[] FindRenderersWithLayerInChildren(LayerMask layer) {
            Renderer[] rr = target.GetComponentsInChildren<Renderer>();
            tempRR.Clear();
            for (var i = 0; i < rr.Length; i++) {
                Renderer r = rr[i];
                if (((1 << r.gameObject.layer) & layer) != 0) {
                    if (ValidRenderer(r)) {
                        tempRR.Add(r);
                    }
                }
            }
            return tempRR.ToArray();
        }

        Renderer[] FindRenderersInChildren(Transform parent) {
            tempRR.Clear();
            parent.GetComponentsInChildren(tempRR);
            for (var i = 0; i < tempRR.Count; i++) {
                Renderer r = tempRR[i];
                if (!ValidRenderer(r)) {
                    tempRR.RemoveAt(i);
                    i--;
                }
            }
            return tempRR.ToArray();
        }

        void CheckGeometrySupportDependencies() {
            InitMaterial(ref fxMatMask, "HighlightPlus/Geometry/Mask");
            InitMaterial(ref fxMatOverlay, "HighlightPlus/Geometry/Overlay");
            InitMaterial(ref fxMatSeeThroughInner, "HighlightPlus/Geometry/SeeThroughInner");
            InitMaterial(ref fxMatSeeThroughBorder, "HighlightPlus/Geometry/SeeThroughBorder");
            InitMaterial(ref fxMatSeeThroughMask, "HighlightPlus/Geometry/SeeThroughMask");
            InitMaterial(ref fxMatSolidColor, "HighlightPlus/Geometry/SolidColor");
            InitMaterial(ref fxMatClearStencil, "HighlightPlus/ClearStencil");
            InitMaterial(ref fxMatOutlineRef, "HighlightPlus/Geometry/Outline");
            InitMaterial(ref fxMatGlowRef, "HighlightPlus/Geometry/Glow");
            if (fxMatGlowRef != null) {
                Texture2D noiseTex = Resources.Load<Texture2D>("HighlightPlus/blueNoiseVL");
                fxMatGlowRef.SetTexture(ShaderParams.NoiseTex, noiseTex);
            }
            InitMaterial(ref fxMatInnerGlow, "HighlightPlus/Geometry/InnerGlow");
            InitMaterial(ref fxMatTargetRef, "HighlightPlus/Geometry/Target");
            InitMaterial(ref fxMatComposeOutlineRef, "HighlightPlus/Geometry/ComposeOutline");
            InitMaterial(ref fxMatComposeGlowRef, "HighlightPlus/Geometry/ComposeGlow");
            InitMaterial(ref fxMatBlurOutlineRef, "HighlightPlus/Geometry/BlurOutline");
            InitMaterial(ref fxMatBlurGlowRef, "HighlightPlus/Geometry/BlurGlow");
            CheckRequiredCommandBuffers();
        }

        void CheckRequiredCommandBuffers() {
            if (cbGlow == null) {
                cbGlow = new CommandBuffer();
                cbGlow.name = "Outer Glow for " + name;
            }
            if (cbSeeThrough == null) {
                cbSeeThrough = new CommandBuffer();
                cbSeeThrough.name = "See Through for " + name;
            }
        }

        void CheckCommandBuffers() {
            if (cbMask == null) {
                cbMask = new CommandBuffer();
                cbMask.name = "Mask for " + name;
            }
            if (cbOutline == null) {
                cbOutline = new CommandBuffer();
                cbOutline.name = "Outline for " + name;
            }
            if (cbOverlay == null) {
                cbOverlay = new CommandBuffer();
                cbOverlay.name = "Overlay for " + name;
            }
            if (cbInnerGlow == null) {
                cbInnerGlow = new CommandBuffer();
                cbInnerGlow.name = "Inner Glow for " + name;
            }
        }

        void CheckBlurCommandBuffer() {
            if (cbSmoothBlend == null) {
                cbSmoothBlend = new CommandBuffer();
                cbSmoothBlend.name = "Smooth Blend for " + name;
            }
        }

        void Fork(Material mat, ref Material[] mats, Mesh mesh) {
            if (mesh == null)
                return;
            int count = mesh.subMeshCount;
            if (mats == null || mats.Length < count) {
                DestroyMaterialArray(mats);
                mats = new Material[count];
            }
            for (int k = 0; k < count; k++) {
                if (mats[k] == null) {
                    mats[k] = Instantiate(mat);
                }
            }
        }

        void BakeTransform(int objIndex, bool duplicateMesh) {
            if (rms[objIndex].mesh == null)
                return;
            Transform t = rms[objIndex].transform;
            Vector3 scale = t.localScale;
            if (scale.x >= 0 && scale.y >= 0 && scale.z >= 0) {
                rms[objIndex].bakedTransform = false;
                return;
            }
            // Duplicates mesh and bake rotation
            Mesh fixedMesh = duplicateMesh ? Instantiate(rms[objIndex].originalMesh) : rms[objIndex].mesh;
            Vector3[] vertices = fixedMesh.vertices;
            for (int k = 0; k < vertices.Length; k++) {
                vertices[k] = t.TransformPoint(vertices[k]);
            }
            fixedMesh.vertices = vertices;
            Vector3[] normals = fixedMesh.normals;
            if (normals != null) {
                for (int k = 0; k < normals.Length; k++) {
                    normals[k] = t.TransformVector(normals[k]).normalized;
                }
                fixedMesh.normals = normals;
            }
            fixedMesh.RecalculateBounds();
            rms[objIndex].mesh = fixedMesh;
            rms[objIndex].bakedTransform = true;
            rms[objIndex].currentPosition = t.position;
            rms[objIndex].currentRotation = t.eulerAngles;
            rms[objIndex].currentScale = t.lossyScale;
        }

        public void UpdateMaterialProperties(bool forceNow = false) {
            if (forceNow || !Application.isPlaying) {
                requireUpdateMaterial = false;
                UpdateMaterialPropertiesNow();
            } else {
                requireUpdateMaterial = true;
            }
        }


        void UpdateMaterialPropertiesNow() {

            if (rms == null)
                return;

            if (ignore) {
                _highlighted = false;
            }

            maskRequired = (_highlighted && (outline > 0 || (glow > 0 && !glowIgnoreMask))) || seeThrough != SeeThroughMode.Never || (targetFX && targetFXAlignToGround);
            usesSeeThrough = seeThroughIntensity > 0 && (seeThrough == SeeThroughMode.AlwaysWhenOccluded || (seeThrough == SeeThroughMode.WhenHighlighted && _highlighted));

            Color seeThroughTintColor = this.seeThroughTintColor;
            seeThroughTintColor.a = this.seeThroughTintAlpha;

            if (lastOutlineVisibility != outlineVisibility) {
                // change by scripting?
                if (glowQuality == QualityLevel.Highest && outlineQuality == QualityLevel.Highest) {
                    glowVisibility = outlineVisibility;
                }
                lastOutlineVisibility = outlineVisibility;
            }
            if (outlineWidth < 0) {
                outlineWidth = 0;
            }
            if (padding < 0) {
                padding = 0;
            }
            if (outlineQuality == QualityLevel.Medium) {
                outlineOffsetsMin = 4; outlineOffsetsMax = 7;
            } else if (outlineQuality == QualityLevel.High) {
                outlineOffsetsMin = 0; outlineOffsetsMax = 7;
            } else {
                outlineOffsetsMin = outlineOffsetsMax = 0;
            }
            if (glowWidth < 0) {
                glowWidth = 0;
            }
            if (glowQuality == QualityLevel.Medium) {
                glowOffsetsMin = 4; glowOffsetsMax = 7;
            } else if (glowQuality == QualityLevel.High) {
                glowOffsetsMin = 0; glowOffsetsMax = 7;
            } else {
                glowOffsetsMin = glowOffsetsMax = 0;
            }
            if (targetFXTransitionDuration <= 0) {
                targetFXTransitionDuration = 0.0001f;
            }
            if (targetFXStayDuration <= 0) {
                targetFXStayDuration = 0;
            }
            if (targetFXFadePower <= 0) {
                targetFXFadePower = 0;
            }
            if (seeThroughDepthOffset < 0) {
                seeThroughDepthOffset = 0;
            }
            if (seeThroughMaxDepth < 0) {
                seeThroughMaxDepth = 0;
            }
            if (seeThroughBorderWidth < 0) {
                seeThroughBorderWidth = 0;
            }
            outlineEdgeThreshold = Mathf.Clamp01(outlineEdgeThreshold);
            if (outlineSharpness < 1f) {
                outlineSharpness = 1f;
            }

            shouldBakeSkinnedMesh = optimizeSkinnedMesh && ((outline > 0 && outlineQuality != QualityLevel.Highest) || (glow > 0 && glowQuality != QualityLevel.Highest));

            // Setup materials
            if (usesSeeThrough && seeThroughChildrenSortingMode != SeeThroughSortingMode.Default && rms.Length > 0) {
                if (seeThroughChildrenSortingMode == SeeThroughSortingMode.SortByMaterialsRenderQueue) {
                    Array.Sort(rms, MaterialsRenderQueueComparer);
                } else {
                    Array.Sort(rms, MaterialsRenderQueueInvertedComparer);
                }
            }

            // Outline
            float scaledOutlineWidth = (outlineQuality == QualityLevel.High || normalsOption == NormalsOption.Planar) ? 0f : outlineWidth / 100f;
            Material matOutline = fxMatOutline;
            matOutline.SetFloat(ShaderParams.OutlineWidth, scaledOutlineWidth);
            matOutline.SetFloat(ShaderParams.OutlineVertexWidth, normalsOption == NormalsOption.Planar ? outlineWidth / 100f : 0);
            matOutline.SetVector(ShaderParams.OutlineDirection, Vector3.zero);
            matOutline.SetInt(ShaderParams.OutlineZTest, GetZTestValue(outlineVisibility));
            matOutline.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
            matOutline.SetFloat(ShaderParams.ConstantWidth, constantWidth ? 1.0f : 0);
            matOutline.SetFloat(ShaderParams.MinimumWidth, minimumWidth);
            bool useSmoothOutline = outline > 0 && outlineQuality == QualityLevel.Highest;
            if (useSmoothOutline) {
                CheckBlurCommandBuffer();
                fxMatComposeOutline.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                fxMatComposeOutline.SetColor(ShaderParams.Debug, outlineBlitDebug ? debugColor : blackColor);
                fxMatComposeOutline.SetFloat(ShaderParams.OutlineSharpness, outlineSharpness);
                if (outlineEdgeMode == OutlineEdgeMode.Exterior) {
                    fxMatComposeOutline.SetInt(ShaderParams.OutlineStencilCompareFunction, (int)CompareFunction.NotEqual);
                    fxMatComposeOutline.DisableKeyword(ShaderParams.SKW_ALL_EDGES);
                } else {
                    fxMatComposeOutline.SetInt(ShaderParams.OutlineStencilCompareFunction, (int)CompareFunction.Always);
                    fxMatComposeOutline.EnableKeyword(ShaderParams.SKW_ALL_EDGES);
                    outlineDownsampling = 1;
                }
                float edgeWidth = outlineWidth;
                if (outlineEdgeMode == OutlineEdgeMode.Any) edgeWidth = Mathf.Clamp(edgeWidth, outlineBlurPasses / 5f, outlineBlurPasses);
                fxMatBlurOutline.SetFloat(ShaderParams.BlurScale, edgeWidth / outlineDownsampling);
                fxMatBlurOutline.SetFloat(ShaderParams.BlurScaleFirstHoriz, edgeWidth * 2f);
            }
            matOutline.DisableKeyword(ShaderParams.SKW_OUTLINE_GRADIENT_WS);
            matOutline.DisableKeyword(ShaderParams.SKW_OUTLINE_GRADIENT_LS);
            if (outlineColorStyle == ColorStyle.Gradient) {
                const int OUTLINE_GRADIENT_TEX_SIZE = 32;
                bool requiresUpdate = false;
                if (outlineGradientTex == null) {
                    outlineGradientTex = new Texture2D(OUTLINE_GRADIENT_TEX_SIZE, 1, TextureFormat.RGBA32, mipChain: false, linear: true);
                    outlineGradientTex.wrapMode = TextureWrapMode.Clamp;
                    requiresUpdate = true;
                }
                if (outlineGradientColors == null || outlineGradientColors.Length != OUTLINE_GRADIENT_TEX_SIZE) {
                    outlineGradientColors = new Color[OUTLINE_GRADIENT_TEX_SIZE];
                    requiresUpdate = true;
                }
                for (int k = 0; k < OUTLINE_GRADIENT_TEX_SIZE; k++) {
                    float t = (float)k / OUTLINE_GRADIENT_TEX_SIZE;
                    Color color = outlineGradient.Evaluate(t);
                    if (color != outlineGradientColors[k]) {
                        outlineGradientColors[k] = color;
                        requiresUpdate = true;
                    }
                }
                if (requiresUpdate) {
                    outlineGradientTex.SetPixels(outlineGradientColors);
                    outlineGradientTex.Apply();
                }
                matOutline.SetTexture(ShaderParams.OutlineGradientTex, outlineGradientTex);
                matOutline.EnableKeyword(outlineGradientInLocalSpace ? ShaderParams.SKW_OUTLINE_GRADIENT_LS : ShaderParams.SKW_OUTLINE_GRADIENT_WS);
            }

            // Outer Glow
            Material matGlow = fxMatGlow;
            float actualOutlineWidth = outline > 0 ? outlineWidth : 0;
            matGlow.SetVector(ShaderParams.Glow2, new Vector4(normalsOption == NormalsOption.Planar ? 0 : actualOutlineWidth / 100f, glowAnimationSpeed, glowDithering));
            if (glowDitheringStyle == GlowDitheringStyle.Noise) {
                matGlow.EnableKeyword(ShaderParams.SKW_DITHER_BLUENOISE);
            } else {
                matGlow.DisableKeyword(ShaderParams.SKW_DITHER_BLUENOISE);
            }
            matGlow.SetInt(ShaderParams.GlowZTest, GetZTestValue(glowVisibility));
            matGlow.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
            matGlow.SetFloat(ShaderParams.ConstantWidth, constantWidth ? 1.0f : 0);
            matGlow.SetFloat(ShaderParams.MinimumWidth, minimumWidth);
            matGlow.SetInt(ShaderParams.GlowStencilOp, glowBlendPasses ? (int)StencilOp.Keep : (int)StencilOp.Replace);
            matGlow.SetInt(ShaderParams.GlowStencilComp, glowIgnoreMask ? (int)CompareFunction.Always : (int)CompareFunction.NotEqual);

            bool useSmoothGlow = glow > 0 && glowQuality == QualityLevel.Highest;
            if (useSmoothGlow) {
                CheckBlurCommandBuffer();
                fxMatComposeGlow.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                fxMatComposeGlow.SetInt(ShaderParams.GlowStencilComp, glowIgnoreMask ? (int)CompareFunction.Always : (int)CompareFunction.NotEqual);
                if (glowBlendMode == GlowBlendMode.Additive) {
                    fxMatComposeGlow.SetInt(ShaderParams.BlendSrc, (int)BlendMode.One);
                    fxMatComposeGlow.SetInt(ShaderParams.BlendDst, (int)BlendMode.One);
                } else {
                    fxMatComposeGlow.SetInt(ShaderParams.BlendSrc, (int)BlendMode.SrcAlpha);
                    fxMatComposeGlow.SetInt(ShaderParams.BlendDst, (int)BlendMode.OneMinusSrcAlpha);
                }
                fxMatBlurGlow.SetFloat(ShaderParams.BlurScale, glowWidth / glowDownsampling);
                fxMatBlurGlow.SetFloat(ShaderParams.Speed, glowAnimationSpeed);
            }
            // Target
            if (targetFX) {
                if (targetFXTexture == null) {
                    targetFXTexture = Resources.Load<Texture2D>("HighlightPlus/target");
                }
                fxMatTarget.mainTexture = targetFXTexture;
                fxMatTarget.SetInt(ShaderParams.ZTest, GetZTestValue(targetFXVisibility));
            }

            // Per object materials
            bool renderMaskOnTop = _highlighted && ((outline > 0 && outlineVisibility != Visibility.Normal) || (glow > 0 && glowVisibility != Visibility.Normal) || (innerGlow > 0 && innerGlowVisibility != Visibility.Normal));
            bool useVisiblePartContour = outlineContourStyle == ContourStyle.AroundVisibleParts || !(useSmoothGlow || useSmoothOutline);
            for (int k = 0; k < rmsCount; k++) {

                if (rms[k].mesh != null) {

                    Renderer renderer = rms[k].renderer;
                    if (renderer == null)
                        continue;

                    // Mask, See-through & Overlay per submesh
                    for (int l = 0; l < rms[k].mesh.subMeshCount; l++) {
                        if (((1 << l) & subMeshMask) == 0) continue;

                        Material mat = null;
                        renderer.GetSharedMaterials(rendererSharedMaterials);
                        if (l < rendererSharedMaterials.Count) {
                            mat = rendererSharedMaterials[l];
                        }
                        if (mat == null)
                            continue;

                        bool hasTexture = mat.HasProperty(ShaderParams.MainTex);
                        bool useAlphaTest = alphaCutOff > 0 && hasTexture;

                        // Mask
                        if (rms[k].fxMatMask != null && rms[k].fxMatMask.Length > l) {
                            Material fxMat = rms[k].fxMatMask[l];
                            if (fxMat != null) {
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                                if (depthClip && useVisiblePartContour && !renderMaskOnTop) {
                                    fxMat.EnableKeyword(ShaderParams.SKW_DEPTHCLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_DEPTHCLIP);
                                }
                                fxMat.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                                fxMat.SetFloat(ShaderParams.ZShift, spriteMode ? 0 : 0.0001f);
                                fxMat.SetFloat(ShaderParams.Padding, padding);
                            }
                        }

                        // See-through inner
                        bool usesSeeThroughBorder = rms[k].fxMatSeeThroughBorder != null && rms[k].fxMatSeeThroughBorder.Length > l && (seeThroughBorder * seeThroughBorderWidth > 0);
                        if (rms[k].fxMatSeeThroughInner != null && rms[k].fxMatSeeThroughInner.Length > l) {
                            Material fxMat = rms[k].fxMatSeeThroughInner[l];
                            if (fxMat != null) {
                                fxMat.SetFloat(ShaderParams.SeeThrough, seeThroughIntensity);
                                fxMat.SetFloat(ShaderParams.SeeThroughNoise, seeThroughNoise);
                                fxMat.SetColor(ShaderParams.SeeThroughTintColor, seeThroughTintColor);
                                if (seeThroughOccluderMaskAccurate && seeThroughOccluderMask != -1) {
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilRef, 1);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilComp, (int)CompareFunction.Equal);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilPassOp, (int)StencilOp.Zero);
                                } else {
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilRef, 2);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilComp, (int)CompareFunction.Greater);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilPassOp, (int)StencilOp.Replace);
                                }
                                if (seeThroughOrdered) {
                                    fxMat.SetInt(ShaderParams.ZTest, (int)CompareFunction.LessEqual);
                                    fxMat.SetInt(ShaderParams.SeeThroughOrdered, 1);
                                } else {
                                    fxMat.SetInt(ShaderParams.ZTest, (int)CompareFunction.Greater);
                                    fxMat.SetInt(ShaderParams.SeeThroughOrdered, 0);
                                }
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                                if (seeThroughDepthOffset > 0 || seeThroughMaxDepth > 0) {
                                    fxMat.SetFloat(ShaderParams.SeeThroughDepthOffset, seeThroughDepthOffset > 0 ? seeThroughDepthOffset : -1);
                                    fxMat.SetFloat(ShaderParams.SeeThroughMaxDepth, seeThroughMaxDepth > 0 ? seeThroughMaxDepth : 999999);
                                    fxMat.EnableKeyword(ShaderParams.SKW_DEPTH_OFFSET);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_DEPTH_OFFSET);
                                }
                                if (seeThroughBorderOnly) {
                                    fxMat.EnableKeyword(ShaderParams.SKW_SEETHROUGH_ONLY_BORDER);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_SEETHROUGH_ONLY_BORDER);
                                }

                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_TRIPLANAR);
                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_OBJECTSPACE);
                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_SCREENSPACE);
                                if (seeThroughTexture != null) {
                                    fxMat.SetTexture(ShaderParams.SeeThroughTexture, seeThroughTexture);
                                    fxMat.SetFloat(ShaderParams.SeeThroughTextureScale, seeThroughTextureScale);
                                    switch (seeThroughTextureUVSpace) {
                                        case TextureUVSpace.ScreenSpace:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_SCREENSPACE);
                                            break;
                                        case TextureUVSpace.ObjectSpace:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_OBJECTSPACE);
                                            break;
                                        default:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_TRIPLANAR);
                                            break;
                                    }
                                }

                            }
                        }

                        // See-through border
                        if (usesSeeThroughBorder) {
                            Material fxMat = rms[k].fxMatSeeThroughBorder[l];
                            if (fxMat != null) {
                                fxMat.SetColor(ShaderParams.SeeThroughBorderColor, new Color(seeThroughBorderColor.r, seeThroughBorderColor.g, seeThroughBorderColor.b, seeThroughBorder));
                                fxMat.SetFloat(ShaderParams.SeeThroughBorderWidth, (seeThroughBorder * seeThroughBorderWidth) > 0 ? seeThroughBorderWidth / 100f : 0);
                                fxMat.SetFloat(ShaderParams.SeeThroughBorderConstantWidth, constantWidth ? 1.0f : 0);
                                fxMat.SetFloat(ShaderParams.SeeThroughBorderMinimumWidth, minimumWidth);
                                if (seeThroughOccluderMaskAccurate && seeThroughOccluderMask != -1) {
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilRef, 1);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilComp, (int)CompareFunction.Equal);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilPassOp, (int)StencilOp.Zero);
                                } else {
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilRef, 2);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilComp, (int)CompareFunction.Greater);
                                    fxMat.SetInt(ShaderParams.SeeThroughStencilPassOp, (int)StencilOp.Replace);
                                }
                                if (seeThroughOrdered) {
                                    fxMat.SetInt(ShaderParams.ZTest, (int)CompareFunction.LessEqual);
                                    fxMat.SetInt(ShaderParams.SeeThroughOrdered, 1);
                                } else {
                                    fxMat.SetInt(ShaderParams.ZTest, (int)CompareFunction.Greater);
                                    fxMat.SetInt(ShaderParams.SeeThroughOrdered, 0);
                                }
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                                if (seeThroughDepthOffset > 0 || seeThroughMaxDepth > 0) {
                                    fxMat.SetFloat(ShaderParams.SeeThroughDepthOffset, seeThroughDepthOffset > 0 ? seeThroughDepthOffset : -1);
                                    fxMat.SetFloat(ShaderParams.SeeThroughMaxDepth, seeThroughMaxDepth > 0 ? seeThroughMaxDepth : 999999);
                                    fxMat.EnableKeyword(ShaderParams.SKW_DEPTH_OFFSET);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_DEPTH_OFFSET);
                                }
                            }
                        }

                        // Overlay
                        if (rms[k].fxMatOverlay != null && rms[k].fxMatOverlay.Length > l) {
                            Material fxMat = rms[k].fxMatOverlay[l];
                            if (fxMat != null) {
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                if (mat.HasProperty(ShaderParams.Color)) {
                                    fxMat.SetColor(ShaderParams.OverlayBackColor, mat.GetColor(ShaderParams.Color));
                                }
                                fxMat.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                                fxMat.SetInt(ShaderParams.OverlayZTest, GetZTestValue(overlayVisibility));
                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_TRIPLANAR);
                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_OBJECTSPACE);
                                fxMat.DisableKeyword(ShaderParams.SKW_TEXTURE_SCREENSPACE);
                                if (overlayTexture != null) {
                                    fxMat.SetTexture(ShaderParams.OverlayTexture, overlayTexture);
                                    fxMat.SetVector(ShaderParams.OverlayTextureScrolling, overlayTextureScrolling);
                                    switch (overlayTextureUVSpace) {
                                        case TextureUVSpace.ScreenSpace:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_SCREENSPACE);
                                            break;
                                        case TextureUVSpace.ObjectSpace:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_OBJECTSPACE);
                                            break;
                                        default:
                                            fxMat.EnableKeyword(ShaderParams.SKW_TEXTURE_TRIPLANAR);
                                            break;
                                    }
                                }

                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                            }
                        }

                        // Inner Glow
                        if (rms[k].fxMatInnerGlow != null && rms[k].fxMatInnerGlow.Length > l) {
                            Material fxMat = rms[k].fxMatInnerGlow[l];
                            if (fxMat != null) {
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                fxMat.SetFloat(ShaderParams.InnerGlowWidth, innerGlowWidth);
                                fxMat.SetInt(ShaderParams.InnerGlowZTest, GetZTestValue(innerGlowVisibility));
                                fxMat.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                                fxMat.SetInt(ShaderParams.InnerGlowBlendMode, innerGlowBlendMode == InnerGlowBlendMode.Additive ? 1 : 10);
                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                            }
                        }

                        // Solid Color for smooth glow
                        if (rms[k].fxMatSolidColor != null && rms[k].fxMatSolidColor.Length > l) {
                            Material fxMat = rms[k].fxMatSolidColor[l];
                            if (fxMat != null) {
                                fxMat.color = glowHQColor;
                                fxMat.SetInt(ShaderParams.Cull, cullBackFaces ? (int)CullMode.Back : (int)CullMode.Off);
                                fxMat.SetInt(ShaderParams.ZTest, GetZTestValue(useSmoothGlow ? glowVisibility : outlineVisibility));
                                fxMat.SetFloat(ShaderParams.Padding, padding);
                                fxMat.SetFloat(ShaderParams.OutlineEdgeThreshold, outlineEdgeThreshold);
                                if (hasTexture) {
                                    Texture texture = mat.mainTexture;
                                    fxMat.mainTexture = texture;
                                    fxMat.mainTextureOffset = mat.mainTextureOffset;
                                    fxMat.mainTextureScale = mat.mainTextureScale;
                                }
                                if (useAlphaTest) {
                                    fxMat.SetFloat(ShaderParams.CutOff, alphaCutOff);
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALPHACLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALPHACLIP);
                                }
                                if (depthClip && !renderMaskOnTop) {
                                    fxMat.EnableKeyword(ShaderParams.SKW_DEPTHCLIP);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_DEPTHCLIP);
                                }
                                if (outlineEdgeMode == OutlineEdgeMode.Any) {
                                    fxMat.EnableKeyword(ShaderParams.SKW_ALL_EDGES);
                                } else {
                                    fxMat.DisableKeyword(ShaderParams.SKW_ALL_EDGES);
                                }
                                fxMat.SetFloat(ShaderParams.ZShift, spriteMode ? 0 : 0.0001f);
                            }
                        }
                    }
                }
            }
        }

        int MaterialsRenderQueueComparer(ModelMaterials m1, ModelMaterials m2) {
            Material mat1 = m1.renderer != null ? m1.renderer.sharedMaterial : null;
            Material mat2 = m2.renderer != null ? m2.renderer.sharedMaterial : null;
            int mq1 = mat1 != null ? mat1.renderQueue : 0;
            int mq2 = mat2 != null ? mat2.renderQueue : 0;
            return mq1.CompareTo(mq2);
        }

        int MaterialsRenderQueueInvertedComparer(ModelMaterials m1, ModelMaterials m2) {
            Material mat1 = m1.renderer != null ? m1.renderer.sharedMaterial : null;
            Material mat2 = m2.renderer != null ? m2.renderer.sharedMaterial : null;
            int mq1 = mat1 != null ? mat1.renderQueue : 0;
            int mq2 = mat2 != null ? mat2.renderQueue : 0;
            return mq2.CompareTo(mq1);
        }

        float ComputeCameraDistanceFade(Vector3 position, Transform cameraTransform) {
            Vector3 heading = position - cameraTransform.position;
            float distance = Vector3.Dot(heading, cameraTransform.forward);
            if (distance < cameraDistanceFadeNear) {
                return 1f - Mathf.Min(1f, cameraDistanceFadeNear - distance);
            }
            if (distance > cameraDistanceFadeFar) {
                return 1f - Mathf.Min(1f, distance - cameraDistanceFadeFar);
            }
            return 1f;
        }

        int GetZTestValue(Visibility param) {
            switch (param) {
                case Visibility.AlwaysOnTop:
                    return (int)CompareFunction.Always;
                case Visibility.OnlyWhenOccluded:
                    return (int)CompareFunction.Greater;
                default:
                    return (int)CompareFunction.LessEqual;
            }
        }

        void BuildQuad() {
            quadMesh = new Mesh();

            // Setup vertices
            Vector3[] newVertices = new Vector3[4];
            float halfHeight = 0.5f;
            float halfWidth = 0.5f;
            newVertices[0] = new Vector3(-halfWidth, -halfHeight, 0);
            newVertices[1] = new Vector3(-halfWidth, halfHeight, 0);
            newVertices[2] = new Vector3(halfWidth, -halfHeight, 0);
            newVertices[3] = new Vector3(halfWidth, halfHeight, 0);

            // Setup UVs
            Vector2[] newUVs = new Vector2[newVertices.Length];
            newUVs[0] = new Vector2(0, 0);
            newUVs[1] = new Vector2(0, 1);
            newUVs[2] = new Vector2(1, 0);
            newUVs[3] = new Vector2(1, 1);

            // Setup triangles
            int[] newTriangles = { 0, 1, 2, 3, 2, 1 };

            // Setup normals
            Vector3[] newNormals = new Vector3[newVertices.Length];
            for (int i = 0; i < newNormals.Length; i++) {
                newNormals[i] = Vector3.forward;
            }

            // Create quad
            quadMesh.vertices = newVertices;
            quadMesh.uv = newUVs;
            quadMesh.triangles = newTriangles;
            quadMesh.normals = newNormals;

            quadMesh.RecalculateBounds();
        }

        void BuildCube() {
            cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        }


        /// <summary>
        /// Returns true if a given transform is included in this effect
        /// </summary>
        public bool Includes(Transform transform) {
            for (int k = 0; k < rmsCount; k++) {
                if (rms[k].transform == transform) return true;
            }
            return false;
        }

        /// <summary>
        /// Updates profile glow color
        /// </summary>
        public void SetGlowColor(Color color) {
            if (glowPasses != null) {
                for (int k = 0; k < glowPasses.Length; k++) {
                    glowPasses[k].color = color;
                }
            }
            glowHQColor = color;
            UpdateMaterialProperties();
        }


        #region Normals handling

        static List<Vector3> vertices;
        static List<Vector3> normals;
        static Vector3[] newNormals;
        static int[] matches;
        static readonly Dictionary<Vector3, int> vv = new Dictionary<Vector3, int>();
        static readonly List<Material> rendererSharedMaterials = new List<Material>();

        // cached meshes
        static readonly Dictionary<int, Mesh> smoothMeshes = new Dictionary<int, Mesh>();
        static readonly Dictionary<int, Mesh> reorientedMeshes = new Dictionary<int, Mesh>();
        static readonly Dictionary<int, Mesh> combinedMeshes = new Dictionary<int, Mesh>();
        int combinedMeshesHashId;

        // annotate usage count of the instanced meshes due to normals or combine mesh option
        // when highlighte effect is destroyed and the usage count is zero, we destroy the cached mesh
        static readonly Dictionary<Mesh, int> sharedMeshUsage = new Dictionary<Mesh, int>();
        readonly List<Mesh> instancedMeshes = new List<Mesh>();

        void AverageNormals(int objIndex) {
            if (rms == null || objIndex >= rms.Length) return;
            Mesh mesh = rms[objIndex].mesh;

            Mesh newMesh;
            int hashCode = mesh.GetHashCode();
            if (!smoothMeshes.TryGetValue(hashCode, out newMesh) || newMesh == null) {
                if (!mesh.isReadable) return;
                if (normals == null) {
                    normals = new List<Vector3>();
                } else {
                    normals.Clear();
                }
                mesh.GetNormals(normals);
                int normalsCount = normals.Count;
                if (normalsCount == 0)
                    return;
                if (vertices == null) {
                    vertices = new List<Vector3>();
                } else {
                    vertices.Clear();
                }
                mesh.GetVertices(vertices);
                int vertexCount = vertices.Count;
                if (normalsCount < vertexCount) {
                    vertexCount = normalsCount;
                }
                if (newNormals == null || newNormals.Length < vertexCount) {
                    newNormals = new Vector3[vertexCount];
                } else {
                    Vector3 zero = Vector3.zero;
                    for (int k = 0; k < vertexCount; k++) {
                        newNormals[k] = zero;
                    }
                }
                if (matches == null || matches.Length < vertexCount) {
                    matches = new int[vertexCount];
                }
                // Locate overlapping vertices
                vv.Clear();
                for (int k = 0; k < vertexCount; k++) {
                    Vector3 v = vertices[k];
                    if (!vv.TryGetValue(v, out int i)) {
                        vv[v] = i = k;
                    }
                    matches[k] = i;
                }
                // Average normals
                for (int k = 0; k < vertexCount; k++) {
                    int match = matches[k];
                    newNormals[match] += normals[k];
                }
                for (int k = 0; k < vertexCount; k++) {
                    int match = matches[k];
                    normals[k] = newNormals[match].normalized;
                }
                // Reassign normals
                newMesh = Instantiate(mesh);
                newMesh.SetNormals(normals);
                smoothMeshes[hashCode] = newMesh;
                IncrementeMeshUsage(newMesh);
            }
            rms[objIndex].mesh = newMesh;
        }


        void ReorientNormals(int objIndex) {
            if (rms == null || objIndex >= rms.Length) return;
            Mesh mesh = rms[objIndex].mesh;

            Mesh newMesh;
            int hashCode = mesh.GetHashCode();
            if (!reorientedMeshes.TryGetValue(hashCode, out newMesh) || newMesh == null) {
                if (!mesh.isReadable) return;
                if (normals == null) {
                    normals = new List<Vector3>();
                } else {
                    normals.Clear();
                }
                if (vertices == null) {
                    vertices = new List<Vector3>();
                } else {
                    vertices.Clear();
                }
                mesh.GetVertices(vertices);
                int vertexCount = vertices.Count;
                if (vertexCount == 0) return;

                Vector3 mid = Vector3.zero;
                for (int k = 0; k < vertexCount; k++) {
                    mid += vertices[k];
                }
                mid /= vertexCount;
                // Average normals
                for (int k = 0; k < vertexCount; k++) {
                    normals.Add((vertices[k] - mid).normalized);
                }
                // Reassign normals
                newMesh = Instantiate(mesh);
                newMesh.SetNormals(normals);
                reorientedMeshes[hashCode] = newMesh;
                IncrementeMeshUsage(newMesh);
            }
            rms[objIndex].mesh = newMesh;
        }

        const int MAX_VERTEX_COUNT = 65535;
        void CombineMeshes() {

            if (rmsCount <= 1) return;

            // Combine meshes of group into the first mesh in rms
            if (combineInstances == null || combineInstances.Length != rmsCount) {
                combineInstances = new CombineInstance[rmsCount];
            }
            int first = -1;
            int count = 0;
            combinedMeshesHashId = 0;
            int vertexCount = 0;
            Matrix4x4 im = Matrix4x4.identity;
            for (int k = 0; k < rmsCount; k++) {
                combineInstances[k].mesh = null;

                if (rms[k].isSkinnedMesh) continue;

                Mesh mesh = rms[k].mesh;
                if (mesh == null || !mesh.isReadable) continue;

                if (vertexCount + mesh.vertexCount > MAX_VERTEX_COUNT) continue;

                vertexCount += mesh.vertexCount;
                combineInstances[count].mesh = mesh;
                int instanceId = rms[k].renderer.gameObject.GetInstanceID();
                if (first < 0) {
                    first = k;
                    combinedMeshesHashId = instanceId;
                    im = rms[k].transform.worldToLocalMatrix;
                } else {
                    combinedMeshesHashId ^= instanceId;
                    rms[k].mesh = null;
                }
                combineInstances[count].transform = im * rms[k].transform.localToWorldMatrix;
                count++;

            }
            if (count < 2) return;

            if (count != rmsCount) {
                Array.Resize(ref combineInstances, count);
            }

            if (!combinedMeshes.TryGetValue(combinedMeshesHashId, out Mesh combinedMesh) || combinedMesh == null) {
                combinedMesh = new Mesh();
                combinedMesh.CombineMeshes(combineInstances, true, true);
                combinedMeshes[combinedMeshesHashId] = combinedMesh;
                IncrementeMeshUsage(combinedMesh);
            }
            rms[first].mesh = combinedMesh;
            rms[first].isCombined = true;
        }


        void IncrementeMeshUsage(Mesh mesh) {
            int usageCount;
            sharedMeshUsage.TryGetValue(mesh, out usageCount);
            usageCount++;
            sharedMeshUsage[mesh] = usageCount;
            instancedMeshes.Add(mesh);
        }

        /// <summary>
        /// Destroys any cached mesh
        /// </summary>
        public static void ClearMeshCache() {
            foreach (Mesh mesh in combinedMeshes.Values) {
                if (mesh != null) DestroyImmediate(mesh);
            }
            foreach (Mesh mesh in smoothMeshes.Values) {
                if (mesh != null) DestroyImmediate(mesh);
            }
            foreach (Mesh mesh in reorientedMeshes.Values) {
                if (mesh != null) DestroyImmediate(mesh);
            }
        }

        /// <summary>
        /// Clears cached mesh only for the highlighted object
        /// </summary>
        void RefreshCachedMeshes() {
            if (combinedMeshes.TryGetValue(combinedMeshesHashId, out Mesh combinedMesh)) {
                DestroyImmediate(combinedMesh);
                combinedMeshes.Remove(combinedMeshesHashId);
            }
            if (rms == null) return;
            for (int k = 0; k < rms.Length; k++) {
                Mesh mesh = rms[k].mesh;
                if (mesh != null) {
                    if (smoothMeshes.ContainsValue(mesh) || reorientedMeshes.ContainsValue(mesh)) {
                        DestroyImmediate(mesh);
                    }
                }

            }
        }
        #endregion

    }
}
