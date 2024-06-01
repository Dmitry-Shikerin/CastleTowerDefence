using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.BoundedContexts.NavMeshAgents.Presentation;
using Sources.BoundedContexts.Skins.Presentation;
using Sources.BoundedContexts.Skins.PresentationInterfaces;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyViewBase : NavMeshAgentBase, IEnemyViewBase
    {
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        [Required] [SerializeField] private EnemyHealthView _healthView;
        [SerializeField] private List<SkinView> _skins;
        [Required] [SerializeField] private HealthBarView _healthBarView;

        private readonly IPODestroyerService _poDestroyerService = 
            new PODestroyerService();
        
        public HealthBarView HealthBarView => _healthBarView;
        public FSMOwner FsmOwner => _fsmOwner;
        public EnemyHealthView EnemyHealthView => _healthView;
        public IReadOnlyList<ISkinView> Skins => _skins;
        public ICharacterHealthView CharacterHealthView { get; private set; }
        public ITargetPoint TargetPoint { get; private set; }

        public override void Destroy() =>
            _poDestroyerService.Destroy(this);

        public void SetTargetPoint(ITargetPoint targetPointView) =>
            TargetPoint = targetPointView ?? throw new ArgumentNullException(nameof(targetPointView));

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