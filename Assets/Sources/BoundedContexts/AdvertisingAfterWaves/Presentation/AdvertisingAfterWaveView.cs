using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using TMPro;
using UnityEngine;

namespace Sources.BoundedContexts.AdvertisingAfterWaves.Presentation
{
    public class AdvertisingAfterWaveView : View
    {
        [SerializeField] private TMP_Text _timerText;

        public TMP_Text TimerText => _timerText;
    }
}