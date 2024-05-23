using System.Collections.Generic;
using UnityEngine;

namespace Sources.Frameworks.YandexSdcFramework.Presentations.Views
{
    public class LeaderBoardElementViewContainer
    {
        [field: SerializeField] public List<LeaderBoardElementView> LeaderBoardElementViews { get; private set; }
    }
}