using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMorphAnim))]
	public class MegaMorphAnimEditor : Editor
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
			MegaMorphAnim anim = (MegaMorphAnim)target;

			MegaMorph morph = anim.gameObject.GetComponent<MegaMorph>();

			if ( morph != null )
			{
				string[] channels = morph.GetChannelNames();

				int index = GetIndex(anim.SrcChannel, channels);
				MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);

				if ( index != -1 )
				{
					anim.SrcChannel = channels[index];
					anim.SetChannel(morph, 0);
				}

				float min = 0.0f;
				float max = 100.0f;
				anim.GetMinMax(morph, 0, ref min, ref max);

				MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent, min, max);

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel1, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel1 = channels[index];
						anim.SetChannel(morph, 1);
					}
					anim.GetMinMax(morph, 1, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent1, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel2, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel2 = channels[index];
						anim.SetChannel(morph, 2);
					}
					anim.GetMinMax(morph, 2, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent2, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel3, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel3 = channels[index];
						anim.SetChannel(morph, 3);
					}
					anim.GetMinMax(morph, 3, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent3, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel4, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel4 = channels[index];
						anim.SetChannel(morph, 4);
					}
					anim.GetMinMax(morph, 4, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent4, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel5, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel5 = channels[index];
						anim.SetChannel(morph, 5);
					}
					anim.GetMinMax(morph, 5, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent5, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel6, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel6 = channels[index];
						anim.SetChannel(morph, 6);
					}
					anim.GetMinMax(morph, 6, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent6, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel7, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel7 = channels[index];
						anim.SetChannel(morph, 7);
					}
					anim.GetMinMax(morph, 7, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent7, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel8, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel8 = channels[index];
						anim.SetChannel(morph, 8);
					}
					anim.GetMinMax(morph, 8, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent8, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel9, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel9 = channels[index];
						anim.SetChannel(morph, 9);
					}
					anim.GetMinMax(morph, 9, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent9, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel10, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);

					if ( index != -1 )
					{
						anim.SrcChannel10 = channels[index];
						anim.SetChannel(morph, 10);
					}
					anim.GetMinMax(morph, 10, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent10, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel11, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel11 = channels[index];
						anim.SetChannel(morph, 11);
					}
					anim.GetMinMax(morph, 11, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent11, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel12, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel12 = channels[index];
						anim.SetChannel(morph, 12);
					}
					anim.GetMinMax(morph, 12, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent12, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel13, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel13 = channels[index];
						anim.SetChannel(morph, 13);
					}
					anim.GetMinMax(morph, 13, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent13, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel14, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel14 = channels[index];
						anim.SetChannel(morph, 14);
					}
					anim.GetMinMax(morph, 14, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent14, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel15, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel15 = channels[index];
						anim.SetChannel(morph, 15);
					}
					anim.GetMinMax(morph, 15, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent15, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel16, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel16 = channels[index];
						anim.SetChannel(morph, 16);
					}
					anim.GetMinMax(morph, 16, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent16, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel17, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel17 = channels[index];
						anim.SetChannel(morph, 17);
					}
					anim.GetMinMax(morph, 17, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent17, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel18, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel18 = channels[index];
						anim.SetChannel(morph, 18);
					}
					anim.GetMinMax(morph, 18, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent18, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel19, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel19 = channels[index];
						anim.SetChannel(morph, 19);
					}
					anim.GetMinMax(morph, 19, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent19, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel20, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);

					if ( index != -1 )
					{
						anim.SrcChannel20 = channels[index];
						anim.SetChannel(morph, 20);
					}
					anim.GetMinMax(morph, 20, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent20, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel21, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel21 = channels[index];
						anim.SetChannel(morph, 21);
					}
					anim.GetMinMax(morph, 21, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent21, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel22, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel22 = channels[index];
						anim.SetChannel(morph, 22);
					}
					anim.GetMinMax(morph, 22, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent22, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel23, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel23 = channels[index];
						anim.SetChannel(morph, 23);
					}
					anim.GetMinMax(morph, 23, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent23, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel24, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel24 = channels[index];
						anim.SetChannel(morph, 24);
					}
					anim.GetMinMax(morph, 24, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent24, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel25, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel25 = channels[index];
						anim.SetChannel(morph, 25);
					}
					anim.GetMinMax(morph, 25, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent25, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel26, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel16 = channels[index];
						anim.SetChannel(morph, 26);
					}
					anim.GetMinMax(morph, 26, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent26, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel27, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel27 = channels[index];
						anim.SetChannel(morph, 27);
					}
					anim.GetMinMax(morph, 27, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent27, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel28, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel28 = channels[index];
						anim.SetChannel(morph, 28);
					}
					anim.GetMinMax(morph, 28, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent28, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel29, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel29 = channels[index];
						anim.SetChannel(morph, 29);
					}
					anim.GetMinMax(morph, 29, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent29, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel30, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);

					if ( index != -1 )
					{
						anim.SrcChannel30 = channels[index];
						anim.SetChannel(morph, 30);
					}
					anim.GetMinMax(morph, 30, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent30, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel31, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel31 = channels[index];
						anim.SetChannel(morph, 31);
					}
					anim.GetMinMax(morph, 31, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent31, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel32, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel32 = channels[index];
						anim.SetChannel(morph, 32);
					}
					anim.GetMinMax(morph, 32, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent32, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel33, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel33 = channels[index];
						anim.SetChannel(morph, 33);
					}
					anim.GetMinMax(morph, 33, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent33, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel34, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel34 = channels[index];
						anim.SetChannel(morph, 34);
					}
					anim.GetMinMax(morph, 34, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent34, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel35, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel35 = channels[index];
						anim.SetChannel(morph, 35);
					}
					anim.GetMinMax(morph, 35, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent35, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel36, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel36 = channels[index];
						anim.SetChannel(morph, 36);
					}
					anim.GetMinMax(morph, 36, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent36, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel37, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel37 = channels[index];
						anim.SetChannel(morph, 37);
					}
					anim.GetMinMax(morph, 37, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent37, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel38, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel38 = channels[index];
						anim.SetChannel(morph, 38);
					}
					anim.GetMinMax(morph, 38, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent38, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel39, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel39 = channels[index];
						anim.SetChannel(morph, 39);
					}
					anim.GetMinMax(morph, 39, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent39, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel40, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);

					if ( index != -1 )
					{
						anim.SrcChannel40 = channels[index];
						anim.SetChannel(morph, 40);
					}
					anim.GetMinMax(morph, 40, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent40, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel41, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel41 = channels[index];
						anim.SetChannel(morph, 41);
					}
					anim.GetMinMax(morph, 41, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent41, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel42, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel42 = channels[index];
						anim.SetChannel(morph, 42);
					}
					anim.GetMinMax(morph, 42, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent42, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel43, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel43 = channels[index];
						anim.SetChannel(morph, 43);
					}
					anim.GetMinMax(morph, 43, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent43, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel44, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel44 = channels[index];
						anim.SetChannel(morph, 44);
					}
					anim.GetMinMax(morph, 44, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent44, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel45, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel45 = channels[index];
						anim.SetChannel(morph, 45);
					}
					anim.GetMinMax(morph, 45, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent45, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel46, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel46 = channels[index];
						anim.SetChannel(morph, 46);
					}
					anim.GetMinMax(morph, 46, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent46, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel47, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel47 = channels[index];
						anim.SetChannel(morph, 47);
					}
					anim.GetMinMax(morph, 47, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent47, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel48, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel48 = channels[index];
						anim.SetChannel(morph, 48);
					}
					anim.GetMinMax(morph, 48, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent48, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel49, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel49 = channels[index];
						anim.SetChannel(morph, 49);
					}
					anim.GetMinMax(morph, 49, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent49, min, max);
				}

				if ( index != -1 )
				{
					index = GetIndex(anim.SrcChannel50, channels);
					MegaEditorGUILayout.Popup(target, "Source Channel", ref index, channels);
					if ( index != -1 )
					{
						anim.SrcChannel50 = channels[index];
						anim.SetChannel(morph, 50);
					}
					anim.GetMinMax(morph, 50, ref min, ref max);
					MegaEditorGUILayout.Slider(target, "Percent", ref anim.Percent50, min, max);
				}

				if ( GUI.changed )
					EditorUtility.SetDirty(target);
			}
		}
	}
}
#endif