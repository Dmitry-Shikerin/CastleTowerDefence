using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Controllers
{
    public class NukeAbilityPresenter : PresenterBase
    {
        private readonly NukeAbility _nukeAbility;
        private readonly INukeAbilityView _nukeAbilityView;

        public NukeAbilityPresenter(NukeAbility nukeAbility, INukeAbilityView nukeAbilityView)
        {
            _nukeAbility = nukeAbility ?? throw new ArgumentNullException(nameof(nukeAbility));
            _nukeAbilityView = nukeAbilityView ?? throw new ArgumentNullException(nameof(nukeAbilityView));
        }

        public override void Enable()
        {
            _nukeAbilityView.NukeButton.onClickEvent.AddListener(ApplyAbility);
        }

        public override void Disable()
        {
            _nukeAbilityView.NukeButton.onClickEvent.RemoveListener(ApplyAbility);
        }

        private void ApplyAbility()
        {
            _nukeAbilityView.BombView.Show();
            _nukeAbilityView.BombView.Move();
            
            //while(Vector3.Distance(_nukeAbilityView.BombView))
        }
    }
}