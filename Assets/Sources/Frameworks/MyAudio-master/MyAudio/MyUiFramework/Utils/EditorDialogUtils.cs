﻿using UnityEditor;

namespace MyAudios.MyUiFramework.Utils
{
    public static class EditorDialogUtils
    {
        public static void ShowErrorDialog(string title, string message)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(title, message, "Ok");
#endif
        }
    }
}