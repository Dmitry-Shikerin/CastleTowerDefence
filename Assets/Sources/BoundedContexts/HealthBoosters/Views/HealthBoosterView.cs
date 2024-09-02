using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.HealthBoosters.Views
{
    public class HealthBoosterView : View
    {
        [Required] [SerializeField] private UIButton _button;
        [Required] [SerializeField] private TextView _textView;
        
        private HealthBooster _healthBooster;
        private Bunker _bunker;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;
            
            OnCountChanged();
            _button.onClickEvent.AddListener(OnClick);
            _healthBooster.CountChanged += OnCountChanged;
        }

        private void OnDisable()
        {
            if (_isInitialized == false)
                return;
            
            _button.onClickEvent.RemoveListener(OnClick);
            _healthBooster.CountChanged -= OnCountChanged;
        }

        public void Construct(IEntityRepository entityRepository)
        {
            Hide();
            _healthBooster = entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            _bunker = entityRepository.Get<Bunker>(ModelId.Bunker);
            OnCountChanged();
            _isInitialized = true;
            Show();
        }

        private void OnCountChanged() =>
            _textView.SetText(_healthBooster.Amount.ToString());

        private void OnClick()
        {
            if (_healthBooster.TryApply() == false)
                return;
            
            _bunker.ApplyBoost();
        }
    }
}