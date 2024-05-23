using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Controllers.Common;
using Sources.Frameworks.UiFramework.Infrastructure.Services.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons;
using Sources.Frameworks.UiFramework.Presentation.Buttons.Types;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.Controllers.Buttons
{
    public class UiButtonPresenter : PresenterBase
    {
        private readonly IUiButtonService _uiButtonService;
        private readonly IUiButton _view;

        private CancellationTokenSource _cancellationTokenSource;

        public UiButtonPresenter(
            IUiButton view,
            IUiButtonService uiButtonService)
        {
            _uiButtonService = uiButtonService ??
                                   throw new ArgumentNullException(nameof(uiButtonService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _view.AddClickListener(ShowForm);
            _uiButtonService.Handle(_view.EnableCommandId, _view);
        }

        public override void Disable()
        {
            _view.RemoveClickListener(ShowForm);
            _uiButtonService.Handle(_view.DisableCommandId, _view);
            _cancellationTokenSource.Cancel();
        }

        private async void ShowForm()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                if (_view.UseButtonType == UseButtonType.Delayed)
                    await UniTask.Delay(TimeSpan.FromMilliseconds(_view.Delay),
                        cancellationToken: _cancellationTokenSource.Token,
                        ignoreTimeScale: true);

                _uiButtonService.Handle(_view.OnClickCommandId, _view);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}