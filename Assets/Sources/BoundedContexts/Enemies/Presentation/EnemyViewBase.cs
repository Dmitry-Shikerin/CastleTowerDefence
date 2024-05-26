using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Characters.Controllers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.NavMeshAgents.Presentation;
using Sources.BoundedContexts.Skins.Presentation;
using Sources.BoundedContexts.Skins.PresentationInterfaces;
using Sources.Frameworks.Services.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.Services.ObjectPools.Interfaces.Destroyers;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyViewBase : NavMeshAgentBase, IEnemyViewBase
    {
        [Required] [SerializeField] private EnemyHealthView _healthView;
        [ChildGameObjectsOnly]
        [SerializeField] private List<SkinView> _skins; 

        private readonly IPODestroyerService _poDestroyerService = 
            new PODestroyerService();
        
        public EnemyHealthView EnemyHealthView => _healthView;
        public IReadOnlyList<ISkinView> Skins => _skins;
        public ICharacterHealthView CharacterHealthView { get; private set; }

        public override void Destroy()
        {
            _poDestroyerService.Destroy(this);
        }
        
        public void SetCharacterHealth(ICharacterHealthView characterHealthView) =>
            CharacterHealthView = characterHealthView;

        public void EnableNavmeshAgent() =>
            NavMeshAgent.enabled = true;

        public void DisableNavmeshAgent() =>
            NavMeshAgent.enabled = false;

        [Button]
        private void AddAllSkins()
        {
            _skins.Clear();
            _skins = GetComponentsInChildren<SkinView>(true).ToList();
        }
    }
}