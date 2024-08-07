using MyAudios.Soundy.Editor.Views;
using UnityEngine;

namespace MyAudios.Soundy.Editor.SoundySetting.Presentation.Views.Interfaces
{
    public interface ISoundySettingsView : IView
    {
        void SetAutoKillIdleControllers(bool autoKillIdleControllers);
        void SetControllerAutoKillDuration(Vector2Int minMaxValue, int value);
        void SetIdleCheckInterval(Vector2Int minMaxValue, int value);
        void SetMinimumNumberOfControllers(Vector2Int minMaxValue, int value);
    }
}