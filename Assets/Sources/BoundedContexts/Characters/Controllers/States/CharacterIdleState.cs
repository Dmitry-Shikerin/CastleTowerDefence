using System;
using System.Linq;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.UniTaskTweens;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterIdleState : FSMState
    {
        private ICharacterView _view;
        private ICharacterAnimation _animation;
        private IOverlapService _overlapService;
        private UTSequence _sequence;
        
        [Construct]
        private void Construct(CharacterView characterMeleeView)
        {
            _view = characterMeleeView ?? throw new ArgumentNullException(nameof(characterMeleeView));
            _animation = characterMeleeView.Animation;
        }
        
        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        
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
                    
            if (enemyHealthView == null)
                return;
        
            _view.SetEnemyHealth(enemyHealthView);
        }
    }
}