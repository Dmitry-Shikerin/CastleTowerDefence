using UnityEngine;
using UnityEditor;
using System.IO;

namespace MegaFiers
{
	public class MegaShapeOSMWindow : EditorWindow
	{
		public static float			importscale		= 1.0f;
		public static float			smoothness		= 0.0f;
		public static bool			constantspeed	= true;
		public static bool			combine			= false;
		public static MegaShapeOSM	osm;
		public static string		text;
		public static string		importname;
		public static bool			showtags		= true;
		Vector2						pos;

		static public void Init()
		{
			MegaShapeOSMWindow window = ScriptableObject.CreateInstance<MegaShapeOSMWindow>();
			window.titleContent = new GUIContent("Import OSM");
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
			window.ShowUtility();
		}

		void OnGUI()
		{
			importscale = EditorGUILayout.FloatField("Import Scale", importscale);
			constantspeed = EditorGUILayout.Toggle("Constant Speed", constantspeed);
			smoothness = EditorGUILayout.Slider("Smoothness", smoothness, 0.0f, 2.0f);

			if ( GUILayout.Button("Open OSM File") )
			{
				string filename = EditorUtility.OpenFilePanel("OSM File", lastosmpath, "OSM");

				if ( filename == null || filename.Length < 1 )
					return;

				lastosmpath = filename;

				StreamReader streamReader = new StreamReader(filename);
				text = streamReader.ReadToEnd();
				streamReader.Close();
				osm = new MegaShapeOSM();
				importname = System.IO.Path.GetFileNameWithoutExtension(filename);
				osm.readOSMData(text);
			}

			showtags = EditorGUILayout.Foldout(showtags, "Catagories");

			if ( showtags )
			{
				pos = EditorGUILayout.BeginScrollView(pos, "box");

				for ( int i = 0; i < MegaShapeOSM.tags.Count; i++ )
				{
					MegaShapeOSMTag tag = MegaShapeOSM.tags[i];

					tag.show = EditorGUILayout.Foldout(tag.show, tag.k);
					if ( tag.show )
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("", GUILayout.Width(8));
						bool import = EditorGUILayout.Toggle("", tag.import, GUILayout.Width(14));
						if ( import != tag.import )
						{
							tag.import = import;
							for ( int j = 0; j < tag.vs.Count; j++ )
							{
								MegaShapeOSMTag tagv = tag.vs[j];
								tagv.import = import;
							}
						}
						EditorGUILayout.LabelField(tag.k);
						EditorGUILayout.EndHorizontal();

						for ( int j = 0; j < tag.vs.Count; j++ )
						{
							MegaShapeOSMTag tagv = tag.vs[j];

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("", GUILayout.Width(24));
							tagv.import = EditorGUILayout.Toggle("", tagv.import, GUILayout.Width(14));
							EditorGUILayout.LabelField(tagv.k);
							EditorGUILayout.EndHorizontal();
						}
					}
				}

				EditorGUILayout.EndScrollView();
			}

			if ( GUILayout.Button("Import") )
			{
				osm.importData(text, importscale, constantspeed, importname, smoothness, combine);
				this.Close();
			}
		}

		static public string lastosmpath = "";

		[MenuItem("Assets/Import OSM")]
		static void ImportOSM()
		{
			Init();
		}
	}
}