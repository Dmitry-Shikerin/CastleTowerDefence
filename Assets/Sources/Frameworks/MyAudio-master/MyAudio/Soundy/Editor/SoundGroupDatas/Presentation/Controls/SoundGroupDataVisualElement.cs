using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using MyAudios.Soundy.Editor.MinMaxSliders;
using MyAudios.Soundy.Editor.NewSoundContents.Presentation.Controlls;
using MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroupDatas.Presentation.Controlls
{
    public class SoundGroupDataVisualElement : VisualElement
    {
        public SoundGroupDataVisualElement()
        {
            Root = DesignUtils.column;
            
            Header =
                FluidComponentHeader
                    .Get()
                    .SetComponentNameText("Sound Group")
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetAccentColor(EditorColors.EditorUI.Orange);
            
            HeaderVisualElement = new SoundDataBaseHeaderVisualElement();
            
            //PlayMode
            FluidToggleGroup playModeToggleGroup = new FluidToggleGroup();
            playModeToggleGroup.iconContainer.image = EditorTextures.UIManager.Icons.UIToggleGroup;
            playModeToggleGroup
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetLabelText("Play Mode");
            RandomButtonTab = new FluidToggleButtonTab();
            RandomButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Random");
            RandomButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(RandomButtonTab);
            SequenceButtonTab = new FluidToggleButtonTab();
            SequenceButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Sequence");
            SequenceButtonTab.AddToToggleGroup(playModeToggleGroup);
            playModeToggleGroup.RegisterToggle(SequenceButtonTab);

            //ResetSequenceAfterInactiveTime
            Label resetSequenceAfterInactiveTimeLabel =
                DesignUtils.NewLabel("Auto Reset Sequence After");
            ResetSequenceAfterInactiveTimeToggle =
                new FluidToggleSwitch();
            VisualElement resetSequenceAfterInactiveTimeRow = DesignUtils
                .row
                .AddChild(ResetSequenceAfterInactiveTimeToggle)
                .AddChild(resetSequenceAfterInactiveTimeLabel)
                .SetStyleFlexGrow(0)
                .SetStyleMarginRight(5);

            Label sequenceResetTimeLabel =
                DesignUtils.NewLabel("seconds");
            VisualElement sequenceResetTimeRow = DesignUtils
                .row
                .AddChild(sequenceResetTimeLabel);
            VisualElement sequenceTimeRow = DesignUtils.row
                .ResetLayout()
                .AddChild(resetSequenceAfterInactiveTimeRow)
                .AddChild(sequenceResetTimeRow);
            VisualElement playModeRow = DesignUtils
                .column
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStyleBorderRadius(4,4,4,4)
                .AddChild(playModeToggleGroup)
                .AddSpaceBlock(2)
                .AddChild(sequenceTimeRow);

            VisualElement toggleGroupRow = DesignUtils.row
                .AddChild(RandomButtonTab)
                .AddChild(SequenceButtonTab);
            playModeToggleGroup
                .AddSpaceBlock()
                .AddChild(toggleGroupRow);

            //Loop
            Label loopLabel = DesignUtils.NewLabel("Loop");
            LoopToggle =
                new FluidToggleSwitch();
            VisualElement loopRow = DesignUtils
                .row
                .AddChild(loopLabel)
                .AddChild(LoopToggle)
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStyleBorderRadius(4,4,4,4)
                .SetStylePadding(4,4,4,4);

            //Sliders
            Label volumeLabel =
                DesignUtils
                    .NewLabel("Volume")
                    .SetStyleMinWidth(70);
            VolumeSlider = new FluidMinMaxSlider();

            ResetVolumeButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset);

            VisualElement volumeRow = DesignUtils.row
                .AddChild(volumeLabel)
                .AddChild(VolumeSlider)
                .AddChild(ResetVolumeButton)
                .SetStyleBorderRadius(0,0,4,4);

            Label pitchLabel =
                DesignUtils
                    .NewLabel("Pitch")
                    .SetStyleMinWidth(70);
            PitchSlider = new FluidMinMaxSlider();

            ResetPitchButton = FluidButton
                .Get()
                .ResetLayout()
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Reset);

            VisualElement pitchRow = DesignUtils.row
                .AddChild(pitchLabel)
                .AddChild(PitchSlider)
                .AddChild(ResetPitchButton)
                .SetStyleBorderRadius(0,0,4,4);

            Label spatialBlendLabel =
                DesignUtils
                    .NewLabel("SpatialBlend")
                    .SetStyleMinWidth(70);
            SpatialBlendSlider = new FluidRangeSlider();
            SpatialBlendSlider.SetStyleFlexGrow(1);
            SpatialBlendSlider.RegisterCallback<DragUpdatedEvent>((even) => { });

            VisualElement spatialBlendRow = DesignUtils.row
                .AddChild(spatialBlendLabel)
                .AddChild(SpatialBlendSlider)
                .SetStyleBorderRadius(0,0,4,4);

            FluidAnimatedContainer slidersContainer =
                new FluidAnimatedContainer()
                    .SetStylePadding(4,4,4,4);

            //SlidersToggles
            FluidToggleGroup slidersToggleGroup = new FluidToggleGroup();
            slidersToggleGroup.iconContainer.image = EditorTextures.UIManager.Icons.UIToggleGroup;
            slidersToggleGroup
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetLabelText("Sliders")
                .SetStyleBackgroundColor(EditorColors.Default.Background)
                .SetStyleBorderRadius(4,4,0,0);

            FluidToggleButtonTab volumeButtonTab = new FluidToggleButtonTab();
            FluidToggleButtonTab pitchButtonTab = new FluidToggleButtonTab();
            FluidToggleButtonTab spatialBlendButtonTab = new FluidToggleButtonTab();

            volumeButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Volume")
                .SetOnClick(() =>
                {
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    volumeButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);

                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(volumeRow)
                        .Show();
                });
            volumeButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(volumeButtonTab);

            pitchButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Pitch")
                .SetOnClick(() =>
                {
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    pitchButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);

                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(pitchRow)
                        .Show();
                });
            pitchButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(pitchButtonTab);

            spatialBlendButtonTab
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetElementSize(ElementSize.Large)
                .SetLabelText("Spatial Blend")
                .SetOnClick(() =>
                {
                    volumeButtonTab.ResetColors();
                    pitchButtonTab.ResetColors();
                    spatialBlendButtonTab.ResetColors();
                    spatialBlendButtonTab.SetToggleAccentColor(EditorSelectableColors.EditorUI.Orange);

                    slidersContainer.ClearContent();
                    slidersContainer
                        .AddContent(spatialBlendRow)
                        .Show();
                });
            spatialBlendButtonTab.AddOnValueChanged((isOn) =>
            {
                    spatialBlendButtonTab.ResetColors();
            });
            spatialBlendButtonTab.AddToToggleGroup(slidersToggleGroup);
            slidersToggleGroup.RegisterToggle(spatialBlendButtonTab);


            VisualElement slidersToggleGroupRow = DesignUtils
                .row
                .AddChild(volumeButtonTab)
                .AddChild(pitchButtonTab)
                .AddChild(spatialBlendButtonTab);
            slidersToggleGroup
                .AddSpaceBlock()
                .AddChild(slidersToggleGroupRow)
                .AddChild(slidersContainer);

            NewSoundContentVisualElement =
                new NewSoundContentVisualElement();

            AudioDataContent =
                new ScrollView()
                    .ResetLayout()
                    .SetStyleFlexGrow(1)
                    .SetStyleFlexShrink(1);
            
            VisualElement topContent =
                    DesignUtils
                        .column
                        .SetStyleMinHeight(300)
                        .AddChild(Header)
                        .AddSpace(10)
                        .AddChild(HeaderVisualElement)
                        .AddSpaceBlock(2)
                        .AddChild(playModeRow)
                        .AddSpaceBlock(2)
                        .AddChild(loopRow)
                        .AddSpaceBlock(2)
                        .AddChild(slidersToggleGroup)
                        .AddSpaceBlock(2)
                        .AddChild(NewSoundContentVisualElement)
                        .AddSpaceBlock(2);
            Root
                .AddChild(topContent)
                .AddSpaceBlock(2)
                .AddChild(AudioDataContent)
                ;

            Add(Root);
        }

        public SoundDataBaseHeaderVisualElement HeaderVisualElement { get; private set; }
        public VisualElement Root { get; private set; }
        public ScrollView AudioDataContent { get; private set; }
        private FluidComponentHeader Header { get; set; }
        public FluidToggleButtonTab RandomButtonTab { get; private set; }
        public FluidToggleButtonTab SequenceButtonTab { get; private set; }
        public FluidToggleSwitch ResetSequenceAfterInactiveTimeToggle { get; private set; }
        public FloatField SequenceResetTimeField { get; private set; }
        public FluidToggleSwitch LoopToggle { get; private set; }
        public FluidMinMaxSlider VolumeSlider { get; private set; }
        public FluidButton ResetVolumeButton { get; private set; }
        public FluidMinMaxSlider PitchSlider { get; private set; }
        public FluidRangeSlider SpatialBlendSlider { get; private set; }
        public FluidButton ResetPitchButton { get; private set; }
        public NewSoundContentVisualElement NewSoundContentVisualElement { get; private set; }
    }
}