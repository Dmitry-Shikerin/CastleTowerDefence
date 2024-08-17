using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Objects;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects
{
    public class PoolableObject : View, IPoolableObject
    {
        private IObjectPool _pool;
        private CancellationTokenSource _token;
        private TimeSpan _timeSpan;

        private void Awake() =>
            _token = new CancellationTokenSource();

        private void OnDestroy()
        {
            _pool.PoolableObjectDestroyed(this);
            _token.Cancel();
        }

        public void SetPool(IObjectPool pool)
        {
            _pool = pool;
            _timeSpan = TimeSpan.FromSeconds(pool.DeleteAfterTime);
        }

        public void ReturnToPool() =>
            _pool.Return(this);

        public async void StartTimer()
        {
            _token = new CancellationTokenSource();
            
            try
            {
                Debug.Log($"StartTimer: {name}");
                await UniTask.Delay(_timeSpan, cancellationToken: _token.Token);
                _pool.RemoveFromPool(this);
                Debug.Log($"EndTimer: {name}");
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Cancel()
        {
            Debug.Log($"Cancel: {name}");
            _token.Cancel();
        }
    }
}