﻿using System.Linq;
using Sources.BoundedContexts.Ids;
using Sources.BoundedContexts.Ids.Domain.Constant;
using UnityEditor;
using UnityEngine;

namespace Sources.Editor.Tool
{
    public class Tools
    {
        [MenuItem("Tools/Clear prefs")]
        public static void ClearPrefs()
        {
            foreach (string id in ModelId.ModelsIds.Concat(ModelId.AchievementModels))
            {
                Debug.Log($"Deleted {id}");
                PlayerPrefs.DeleteKey(id);
                ES3.DeleteKey(id);
            }

            PlayerPrefs.Save();
        }
    }
}