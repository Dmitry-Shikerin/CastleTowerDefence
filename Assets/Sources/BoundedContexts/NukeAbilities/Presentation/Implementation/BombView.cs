using Sirenix.OdinInspector;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Implementation
{
    public class BombView : View, IBombView
    {
        [Required] [SerializeField] private Transform _from;
        [Required] [SerializeField] private Transform _to;
        [SerializeField] private float _speed;
        
        public Vector3 FromPosition => _from.position;
        public Vector3 ToPosition => _to.position;
        public Vector3 Position => transform.position;

        public void Move()
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ToPosition, step);
        }
    }
}