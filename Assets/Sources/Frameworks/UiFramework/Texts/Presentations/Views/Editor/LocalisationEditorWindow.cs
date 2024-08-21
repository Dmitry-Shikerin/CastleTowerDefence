using Sirenix.OdinInspector.Editor;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Configs;
using Sources.Frameworks.UiFramework.Texts.Services.Localizations.Phrases;
using UnityEditor;

namespace Sources.Frameworks.UiFramework.Texts.Presentations.Views.Editor
{
    public class LocalisationEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Localisation")]
        private static void OpenWindow()
        {
            GetWindow(typeof(LocalisationEditorWindow)).Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;

            tree.Add("Database", LocalizationDataBase.Instance);
            
            foreach (LocalizationPhrase phrase in LocalizationDataBase.Instance.Phrases)
                tree.Add($"Phrases/{phrase.LocalizationId}", phrase);
            
            return tree;
        }
    }
}