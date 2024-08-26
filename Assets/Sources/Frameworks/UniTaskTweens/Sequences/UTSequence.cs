using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;

namespace Sources.Frameworks.UniTaskTweens.Sequences
{
    public class UTSequence
    {
        private CancellationTokenSource _token = new CancellationTokenSource();
        private List<Func<CancellationToken, UniTask>> _tasks = new List<Func<CancellationToken, UniTask>>();

        private LoopType _loopType = LoopType.None;
        private bool _isComplete;
        private bool _isStarted;

        public event Action Completed;
        public event Action Started;

        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;

                if (_isStarted)
                    Started?.Invoke();
            }
        }

        public bool IsComplete
        {
            get => _isComplete;
            set
            {
                _isComplete = value;

                if (_isComplete)
                {
                    Completed?.Invoke();
                    _token.Cancel();
                }
            }
        }

        public UTSequence Add(Func<CancellationToken, UniTask> task)
        {
            _tasks.Add(task);
            return this;
        }

        public UTSequence Add(Action action)
        {
            _tasks.Add(async _ => action.Invoke());
            return this;
        }

        public UTSequence AddRange(params Func<CancellationToken, UniTask>[] tasks)
        {
            _tasks.AddRange(tasks);
            return this;
        }

        public UTSequence Add(UTSequence sequence)
        {
            _tasks.Add(_ =>
            {
                UniTask task = sequence.StartAsync(_token);
                sequence.IsStarted = false;
                sequence.IsComplete = false;
                return task;
            });
            return this;
        }

        public UTSequence AddDelayFromSeconds(float seconds)
        {
            _tasks.Add(token => UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: token));
            return this;
        }

        public async UniTask StartAsync(CancellationTokenSource cancellationToken = default)
        {
            if (cancellationToken == default)
                _token = new CancellationTokenSource();
            else
                _token = cancellationToken;

            try
            {
                IsStarted = true;
                await StartSequence().Invoke();
                IsComplete = true;
            }
            catch (OperationCanceledException)
            {
            }
        }

        public UTSequence SetLoop()
        {
            _loopType = LoopType.Loop;
            return this;
        }

        public UTSequence OnComplete(Action action)
        {
            Completed = action;
            return this;
        }

        public UTSequence OnStart(Action action)
        {
            Started = action;
            return this;
        }

        public UTSequence SetLoopType(LoopType loopType)
        {
            _loopType = loopType;
            return this;
        }

        public void Stop()
        {
            _token.Cancel();
        }

        private Func<UniTask> StartSequence()
        {
            return _loopType switch
            {
                LoopType.None => () => StartSequenceFromNumber(1),
                LoopType.Loop => StartSequenceFromLoop,
            };
        }

        private async UniTask StartSequenceFromNumber(int number)
        {
            for (int i = 0; i < number; i++)
            {
                foreach (Func<CancellationToken, UniTask> task in _tasks)
                {
                    await task.Invoke(_token.Token);
                }
            }
        }

        private async UniTask StartSequenceFromLoop()
        {
            while (_token.IsCancellationRequested == false)
            {
                foreach (Func<CancellationToken, UniTask> task in _tasks)
                {
                    await task.Invoke(_token.Token);
                }
            }
        }
    }
}