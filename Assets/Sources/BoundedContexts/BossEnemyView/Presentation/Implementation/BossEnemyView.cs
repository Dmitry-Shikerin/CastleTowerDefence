using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.BossEnemyView.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterHealth.PresentationInterfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using UnityEngine;

namespace Sources.BoundedContexts.BossEnemyView.Presentation.Implementation
{
    public class BossEnemyView : EnemyViewBase, IBossEnemyView
    {
        [Required] [SerializeField] private BossEnemyAnimation bossEnemyAnimation;
        [Required] [SerializeField] private ParticleSystem _massAttackParticle;
        [Required] [SerializeField] private BossEnemyDependencyProvider _provider;
        [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public IBossEnemyAnimation Animation => bossEnemyAnimation;
        public FSMOwner FsmOwner => _fsmOwner;
        public BossEnemyDependencyProvider Provider => _provider;

        public void PlayMassAttackParticle() =>
            _massAttackParticle.Play();

        public void SetAgentSpeed(float speed) =>
            NavMeshAgent.speed = speed;
    }
}