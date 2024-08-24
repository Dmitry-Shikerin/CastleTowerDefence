using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;

namespace Sources.Frameworks.UniTaskTweens
{
    public static class UTTween
    {
        public static UTSequence Sequence(
            LoopType loopType = LoopType.None,
            params Func<CancellationToken, UniTask>[] tasks)
        { 
            return new UTSequence().SetLoopType(loopType).AddRange(tasks);
        }
    }
}