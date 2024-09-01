using Sirenix.OdinInspector;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Triggers.Presentation;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation
{
    public class FlamethrowerView : View, IFlamethrowerView
    {
        [Required] [SerializeField] private Transform _from;
        [Required] [SerializeField] private Transform _to;
        [Range(0.5f, 5)]
        [SerializeField] private float _speed;
        
        public Vector3 FromPosition => _from.position;
        public Vector3 ToPosition => _to.position;
        public Vector3 Position => transform.position;
        
        public void Move(Vector3 targetPosition)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime);
        }
    }
}