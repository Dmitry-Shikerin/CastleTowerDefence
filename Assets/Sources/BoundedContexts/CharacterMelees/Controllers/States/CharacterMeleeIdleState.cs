using System;
using System.Linq;
using System.Threading;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.UniTaskTweens;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeIdleState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private IOverlapService _overlapService;
        private UTSequence _sequence;

        [Construct]
        private void Construct(CharacterMeleeView characterMeleeView)
        {
            _view = characterMeleeView ?? throw new ArgumentNullException(nameof(characterMeleeView));
            _animation = characterMeleeView.MeleeAnimation;
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
            _sequence.Start();
        }

        protected override void OnUpdate()
        {
            _view.SetLookRotation(0);
        }

        protected override void OnExit()
        {
            _sequence.Stop();
        }

        private void FindTarget()
        {
            IEnemyHealthView enemyHealthView =
                _overlapService.OverlapSphere<EnemyHealthView>(
                        _view.Position, _view.FindRange,
                        LayerConst.Enemy,
                        LayerConst.Defaul)
                    .FirstOrDefault();
            
            if (enemyHealthView == null)
                return;

            _view.SetEnemyHealth(enemyHealthView);
        }
    }
}