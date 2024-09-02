using Sirenix.OdinInspector;
using Sources.BoundedContexts.HealthBoosters.Domain;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.HealthBoosters.Views
{
    public class MainMenuHealthBoosterView : View
    {
        [Required] [SerializeField] private TextView _textView;
        
        private HealthBooster _healthBooster;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;
            
            OnCountChanged();
            _healthBooster.CountChanged += OnCountChanged;
        }

        private void OnDisable()
        {
            if (_isInitialized == false)
                return;
            
            _healthBooster.CountChanged -= OnCountChanged;
        }

        public void Construct(IEntityRepository entityRepository)
        {
            Hide();
            _healthBooster = entityRepository.Get<HealthBooster>(ModelId.HealthBooster);
            OnCountChanged();
            _isInitialized = true;
            Show();
        }

        private void OnCountChanged() =>
            _textView.SetText(_healthBooster.Amount.ToString());
    }
}