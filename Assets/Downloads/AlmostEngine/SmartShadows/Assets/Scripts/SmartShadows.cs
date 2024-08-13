using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AlmostEngine.Shadows
{
	public class SmartShadows
	{
		public static string VERSION = "Smart Shadows v1.1.0";
		public static string AUTHOR = "(c)Arnaud Emilien - support@wildmagegames.com";

		#if UNITY_EDITOR
		public static void About ()
		{
			EditorUtility.DisplayDialog ("About", VERSION + "\n" + AUTHOR, "Close");
		}
		#endif
	}
}

