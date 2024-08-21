using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyAudios.MyUiFramework.Utils
{
    public static class MyAssetUtils
    {
        /// <summary>
        /// Returns a reference to a scriptable object of type T with the given fileName at the relative resourcesPath.
        /// <para/> If the asset is not found, one will get created automatically (in the Editor only) 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resourcesPath"></param>
        /// <param name="saveAssetDatabase"></param>
        /// <param name="refreshAssetDatabase"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetScriptableObject<T>(
            string fileName, string resourcesPath, bool saveAssetDatabase, bool refreshAssetDatabase)
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(resourcesPath))
                return null;
            
            if (string.IsNullOrEmpty(fileName))
                return null;
            
            // ReSharper disable once SuspiciousTypeConversion.Global
            // if (resourcesPath[resourcesPath.Length - 1].Equals(@"\") == false)
            //     resourcesPath += @"\";
            
            resourcesPath = resourcesPath.Replace(@"\", "/");
            resourcesPath = CleanPath(resourcesPath);

            T obj = (T) Resources.Load(fileName, typeof(T));

            if (obj == null)
            {
                string simpleResourcesPath = resourcesPath.Replace(
                    resourcesPath.Substring(0, resourcesPath.LastIndexOf(
                        "Resources", StringComparison.Ordinal)), "");
                simpleResourcesPath = simpleResourcesPath.Replace(
                    "Resources", "").Remove(0, 1);
                obj = (T) Resources.Load(Path.Combine(simpleResourcesPath, fileName), typeof(T));
            }

#if UNITY_EDITOR
            if (obj != null)
                return obj;
            
            obj = CreateAsset<T>(resourcesPath, fileName, ".asset", saveAssetDatabase, refreshAssetDatabase);
#endif
            return obj;
        }

        public static T GetScriptableObject<T>(
            string fileName, string resourcesPath)
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(resourcesPath))
                return null;
            
            if (string.IsNullOrEmpty(fileName))
                return null;

            // T asset = AssetDatabase.LoadAssetAtPath<T>($"{resourcesPath}/{fileName}");
            T asset = (T)Resources.Load($"{resourcesPath}/{fileName}", typeof(T));
            
#if UNITY_EDITOR
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, resourcesPath + "/" + fileName + ".asset");
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            
            return asset;
        }

        public static T GetResource<T>(string resourcesPath, string fileName)
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(resourcesPath))
                return null;
            
            if (string.IsNullOrEmpty(fileName)) 
                return null;
            
            resourcesPath = CleanPath(resourcesPath);
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!resourcesPath[resourcesPath.Length - 1].Equals(@"\")) resourcesPath += @"\";
            resourcesPath = resourcesPath.Replace(@"\", "/");

            return (T) Resources.Load(resourcesPath + fileName, typeof(T));
        }

        public static string CleanPath(string path)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!path[path.Length - 1].Equals(@"\"))
                path += @"\";
            
            path = path.Replace(@"\\", @"\");
            path = path.Replace(@"\", "/");
            
            return path;
        }

#if UNITY_EDITOR
        public static T CreateAsset<T>(
            string relativePath, 
            string fileName, 
            string extension = ".asset", 
            bool saveAssetDatabase = true, 
            bool refreshAssetDatabase = true)
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;
            
            if (string.IsNullOrEmpty(fileName))
                return null;
            
            relativePath = CleanPath(relativePath);
            // ReSharper disable once SuspiciousTypeConversion.Global
            // if (!relativePath[relativePath.Length - 1].Equals(@"\"))
            //     relativePath += @"\";
            
            // relativePath = relativePath.Replace(@"\\", @"\");
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, relativePath + fileName + extension);
            EditorUtility.SetDirty(asset);
            
            if (saveAssetDatabase)
                AssetDatabase.SaveAssets();
            
            if (refreshAssetDatabase)
                AssetDatabase.Refresh();
            
            return asset;
        }

        public static List<T> GetAssets<T>() where T : ScriptableObject
        {
            List<T> list = new List<T>();
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            
            foreach (string guid in guids)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset == null) continue;
                list.Add(asset);
            }

            return list;
        }
        
        public static void MoveAssetToTrash(
            string relativePath, 
            string fileName, 
            bool saveAssetDatabase = true,
            bool refreshAssetDatabase = true, 
            bool printDebugMessage = true)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;
            
            if (string.IsNullOrEmpty(fileName))
                return;
            
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!relativePath[relativePath.Length - 1].Equals(@"\"))
                relativePath += @"\";
            
            relativePath = CleanPath(relativePath);
            
            if (AssetDatabase.MoveAssetToTrash(relativePath + fileName + ".asset") == false)
                return;
            
            if (saveAssetDatabase)
                AssetDatabase.SaveAssets();
            
            if (refreshAssetDatabase)
                AssetDatabase.Refresh();
        }
#endif
    }
}