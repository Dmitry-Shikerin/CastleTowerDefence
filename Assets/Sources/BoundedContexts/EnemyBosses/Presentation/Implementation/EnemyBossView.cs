using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Presentation.Implementation
{
    public class EnemyBossView : EnemyViewBase, IEnemyBossView
    {
        [Required] [SerializeField] private EnemyBossAnimation _enemyBossAnimation;
        [Required] [SerializeField] private ParticleSystem _massAttackParticle;
        [SerializeField] private float _findRange;
        
        public IEnemyBossAnimation Animation => _enemyBossAnimation;
        public float FindRange => _findRange;

        public void PlayMassAttackParticle() =>
            _massAttackParticle.Play();
    }
}