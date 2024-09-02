using Sirenix.OdinInspector.Editor;
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

        protected override object GetTarget()
        {
            return RuntimeCameraService.Instance;
        }
    }
}