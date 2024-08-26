using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.UniTaskTweens.Sequences;
using Sources.Frameworks.UniTaskTweens.Sequences.Types;
using UnityEngine;

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

        public static UTSequence UTMoveTowards(this Transform transform, Vector3 targetPosition, float speed)
        {
            return new UTSequence()
                .SetLoopType(LoopType.None)
                .Add(async token =>
                {
                    while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                    {
                        transform.position = Vector3.MoveTowards(
                            transform.position,
                            targetPosition,
                            speed * Time.deltaTime);

                        await UniTask.Yield(token);
                    }
                });
        }
    }
}