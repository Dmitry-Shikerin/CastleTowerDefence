using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation
{
    public class EnemyKamikazeView : EnemyViewBase, IEnemyKamikazeView
    {
        [Required] [SerializeField] private EnemyAnimation _animation;
        [Range(1, 5)]
        [Required] [SerializeField] private float _findRange;

        public IEnemyAnimation Animation => _animation;
        public float FindRange => _findRange;
    }
}