using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Editor.UIElements;
using Doozy.Runtime.UIElements.Extensions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.AudioDatas.Presentation.Controlls
{
    public class AudioDataVisualElement : VisualElement
    {
        public FluidButton PlayButton { get; private set; }
        public FluidButton DeleteButton { get; private set; }
        public FluidRangeSlider Slider { get; private set; }
        public ObjectField ObjectField { get; private set; }

        public AudioDataVisualElement()
        {
            VisualElement container =
                DesignUtils.column
                    .ResetLayout()
                    .SetStyleColor(EditorColors.Default.Background)
                    .SetStyleAlignContent(Align.Center)
                    .SetStyleBorderRadius(4,4,4,4)
                    .SetStylePadding(4,4,4,4)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);

            VisualElement topLine = DesignUtils.row;
            VisualElement botLine = DesignUtils.row;
            Label label = DesignUtils
                .NewLabel()
                .SetText("AudioClip");
            label
                .SetStyleColor(EditorColors.Default.WindowHeaderTitle)
                .SetStyleMinWidth(70);
            
            ObjectField = new ObjectField();
            ObjectField.SetStyleFlexGrow(1);
            ObjectField
                .SetObjectType(typeof(AudioClip));
            ObjectField.SetValueWithoutNotify(null);

            PlayButton =
                FluidButton
                    .Get()
                    .ResetLayout()
                    .SetButtonStyle(ButtonStyle.Contained)
                    .SetIcon(EditorSpriteSheets.EditorUI.Icons.Play)
                    .SetAccentColor(EditorSelectableColors.SceneManagement.Component);
            VisualElement slidersContainer = DesignUtils.column;
            Slider = new FluidRangeSlider().SetStyleMaxHeight(18);
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
                    .SetAccentColor(EditorSelectableColors.Default.Remove);
            topLine
                .AddChild(PlayButton)
                .AddChild(slidersContainer);
            botLine
                .AddChild(label)
                .AddChild(ObjectField)
                .AddChild(new VisualElement().SetStyleMinWidth(7))
                .AddChild(DeleteButton);
            container
                .AddChild(topLine)
                .AddSpaceBlock()
                .AddChild(botLine);

            Add(container);
        }
    }
}