using System;
using System.Linq;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.UniTaskTweens;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeIdleState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private IOverlapService _overlapService;
        private UTSequence _sequence;

        [Construct]
        private void Construct(CharacterRangeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = view.RangeAnimation ?? throw new ArgumentNullException(nameof(view.RangeAnimation));
        }

        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));

        protected override void OnInit()
        {
            _sequence = UTTween
                .Sequence(LoopType.Loop)
                .AddDelayFromSeconds(0.5f)
                .Add(FindTarget);
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _sequence.StartAsync();
        }

        protected override void OnUpdate() =>
            _view.SetLookRotation(0);

        protected override void OnExit() =>
            _sequence.Stop();

        private void FindTarget()
        {
            IEnemyHealthView enemyHealthView = 
                _overlapService.OverlapSphere<EnemyHealthView>(
                        _view.Position, _view.FindRange, 
                        LayerConst.Enemy, 
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if(enemyHealthView?.CurrentHealth <= 0)
                return;
            
            if (enemyHealthView != null)
                _view.SetEnemyHealth(enemyHealthView);
        }
    }
}