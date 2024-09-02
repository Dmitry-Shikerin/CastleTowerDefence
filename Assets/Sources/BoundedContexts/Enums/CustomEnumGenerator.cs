using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.BoundedContexts.Enums
{
    public class CustomEnumGenerator : MonoBehaviour
    {
        [SerializeField] private List<string> _listEnums;
        
        [Button]
        private void GenerateEnum()
        {
        }
    }
}