using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    public class EnemySpawnStrategy : Config
    {
        [ShowInInspector]
        [TableMatrix(
            HorizontalTitle = "Spawn Points", 
            DrawElementMethod = nameof(Draw), 
            ResizableColumns = false, 
            RowHeight = 40)]
        public bool[,] SpawnPoints;

        private static bool Draw(Rect rect, bool value)
        {
#if UNITY_EDITOR
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
            
            UnityEditor.EditorGUI.DrawRect(
                rect.Padding(1), value 
                    ? new Color(0.1f, 0.8f, 0.2f) 
                    : new Color(0, 0, 0, 0.5f));
            
            return value;
#else
            return false;
#endif
        }

        [OnInspectorInit]
        private void CreateData()
        {
             if(SpawnPoints != null)
                 return;
             
             SpawnPoints = new bool[2, 4];
             
             for (int i = 0; i < SpawnPoints.GetLength(0); i++)
             {
                 for (int j = 0; j < SpawnPoints.GetLength(1); j++)
                 {
                     SpawnPoints[i, j] = false;
                 }
             }
        }

    }
}