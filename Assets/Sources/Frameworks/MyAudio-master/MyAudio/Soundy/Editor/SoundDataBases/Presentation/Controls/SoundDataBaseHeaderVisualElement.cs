using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundDataBases.Presentation.Controls
{
    public class SoundDataBaseHeaderVisualElement : VisualElement
    {
        public SoundDataBaseHeaderVisualElement()
        {
            Container =
                DesignUtils
                    .row
                    .ResetLayout()
                    .SetStylePadding(4,2,4,2)
                    .SetStyleBorderRadius(4,4,4,4)
                    .SetStyleBackgroundColor(EditorColors.Default.Background);

            Image image = new Image()
                .ResetLayout()
                .SetStyleBackgroundImage(EditorTextures.EditorUI.Icons.Dashboard)
                .SetStyleWidth(20);
            SoundGroupTextField = new TextField();
            //Потом включить
            SoundGroupTextField
                .SetStyleFlexGrow(1);
            SoundGroupTextField
                .SetValueWithoutNotify("New Sound Group");
            RenameButton = new FluidButton();
            RenameButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Save)
                .SetLabelText("Rename")
                .SetAccentColor(EditorSelectableColors.Default.Add)
                .SetElementSize(ElementSize.Small);
            
            PingAssetButton = new FluidButton();
            PingAssetButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Small)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Location);
            
            RemoveButton = new FluidButton();
            RemoveButton
                .SetButtonStyle(ButtonStyle.Contained)
                .SetElementSize(ElementSize.Small)
                .SetAccentColor(EditorSelectableColors.Default.Remove)
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Close)
                .SetLabelText("Remove");

            Container
                .AddSpace(4)
                .AddChild(image)
                .AddSpace(8)
                .AddChild(SoundGroupTextField)
                .AddSpace(4)
                .AddChild(RenameButton)
                .AddSpace(4)
                .AddChild(PingAssetButton)
                .AddSpace(4)
                .AddChild(RemoveButton);
            
            Add(Container);
        }

        public VisualElement Container { get; private set; }
        public TextField SoundGroupTextField { get; private set; }
        public FluidButton RenameButton { get; private set; }
        public FluidButton RemoveButton { get; private set; }
        public FluidButton PingAssetButton { get; private set; }
    }
}