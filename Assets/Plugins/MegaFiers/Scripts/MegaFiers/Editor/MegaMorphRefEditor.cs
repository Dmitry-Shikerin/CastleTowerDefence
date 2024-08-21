
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMorphRef))]
	public class MegaMorphRefEditor : Editor
	{
		static public Color ChanCol1 = new Color(0.44f, 0.67f, 1.0f);
		static public Color ChanCol2 = new Color(1.0f, 0.67f, 0.44f);

		Stack<Color> bcol = new Stack<Color>();
		Stack<Color> ccol = new Stack<Color>();
		Stack<Color> col  = new Stack<Color>();

		bool extraparams = false;

		private MegaModifier	src;
		bool showmodparams = false;
		bool showchannels = true;

		private void OnEnable()
		{
			src = target as MegaModifier;
		}

		void PushCols()
		{
			bcol.Push(GUI.backgroundColor);
			ccol.Push(GUI.contentColor);
			col.Push(GUI.color);
		}

		void PopCols()
		{
			GUI.backgroundColor = bcol.Pop();
			GUI.contentColor = ccol.Pop();
			GUI.color = col.Pop();
		}

		void DisplayChannel(MegaMorphRef morph, MegaMorphChan channel, int num)
		{
			if ( GUILayout.Button(num + " - " + channel.mName) )
				channel.showparams = !channel.showparams;

			float min = 0.0f;
			float max = 100.0f;
			if ( morph.UseLimit )
			{
				min = morph.Min;
				max = morph.Max;
			}

			GUI.backgroundColor = new Color(1, 1, 1);
			if ( channel.showparams )
			{
				MegaEditorGUILayout.Text(target, "Name", ref channel.mName);

				if ( morph.UseLimit )
					MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, min, max);
				else
				{
					if ( channel.mUseLimit )
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
					else
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, 0.0f, 100.0f);
				}
			}
			else
			{
				if ( morph.UseLimit )
					MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, min, max);
				else
				{
					if ( channel.mUseLimit )
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
					else
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, 0.0f, 100.0f);
				}
			}
		}

		void DisplayChannelLim(MegaMorphRef morph, MegaMorphChan channel, int num)
		{
			float min = 0.0f;
			float max = 100.0f;
			if ( morph.UseLimit )
			{
				min = morph.Min;
				max = morph.Max;
			}

			GUI.backgroundColor = new Color(1, 1, 1);
			if ( morph.UseLimit )
				MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, min, max);
			else
			{
				if ( channel.mUseLimit )
					MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
				else
					MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, 0.0f, 100.0f);
			}
		}

		public override void OnInspectorGUI()
		{
			MegaMorphRef morph = (MegaMorphRef)target;

			PushCols();

			MegaMorph src = (MegaMorph)EditorGUILayout.ObjectField("Source", morph.source, typeof(MegaMorph), true);
			if ( src != morph.source )
				morph.SetSource(src);

			showmodparams = EditorGUILayout.Foldout(showmodparams, "Modifier Common Params");

			if ( showmodparams )
			{
				MegaEditorGUILayout.Text(target, "Label", ref morph.Label);
				MegaEditorGUILayout.Int(target, "MaxLOD", ref morph.MaxLOD);
				MegaEditorGUILayout.Toggle(target, "Mod Enabled", ref morph.ModEnabled);
				MegaEditorGUILayout.Toggle(target, "Display Gizmo", ref morph.DisplayGizmo);
				//MegaEditorGUILayout.Int(target, "Order", ref morph.Order);
				MegaEditorGUILayout.Color(target, "Giz Col 1", ref morph.gizCol1);
				MegaEditorGUILayout.Color(target, "Giz Col 2", ref morph.gizCol2);
			}

			MegaEditorGUILayout.Toggle(target, "Animate", ref morph.animate);

			if ( morph.animate )
			{
				MegaEditorGUILayout.Float(target, "AnimTime", ref morph.animtime);
				MegaEditorGUILayout.Float(target, "LoopTime", ref morph.looptime);
				MegaEditorGUILayout.Float(target, "Speed", ref morph.speed);
				MegaEditorGUILayout.RepeatMode(target, "RepeatMode", ref morph.repeatMode);
			}

			string bname = "Hide Channels";

			if ( !showchannels )
				bname = "Show Channels";

			if ( GUILayout.Button(bname) )
				showchannels = !showchannels;

			MegaEditorGUILayout.Toggle(target, "Compact Display", ref morph.limitchandisplay);

			if ( showchannels && morph.chanBank != null )
			{
				if ( morph.limitchandisplay )
				{
					MegaEditorGUILayout.Int(target, "Start", ref morph.startchannel);
					MegaEditorGUILayout.Int(target, "Display", ref morph.displaychans);
					if ( morph.displaychans < 0 )
						morph.displaychans = 0;

					if ( morph.startchannel < 0 )
						morph.startchannel = 0;

					if ( morph.startchannel >= morph.chanBank.Count - 1 )
						morph.startchannel = morph.chanBank.Count - 1;

					int end = morph.startchannel + morph.displaychans;
					if ( end >= morph.chanBank.Count )
						end = morph.chanBank.Count;

					for ( int i = morph.startchannel; i < end; i++ )
					{
						PushCols();

						if ( (i & 1) == 0 )
							GUI.backgroundColor = ChanCol1;
						else
							GUI.backgroundColor = ChanCol2;

						DisplayChannelLim(morph, morph.chanBank[i], i);
						PopCols();
					}
				}
				else
				{
					for ( int i = 0; i < morph.chanBank.Count; i++ )
					{
						PushCols();

						if ( (i & 1) == 0 )
							GUI.backgroundColor = ChanCol1;
						else
							GUI.backgroundColor = ChanCol2;

						DisplayChannel(morph, morph.chanBank[i], i);
						PopCols();
					}
				}
			}

			extraparams = EditorGUILayout.Foldout(extraparams, "Extra Params");

			if ( extraparams )
			{
				MegaEditorGUILayout.Color(target, "Channel Col 1", ref ChanCol1);
				MegaEditorGUILayout.Color(target, "Channel Col 2", ref ChanCol2);
			}

			PopCols();

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}
	}
}
#endif