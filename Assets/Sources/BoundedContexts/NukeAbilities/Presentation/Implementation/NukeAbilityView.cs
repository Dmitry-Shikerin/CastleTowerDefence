using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.NukeAbilities.Controllers;
using Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Implementation
{
    public class NukeAbilityView : PresentableView<NukeAbilityPresenter>, INukeAbilityView
    {
        //при нажатии кнопки появляется вьюшка бомбы, бомба двигается сверху вниз, при попадании в земелю бомба исчезает и вклюячается паркл ядерного удара
        //потом мы делаем бокс каст, находим enemyhealth и наносим большой урон им
        [Required] [SerializeField] private ParticleSystem _nukeAbilityEffect;
        [Required] [SerializeField] private BombView _bombView;
        [Required] [SerializeField] private UIButton _nukeAbilityButton;
        [Required] [SerializeField] private BoxCollider _damageCollider;

        public Vector3 DamageSize => _damageCollider.size;
        public UIButton NukeButton => _nukeAbilityButton;
        public IBombView BombView => _bombView;
        
        public void PlayNukeParticle()
        {
            _nukeAbilityEffect.Play();
        }
    }
}