using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMorphLink))]
	public class MegaMorphLinkEditor : Editor
	{
		int GetIndex(string name, string[] channels)
		{
			int index = -1;
			for ( int i = 0; i < channels.Length; i++ )
			{
				if ( channels[i] == name )
				{
					index = i;
					break;
				}
			}
			return index;
		}

		public override void OnInspectorGUI()
		{
			MegaMorphLink anim = (MegaMorphLink)target;

			MegaEditorGUILayout.Morph(target, "Morph", ref anim.morph, true);

			MegaMorph morph = anim.morph;

			if ( morph != null )
			{
				if ( GUILayout.Button("Add Link") )
				{
					Undo.RecordObject(target, "Add Link");
					MegaMorphLinkDesc desc = new MegaMorphLinkDesc();
					anim.links.Add(desc);
				}

				string[] channels = morph.GetChannelNames();

				for ( int i = 0; i < anim.links.Count; i++ )
				{
					MegaMorphLinkDesc md = anim.links[i];
					MegaEditorGUILayout.Text(target, "Name", ref md.name);

					MegaEditorGUILayout.BeginToggle(target, "Active", ref md.active);
					MegaEditorGUILayout.Popup(target, "Channel", ref md.channel, channels);

					MegaEditorGUILayout.Transform(target, "Target", ref md.target, true);
					MegaEditorGUILayout.LinkSrc(target, "Source", ref md.src);

					if ( md.src != MegaLinkSrc.Angle && md.src != MegaLinkSrc.DotRotation )
						MegaEditorGUILayout.Axis(target, "Axis", ref md.axis);

					EditorGUILayout.LabelField("Val", md.GetVal().ToString());
					MegaEditorGUILayout.Float(target, "Min", ref md.min);
					MegaEditorGUILayout.Float(target, "Max", ref md.max);
					MegaEditorGUILayout.Float(target, "Low", ref md.low);
					MegaEditorGUILayout.Float(target, "High", ref md.high);

					MegaEditorGUILayout.BeginToggle(target, "Use Curve", ref md.useCurve);
					MegaEditorGUILayout.Curve(target, "Curve", ref md.curve);
					MegaEditorGUILayout.EndToggle();

					if ( md.src == MegaLinkSrc.Angle || md.src == MegaLinkSrc.DotRotation )
					{
						EditorGUILayout.BeginHorizontal();
						if ( GUILayout.Button("Set Start Rot") )
						{
							if ( md.target )
							{
								Undo.RecordObject(target, "Set Start Rot");
								md.rot = md.target.localRotation;
							}
						}

						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Set Min Val") )
					{
						if ( md.target )
						{
							Undo.RecordObject(target, "Set Min Val");
							md.min = md.GetVal();
						}
					}

					if ( GUILayout.Button("Set Max Val") )
					{
						if ( md.target )
						{
							Undo.RecordObject(target, "Set Max Val");
							md.max = md.GetVal();
						}
					}

					EditorGUILayout.EndHorizontal();

					EditorGUILayout.EndToggleGroup();
					if ( GUILayout.Button("Delete") )
					{
						Undo.RecordObject(target, "Delete");
						anim.links.RemoveAt(i);
						i--;
					}
				}

				if ( GUI.changed )
					EditorUtility.SetDirty(target);
			}
		}
	}
}
#endif