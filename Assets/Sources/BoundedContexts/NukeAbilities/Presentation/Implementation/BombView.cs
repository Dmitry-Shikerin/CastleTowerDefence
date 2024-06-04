using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Implementation
{
    public class BombView : View, IBombView
    {
        private float _speed = 0.01f;
        public Vector3 FromPosition { get; }
        public Vector3 ToPosition { get; }

        public void Move()
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(FromPosition, ToPosition, step);
        }
    }
}