using System;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.Reactor.Internal;
using Doozy.Runtime.Colors;
using Doozy.Runtime.Common.Events;
using Doozy.Runtime.Reactor.Easings;
using Doozy.Runtime.Reactor.Internal;
using Doozy.Runtime.Reactor.Reactions;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.IntSlider
{
    public class FluidIntSlider : VisualElement
    {
        public FluidIntSlider()
        {
            slider = new SliderInt()
                .ResetLayout();
            
            this
                .SetStyleFlexShrink(0)
                .SetStyleFlexGrow(1)
                .SetStylePaddingLeft(DesignUtils.k_Spacing2X)
                .SetStylePaddingRight(DesignUtils.k_Spacing2X);
            
            labelsContainer =
                new VisualElement()
                    .SetName("Labels Container")
                    .SetStyleFlexDirection(FlexDirection.Row)
                    .SetStyleJustifyContent(Justify.Center)
                    .SetStyleAlignItems(Align.FlexStart)
                    .SetStyleMarginTop(4)
                    .SetStyleFlexGrow(1);

            sliderContainer =
                new VisualElement()
                    .SetStyleHeight(28)
                    .SetName("Slider Container")
                    .SetStyleFlexGrow(1)
                    .SetStylePaddingTop(8);

            snapIntervalIndicatorsContainer =
                new VisualElement()
                    .SetName("Snap Interval Indicators Container")
                    .SetStylePosition(Position.Absolute)
                    .SetStyleLeft(0)
                    .SetStyleTop(0)
                    .SetStyleRight(0)
                    .SetStyleBottom(0);

            snapValuesIndicatorsContainer =
                new VisualElement()
                    .SetName("Snap Values Indicators Container")
                    .SetStylePosition(Position.Absolute)
                    .SetStyleLeft(0)
                    .SetStyleTop(0)
                    .SetStyleRight(0)
                    .SetStyleBottom(0);
            
            sliderTracker = slider.Q<VisualElement>("unity-tracker");
            sliderDraggerBorder = slider.Q<VisualElement>("unity-dragger-border");
            sliderDragger = slider.Q<VisualElement>("unity-dragger");
            
            sliderDragger.SetStyleBorderColor(EditorColors.Default.BoxBackground.WithRGBShade(0.4f));

            resetToValueReaction =
                Reaction
                    .Get<FloatReaction>()
                    .SetEditorHeartbeat()
                    .SetDuration(0.34f)
                    .SetEase(Ease.OutExpo);

            Initialize();
            Compose();
            
            Add(slider);
        }
        
        public VisualElement labelsContainer { get; }
        public VisualElement snapIntervalIndicatorsContainer { get; }
        public VisualElement snapValuesIndicatorsContainer { get; }
        public VisualElement sliderContainer { get; }
        public SliderInt slider { get; }

        public Label lowValueLabel { get; private set; }
        public Label highValueLabel { get; private set; }
        public Label valueLabel { get; private set; }

        public bool snapToInterval { get; set; } = true;
        public float snapInterval { get; set; } = 0.1f;

        public bool snapToValues { get; set; } = false;
        public float[] snapValues { get; set; } = { 0.1f, 0.5f, 1f, 2f, 5f, 10f };
        public float snapValuesInterval { get; set; } = 0.1f;

        public bool autoResetToValue { get; set; } = false;
        public float autoResetValue { get; set; } = 0f;

        public UnityEvent onStartValueChange { get; } = new UnityEvent();
        public UnityEvent onEndValueChange { get; } = new UnityEvent();
        public FloatEvent onMinValueChanged { get; } = new FloatEvent();
        public FloatEvent onMaxValueChanged { get; } = new FloatEvent();
        public Action<Vector2> onValueChanged;

        private FloatReaction resetToValueReaction { get; set; }

        public VisualElement sliderTracker { get; }
        public VisualElement sliderDraggerBorder { get; }
        public VisualElement sliderDragger { get; }

        private const float TRACKER_OFFSET = 4;
        private float sliderTrackerWidth => sliderTracker.resolvedStyle.width - TRACKER_OFFSET * 2;
        
        private void Initialize()
        {
            Label GetLabel() =>
                DesignUtils.fieldLabel
                    .SetStyleMinWidth(40)
                    .SetStyleWidth(40)
                    .SetStyleFontSize(11);

            valueLabel =
                GetLabel()
                    .SetStyleColor(EditorColors.Default.UnityThemeInversed)
                    .SetStyleTextAlign(TextAnchor.LowerCenter)
                    .SetStyleFontSize(13);

            lowValueLabel = GetLabel().SetStyleTextAlign(TextAnchor.UpperLeft);
            highValueLabel = GetLabel().SetStyleTextAlign(TextAnchor.UpperRight);

            labelsContainer
                .AddChild(lowValueLabel)
                .AddChild(DesignUtils.flexibleSpace)
                .AddChild(valueLabel)
                .AddChild(DesignUtils.flexibleSpace)
                .AddChild(highValueLabel);

            sliderContainer
                .SetStyleHeight(20)
                .AddChild(snapIntervalIndicatorsContainer)
                .AddChild(snapValuesIndicatorsContainer)
                .AddChild(slider);
        }

        private void Compose()
        {
            this
                .AddChild(sliderContainer)
                .AddChild(labelsContainer);
        }
    }
}