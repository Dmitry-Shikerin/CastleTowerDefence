using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sources.Frameworks.GameServices.Prefabs.Implementation;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Domain;
using Sources.Frameworks.MyGameCreator.SkyAndWeathers.Presentation;
using Sources.Frameworks.MyGameCreator.SkyAndWeawers.Domain;
using TeoGames.Mesh_Combiner.Scripts.Extension;
using UnityEngine;
using OperationCanceledException = System.OperationCanceledException;

namespace Sources.Frameworks.MyGameCreator.SkyAndWeathers.Infrastructure.Services.Implementation
{
    public class SkyAndWeatherService : ISkyAndWeatherService
    {
        private readonly SkyAndWeatherView _skyAndWeatherView;
        private readonly IAssetCollector _assetCollector;
        private SkyAndWeatherCollector _skyWeatherCollector;
        private CancellationTokenSource _token;

        public SkyAndWeatherService(
            SkyAndWeatherView skyAndWeatherView,
            IAssetCollector assetCollector)
        {
            _skyAndWeatherView = skyAndWeatherView ?? throw new ArgumentNullException(nameof(skyAndWeatherView));
            _assetCollector = assetCollector ?? throw new ArgumentNullException(nameof(assetCollector));
        }

        private float Duration => _skyWeatherCollector.DayTime / 1000;

        public void Initialize()
        {
            _skyWeatherCollector = _assetCollector.Get<SkyAndWeatherCollector>();
            _token = new CancellationTokenSource();
            Start();
        }

        private async void Start()
        {
            SkyAndWeatherConfig day = _skyWeatherCollector.Configs.First(config => config.Id == "Day");
            SkyAndWeatherConfig night = _skyWeatherCollector.Configs.First(config => config.Id == "Night");

            try
            {
                while (_token.IsCancellationRequested == false)
                {
                    await ChangePartColor(day, _token.Token, _skyWeatherCollector.IsDay);
                    await ChangePartColor(night, _token.Token, _skyWeatherCollector.IsNight);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask ChangePartColor(
            SkyAndWeatherConfig skyAndWeatherConfig, 
            CancellationToken token,
            bool isUse)
        {
            if (isUse == false)
            {
                await UniTask.Yield(token);
                return;
            }
            
            await ChangeColor(skyAndWeatherConfig.UpperColor, token);
            await ChangeColor(skyAndWeatherConfig.MiddleColor, token);
            await ChangeColor(skyAndWeatherConfig.LowerColor, token);
        }

        private async UniTask ChangeColor(Color to, CancellationToken cancellationToken)
        {
            while (_skyAndWeatherView.Light.color != to)
            {
                _skyAndWeatherView.Light.color = Vector4.MoveTowards(
                    _skyAndWeatherView.Light.color, to, Duration * Time.deltaTime);
                
                await UniTask.Yield(cancellationToken);
            }
        }

        public void Destroy()
        {
            _token.Cancel();
        }
    }
}