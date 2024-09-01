using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.BurnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.Cameras.Presentation;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.BoundedContexts.NavMeshAgents.Presentation;
using Sources.BoundedContexts.NavMeshAgents.Presentation.Implementation;
using Sources.BoundedContexts.Skins.Presentation;
using Sources.BoundedContexts.Skins.PresentationInterfaces;
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
        [Required] [SerializeField] private BurnAbilityView _burnAbilityView;
        [Required] [SerializeField] private LookAtCamera _lookAtCamera;

        private readonly IPODestroyerService _poDestroyerService = 
            new PODestroyerService();

        public LookAtCamera LookAtCamera => _lookAtCamera;
        public FSMOwner FsmOwner => _fsmOwner;
        public BurnAbilityView BurnAbilityView => _burnAbilityView;
        public HealthBarView HealthBarView => _healthBarView;
        public EnemyHealthView EnemyHealthView => _healthView;
        public IReadOnlyList<ISkinView> Skins => _skins;
        public ICharacterHealthView CharacterHealthView { get; private set; }
        public IBunkerView BunkerView { get; private set; }
        public ICharacterSpawnPoint CharacterMeleePoint { get; private set; }

        public override void Destroy()
        {
            _poDestroyerService.Destroy(this);
            StopFsm();
        }

        public void SetBunkerView(IBunkerView bunkerView) =>
            BunkerView = bunkerView ?? throw new ArgumentNullException(nameof(bunkerView));

        public void SetCharacterMeleePoint(ICharacterSpawnPoint characterSpawnPoint) =>
            CharacterMeleePoint = characterSpawnPoint ?? throw new ArgumentNullException(nameof(characterSpawnPoint));

        public void SetCharacterHealth(ICharacterHealthView characterHealthView) =>
            CharacterHealthView = characterHealthView;

        public void EnableNavmeshAgent() =>
            NavMeshAgent.enabled = true;

        public void DisableNavmeshAgent() =>
            NavMeshAgent.enabled = false;

        public void StartFsm() =>
            _fsmOwner.StartBehaviour();

        public void StopFsm() =>
            _fsmOwner.StopBehaviour();

        [Button]
        private void AddAllSkins()
        {
            _skins.Clear();
            _skins = GetComponentsInChildren<SkinView>(true).ToList();
        }
    }
}