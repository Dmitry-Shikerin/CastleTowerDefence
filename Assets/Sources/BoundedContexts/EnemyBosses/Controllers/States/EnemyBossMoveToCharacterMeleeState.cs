using System.Linq;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.UniTaskTweens;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossMoveToCharacterMeleeState : FSMState
    {
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IOverlapService _overlapService;
        private UTSequence _sequence;

        [Construct]
        private void Construct(Enemy enemy, EnemyBossView view)
        {
            _view = view;
            _animation = _view.Animation;
        }

        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService;

        protected override void OnInit()
        {
            _sequence = UTTween
                .Sequence()
                .AddDelayFromSeconds(0.5f)
                .Add(FindTarget)
                .SetLoop();
        }

        protected override void OnEnter()
        {
            _animation.PlayWalk();
            _sequence.StartAsync();
        }

        protected override void OnUpdate() =>
            _view.Move(_view.CharacterMeleePoint.Position);

        protected override void OnExit() =>
            _sequence.Stop();

        private void FindTarget()
        {
            var characterHealthView =
                _overlapService.OverlapSphere<CharacterHealthView>(
                        _view.Position, _view.FindRange,
                        LayerConst.Character,
                        LayerConst.Defaul)
                    .FirstOrDefault();

            if (characterHealthView == null)
                return;

            _view.SetCharacterHealth(characterHealthView);
        }
    }
}