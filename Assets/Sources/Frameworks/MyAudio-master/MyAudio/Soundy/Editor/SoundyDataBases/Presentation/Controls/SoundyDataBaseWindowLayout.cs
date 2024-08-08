using System.Collections.Generic;
using Doozy.Editor.EditorUI;
using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.ScriptableObjects.Colors;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundyDataBases.Presentation.Controls
{
    public class SoundyDataBaseWindowLayout : FluidWindowLayout
    {
        public override string layoutName => "Sound Databases";
        public override List<Texture2D> animatedIconTextures => EditorSpriteSheets.EditorUI.Components.LineMixedValues;
        public override Color accentColor => EditorColors.Default.UIComponent;
        public override EditorSelectableColorInfo selectableAccentColor => 
            EditorSelectableColors.Default.UIComponent;
        public FluidButton SettingsButton { get; private set; }
        public FluidButton RefreshButton { get; private set; }
        public FluidButton NewDataBaseButton { get; private set; }

        public SoundyDataBaseWindowLayout()
        {
            AddHeader("Soundy Database", "Sound Groups", animatedIconTextures);
            sideMenu.RemoveSearch();
            
            SettingsButton = new FluidButton()
                .SetElementSize(ElementSize.Normal)
                .SetLabelText("Settings")
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Settings)
                // .SetStyleBackgroundColor(EditorColors.Default.BoxBackground)
                .SetAccentColor(EditorSelectableColors.EditorUI.Orange)
                .SetStyleBorderRadius(4,4,4,4);
            
            RefreshButton = new FluidButton()
                .SetElementSize(ElementSize.Normal)
                .SetLabelText("Refresh")
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Refresh)
                // .SetStyleBackgroundColor(EditorColors.Default.BoxBackground)
                .SetAccentColor(EditorSelectableColors.EditorUI.Orange)
                .SetStyleBorderRadius(4,4,4,4);
            
            NewDataBaseButton = new FluidButton()
                .SetElementSize(ElementSize.Normal)
                .SetLabelText("New Database")
                .SetIcon(EditorSpriteSheets.EditorUI.Icons.Plus)
                .SetAccentColor(EditorSelectableColors.Default.Add)
                // .SetStyleBackgroundColor(EditorColors.Default.BoxBackground)
                .SetStyleBorderRadius(4,4,4,4);

            sideMenu.buttonsScrollViewContainer
                .AddSpace(5)
                .AddChild(SettingsButton)
                .AddSpace(5)
                .AddChild(RefreshButton)
                .AddSpace(4)
                .AddChild(NewDataBaseButton)
                .AddSpace(20);
        }
    }
}