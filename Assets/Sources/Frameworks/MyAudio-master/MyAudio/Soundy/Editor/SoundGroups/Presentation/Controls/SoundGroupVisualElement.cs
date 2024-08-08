using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundGroups.Presentation.Controls
{
    public class SoundGroupVisualElement : VisualElement
    {
        public FluidButton PlayButton { get; private set; }
        public FluidButton SoundGroupDataButton { get; private set; }
        public FluidButton DeleteButton { get; private set; }
        public FluidRangeSlider Slider { get; private set; }

        public SoundGroupVisualElement()
        {
            VisualElement container =
                DesignUtils.row
                    .ResetLayout()
                    // .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBackgroundColor(EditorColors.Default.BoxBackground)
                    .SetStylePadding(8,8,8,8)
                    .SetStyleBorderRadius(4,4,4,4);
            SoundGroupDataButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Sound)
                    .SetStyleMaxWidth(130)
                    .SetStyleMinWidth(130)
                    .SetElementSize(ElementSize.Large);
            PlayButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Play)
                    .SetElementSize(ElementSize.Large)
                    .SetAccentColor(EditorSelectableColors.Default.AudioComponent);                            
            VisualElement slidersContainer = DesignUtils.column;
            Slider = new FluidRangeSlider()
                .SetStyleMaxHeight(18);
            Slider
                .slider
                .SetStyleBorderColor(EditorColors.EditorUI.Orange)
                .SetStyleColor(EditorColors.EditorUI.Orange);
            
            slidersContainer
                .AddChild(Slider);
            DeleteButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Minus)
                    .SetElementSize(ElementSize.Large)
                    .SetAccentColor(EditorSelectableColors.Brands.YouTube);
            // DeleteButton
            //     .buttonIcon
            //     .SetStyleBackgroundImageTintColor(EditorColors.EditorUI.Orange);

            container
                .AddChild(SoundGroupDataButton)
                .AddSpace(4)
                .AddChild(PlayButton)
                .AddSpace(4)
                .AddChild(slidersContainer)
                .AddSpace(4)
                .AddChild(DeleteButton);

            Add(container);
        }
    }
}