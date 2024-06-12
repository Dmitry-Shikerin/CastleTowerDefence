using System.Collections.Generic;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Repositories.Domain.Configs
{
    [CreateAssetMenu(fileName = "EntityDataBase", menuName = "Configs/EntityDataBase", order = 51)]
    public class EntityDataBase : ScriptableObject
    {
        [SerializeField] private List<string> _ids;
        
        public IReadOnlyList<string> Ids => _ids;
    }
}