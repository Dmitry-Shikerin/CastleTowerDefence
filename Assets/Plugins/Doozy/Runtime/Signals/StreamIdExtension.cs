// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using UnityEngine;
// ReSharper disable All

namespace Doozy.Runtime.Signals
{
    public partial class Signal
    {
        public static bool Send(StreamId.MainMenu id, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), message);
        public static bool Send(StreamId.MainMenu id, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalSource, message);
        public static bool Send(StreamId.MainMenu id, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalProvider, message);
        public static bool Send(StreamId.MainMenu id, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalSender, message);
        public static bool Send<T>(StreamId.MainMenu id, T signalValue, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalValue, message);
        public static bool Send<T>(StreamId.MainMenu id, T signalValue, GameObject signalSource, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalValue, signalSource, message);
        public static bool Send<T>(StreamId.MainMenu id, T signalValue, SignalProvider signalProvider, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalValue, signalProvider, message);
        public static bool Send<T>(StreamId.MainMenu id, T signalValue, Object signalSender, string message = "") => SignalsService.SendSignal(nameof(StreamId.MainMenu), id.ToString(), signalValue, signalSender, message);
   
    }

    public partial class StreamId
    {
        public enum MainMenu
        {
            Leaderboard
        }         
    }
}

