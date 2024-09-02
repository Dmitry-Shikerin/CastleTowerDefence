using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using UnityEditor;
using UnityEngine;

namespace Sources.Frameworks.Editor.Tool
{
    public class Tools
    {
        [MenuItem("Tools/Clear prefs")]
        public static void ClearPrefs()
        {
            foreach (string id in ModelId.ModelData.Keys)
            {
                Debug.Log($"Deleted {id}");
                PlayerPrefs.DeleteKey(id);
                ES3.DeleteKey(id);
            }

            PlayerPrefs.Save();
        }
    }
}