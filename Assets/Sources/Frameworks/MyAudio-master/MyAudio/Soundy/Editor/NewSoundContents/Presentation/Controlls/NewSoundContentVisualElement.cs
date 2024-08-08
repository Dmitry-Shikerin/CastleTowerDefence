using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.NewSoundContents.Presentation.Controlls
{
    public class NewSoundContentVisualElement : VisualElement
    {
        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public FluidButton CreateButton { get; private set; }
        public FluidButton SortDescendingButton { get; private set; }
        public FluidButton SortAscendingButton { get; private set; }
        public FluidButton CleanButton { get; private set; }

        public NewSoundContentVisualElement()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout()
                    .SetStyleMaxHeight(40)
                    .SetStylePadding(4,4,4,4)
                    .SetStyleBorderRadius(4,4,0,0)
                    .SetStyleBackgroundColor(EditorColors.Default.BoxBackground);
            SortDescendingButton = new FluidButton()
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.SortZa);
            SortAscendingButton = new FluidButton()
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.SortAz);
            CleanButton = new FluidButton()
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Clear);
            VisualElement spase = DesignUtils.row
                .SetStyleMinWidth(100);
            SoundGroupTextField = new TextField();
            SoundGroupTextField
                .ResetLayout()
                .SetStyleFlexGrow(1)
                .SetStyleMaxHeight(40)
                .SetStyleBorderRadius(15,15,15,15);
            CreateButton = new FluidButton()
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Normal)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Plus)
                .SetAccentColor(EditorSelectableColors.Default.Add)
                .SetLabelText("Add New");

            Container
                .AddChild(SortDescendingButton)
                .AddChild(SortAscendingButton)
                .AddChild(CleanButton)
                .AddChild(spase)
                .AddChild(SoundGroupTextField)
                .AddSpace(4)
                .AddChild(CreateButton)
                ;

            Add(Container);
        }
    }
}