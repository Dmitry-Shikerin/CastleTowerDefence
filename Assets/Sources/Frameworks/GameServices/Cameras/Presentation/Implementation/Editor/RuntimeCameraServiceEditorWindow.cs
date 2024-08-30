using Sirenix.OdinInspector.Editor;
using Sources.Frameworks.UiFramework.Texts.Presentations.Views.Editor;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Implementation.Editor
{
    public class RuntimeCameraServiceEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/CameraService")]
        private static void OpenWindow()
        {
            GetWindow(typeof(RuntimeCameraServiceEditorWindow)).Show();
        }
        
        // protected override OdinMenuTree BuildMenuTree()
        // {
        //     OdinMenuTree tree = new OdinMenuTree();
        //     tree.Selection.SupportsMultiSelect = false;
        //
        //     tree.Add("Database", LocalizationDataBase.Instance);
        //     
        //     foreach (LocalizationPhrase phrase in LocalizationDataBase.Instance.Phrases)
        //         tree.Add($"Phrases/{phrase.LocalizationId}", phrase);
        //     
        //     return tree;
        // }

        protected override object GetTarget()
        {
            return RuntimeCameraService.Instance;
        }
    }
}