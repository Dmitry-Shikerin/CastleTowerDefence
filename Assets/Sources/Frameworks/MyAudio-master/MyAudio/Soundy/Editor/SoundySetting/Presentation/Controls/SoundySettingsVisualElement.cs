using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Utils;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine.UIElements;

namespace MyAudios.Soundy.Editor.SoundySetting.Presentation.Controls
{
    public class SoundySettingsVisualElement : VisualElement
    {
        public SoundySettingsVisualElement()
        {
            VisualElement autoKillRow = DesignUtils.row
                .ResetLayout();
            VisualElement autoKillLabel = new Label("Auto Kill")
                .ResetLayout()
                .SetStyleMinWidth(130);
            AutoKillToggle = new FluidToggleSwitch();
            autoKillRow
                .AddChild(autoKillLabel)
                .AddChild(AutoKillToggle);
            KillDurationRow = new SoundySettingsRowVisualElement();
            KillDurationRow.Label
                .SetText("IIdle Kill Duration");
            IdleCheckRow = new SoundySettingsRowVisualElement();
            IdleCheckRow.Label
                .SetText("Idle Check Interval");
            MinControllersRow = new SoundySettingsRowVisualElement();
            MinControllersRow.Label
                .SetText("Min Controllers Count");
            
            this
                .AddChild(autoKillRow)
                .AddChild(KillDurationRow)
                .AddChild(IdleCheckRow)
                .AddChild(MinControllersRow);
        }
        
        public FluidToggleSwitch AutoKillToggle { get; }
        public SoundySettingsRowVisualElement KillDurationRow { get; }
        public SoundySettingsRowVisualElement IdleCheckRow { get; }
        public SoundySettingsRowVisualElement MinControllersRow { get; }
    }
}