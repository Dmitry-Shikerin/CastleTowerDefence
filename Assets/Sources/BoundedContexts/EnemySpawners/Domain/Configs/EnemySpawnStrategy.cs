using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    [CreateAssetMenu(fileName = "EnemySpawnStrategy", menuName = "Configs/EnemySpawnStrategy", order = 51)]
    public class EnemySpawnStrategy : SerializedScriptableObject
    {
        [TableMatrix(DrawElementMethod = "Draw", ResizableColumns = false, RowHeight = 40)] 
        [SerializeField] private bool[,] _spawnPoints;
        
        private static bool Draw(Rect rect, bool value)
        {
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
        }

        [OnInspectorInit]
        private void CreateData()
        {
            if(_spawnPoints != null)
                return;
            
            _spawnPoints = new bool[4, 4];
            
            Debug.Log($"CreateData");
            
            for (int i = 0; i < _spawnPoints.GetLength(0); i++)
            {
                for (int j = 0; j < _spawnPoints.GetLength(1); j++)
                {
                    _spawnPoints[i, j] = false;
                }
            }
        }
    }
}