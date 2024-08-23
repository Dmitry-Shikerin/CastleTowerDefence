using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;
using Sources.Frameworks.UniTaskTwins.Sequences;

namespace Sources.Frameworks.UniTaskTweens.Sequences
{
    public class UTSequence : IUTSequence
    {
        private CancellationTokenSource _token = new CancellationTokenSource();
        private List<Func<CancellationToken, UniTask>>_tasks = new List<Func<CancellationToken, UniTask>>();
        
        private LoopType _loopType = LoopType.None;

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

        public UTSequence AddDelayFromSeconds(float seconds)
        {
            _tasks.Add( token => UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: token));
            return this;
        }

        public async void Start()
        {
            _token = new CancellationTokenSource();
            
            try
            {
                await StartSequence().Invoke();
            }
            catch (OperationCanceledException)
            {
            }
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