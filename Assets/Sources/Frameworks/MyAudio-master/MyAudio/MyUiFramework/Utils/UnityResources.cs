using UnityEditor;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils
{
    public static class UnityResources
    {
#if UNITY_EDITOR

        #region UI built-in sprites

        private const string STANDARD_SPRITE_PATH = "UI/Skin/UISprite.psd";
        private const string BACKGROUND_SPRITE_RESOURCE_PATH = "UI/Skin/Background.psd";
        private const string INPUT_FIELD_BACKGROUND_PATH = "UI/Skin/InputFieldBackground.psd";
        private const string KNOB_PATH = "UI/Skin/Knob.psd";
        private const string CHECKMARK_PATH = "UI/Skin/Checkmark.psd";

        public static Sprite UISprite =>
            AssetDatabase.GetBuiltinExtraResource<Sprite>(STANDARD_SPRITE_PATH);
        public static Sprite Background =>
            AssetDatabase.GetBuiltinExtraResource<Sprite>(BACKGROUND_SPRITE_RESOURCE_PATH);
        public static Sprite FieldBackground =>
            AssetDatabase.GetBuiltinExtraResource<Sprite>(INPUT_FIELD_BACKGROUND_PATH);
        public static Sprite Knob =>
            AssetDatabase.GetBuiltinExtraResource<Sprite>(KNOB_PATH);
        public static Sprite Checkmark =>
            AssetDatabase.GetBuiltinExtraResource<Sprite>(CHECKMARK_PATH);

        #endregion

#endif
    }
}