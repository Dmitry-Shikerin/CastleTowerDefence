using Sirenix.OdinInspector;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Presentation
{
    public class EnemyView : EnemyViewBase, IEnemyView
    {
        [Range(1, 3)]
        [Required] [SerializeField] private float _findRange = 3;
        [Required] [SerializeField] private EnemyAnimation _animation;

        public IEnemyAnimation Animation => _animation;
        public float FindRange => _findRange;
    }
}