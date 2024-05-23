using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Presentation.Animations.Types;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.Services.Animations;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Animations;
using Sources.Presentations.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.UiFramework.Presentation.Animations
{
    public class UiAnimator : View
    {
        [DisplayAsString(false)] [HideLabel] [Indent(8)]
        [SerializeField] private string _label = UiConstant.UiAnimatorLabel;
        
        [Space(15)]
        [EnumToggleButtons] [LabelText("FPS", SdfIconType.Activity)] 
        [SerializeField] private AnimationFps _animationFps = AnimationFps.Thirty;
        
        [TabGroup("Types")] [PropertyTooltip(UiTooltipConstant.AnimationType)]
        [SerializeField] private AnimationType _animationType = AnimationType.Scale;
        
        [TabGroup("Types")] [PropertyTooltip(UiTooltipConstant.ReactionAnimationType)]
        [SerializeField] private ReactionAnimationType _reactionAnimationType = ReactionAnimationType.ButtonClick;

        [EnableIf(nameof(_animationType), AnimationType.Scale)] 
        [TabGroup("ScaleSettings")] 
        [SerializeField] private ScaleAnimationType _scaleAnimationType;

        [EnableIf(nameof(_animationType), AnimationType.Scale)] 
        [TabGroup("ScaleSettings")] 
        [SerializeField] private float _animationDuration = 0.1f;
        
        [EnableIf(nameof(_animationType), AnimationType.Scale)] 
        [TabGroup("ScaleSettings")] 
        [SerializeField] private AnimationCurve _scaleAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [EnableIf(nameof(_animationType), AnimationType.Scale)] 
        [TabGroup("ScaleSettings")]
        [SerializeField] private Vector3 _fromScale = new Vector3(1, 1, 1);

        [EnableIf(nameof(_animationType), AnimationType.Scale)] 
        [TabGroup("ScaleSettings")] 
        [SerializeField] private Vector3 _targetScale = new Vector3(0.7f, 0.7f, 0.7f);

        [EnableIf(nameof(_animationType), AnimationType.Rotate)] 
        [TabGroup("RotateSettings")] 
        [SerializeField] private AnimationCurve _rotateAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [EnableIf(nameof(_animationType), AnimationType.Rotate)] 
        [TabGroup("RotateSettings")] 
        [SerializeField] private Vector3 _rotationVector = new Vector3(0, 0, -0.5f);
        
        private readonly IAnimationService _animationService = new AnimationService();

        public AnimationType AnimationType => _animationType;
        public ReactionAnimationType ReactionAnimationType => _reactionAnimationType;
        public ScaleAnimationType ScaleAnimationType => _scaleAnimationType;
        public float AnimationDuration => _animationDuration;
        public Vector3 FromScale => _fromScale;
        public Vector3 TargetScale => _targetScale;
        public Vector3 RotationVector => _rotationVector;

        private void Awake()
        {
            _animationService.Construct(this);
            _animationService.Awake();
        }

        private void OnEnable() =>
            _animationService.Enable();

        private void OnDisable() =>
            _animationService.Disable();

        private void OnDestroy() =>
            _animationService.Destroy();
    }
}