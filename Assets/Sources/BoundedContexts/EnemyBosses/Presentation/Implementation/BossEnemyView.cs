using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Implementation
{
    public class BossEnemyView : EnemyViewBase, IBossEnemyView
    {
        [Required] [SerializeField] private BossEnemyAnimation bossEnemyAnimation;
        [Required] [SerializeField] private ParticleSystem _massAttackParticle;
        [Required] [SerializeField] private EnemyBossDependencyProvider _provider;
        // [Required] [SerializeField] private FSMOwner _fsmOwner;
        
        public IBossEnemyAnimation Animation => bossEnemyAnimation;
        // public FSMOwner FsmOwner => _fsmOwner;
        public EnemyBossDependencyProvider Provider => _provider;

        public void PlayMassAttackParticle() =>
            _massAttackParticle.Play();

        public void SetAgentSpeed(float speed) =>
            NavMeshAgent.speed = speed;
    }
}