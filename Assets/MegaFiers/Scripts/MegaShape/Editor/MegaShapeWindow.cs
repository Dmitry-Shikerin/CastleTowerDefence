using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	public class MegaShapeWindow : EditorWindow
	{
		Vector2			scroll			= Vector2.zero;
		int				toolbarInt		= 0;
		string[]		toolbarStrings	= { "Shapes", "Params" };
		static bool		showcommon;
		static MegaAxis	axis			= MegaAxis.Y;
		static bool		drawknots		= true;
		static bool		drawhandles		= false;
		static float	stepdist		= 0.5f;
		static float	knotsize		= 2.0f;
		static Color	col1			= Color.white;
		static Color	col2			= Color.black;
		static bool		makemesh		= false;
		static Color	butcol			= new Color(0.75f, 0.75f, 1.0f);

		[MenuItem("GameObject/Create Other/MegaShape/Star Shape")]			static void CreateStarShape()		{ CreateShape("Star", typeof(MegaShapeStar)); }
		[MenuItem("GameObject/Create Other/MegaShape/Circle Shape")]		static void CreateCircleShape()		{ CreateShape("Circle", typeof(MegaShapeCircle)); }
		[MenuItem("GameObject/Create Other/MegaShape/NGon Shape")]			static void CreateNGonShape()		{ CreateShape("NGon", typeof(MegaShapeNGon)); }
		[MenuItem("GameObject/Create Other/MegaShape/Arc Shape")]			static void CreateArcShape()		{ CreateShape("Arc", typeof(MegaShapeArc)); }
		[MenuItem("GameObject/Create Other/MegaShape/Ellipse Shape")]		static void CreateEllipseShape()	{ CreateShape("Ellipse", typeof(MegaShapeEllipse)); }
		[MenuItem("GameObject/Create Other/MegaShape/Rectangle Shape")]		static void CreateRectangleShape()	{ CreateShape("Rectangle", typeof(MegaShapeRectangle)); }
		[MenuItem("GameObject/Create Other/MegaShape/Helix Shape")]			static void CreateHelixShape()		{ CreateShape("Helix", typeof(MegaShapeHelix)); }
		[MenuItem("GameObject/Create Other/MegaShape/Line Shape")]			static void CreateLineShape()		{ CreateShape("Line", typeof(MegaShapeLine)); }

		static MegaModBut[] mods = new MegaModBut[] {
			new MegaModBut("Arc",			"Create a Arc Shape",		typeof(MegaShapeArc), butcol),
			new MegaModBut("Circle",		"Create a Circle Shape",	typeof(MegaShapeCircle), butcol),
			new MegaModBut("Ellipse",		"Create a Ellipse Shape",	typeof(MegaShapeEllipse), butcol),
			new MegaModBut("Helix",			"Create a Helix Shape",		typeof(MegaShapeHelix), butcol),
			new MegaModBut("Line",			"Create a Line Shape",		typeof(MegaShapeLine), butcol),
			new MegaModBut("NGon",			"Create a NGon Shape",		typeof(MegaShapeNGon), butcol),
			new MegaModBut("Rectangle",		"Create a Rectangle Shape",	typeof(MegaShapeRectangle), butcol),
			new MegaModBut("Star",			"Create a Star Shape",		typeof(MegaShapeStar), butcol),
		};

		[MenuItem("GameObject/Mega Shapes")]
		static void Init()
		{
			EditorWindow.GetWindow(typeof(MegaShapeWindow), false, "MegaShapes");
		}

		void DoButtons(MegaModBut[] buttons, float width, int bstep, bool modobj)
		{
			Color c = GUI.backgroundColor;
			int off = 0;
			GUI.backgroundColor = Color.blue;
			Color guicol = GUI.color;
			GUI.color = new Color(1, 1, 1, 1);
			GUI.backgroundColor = new Color(0, 0, 0, 0);
			GUI.contentColor = Color.white;

			for ( int i = 0; i < buttons.Length; i++ )
			{
				GUI.contentColor = buttons[i].color;
				GUI.backgroundColor = buttons[i].color * 0.08f;

				if ( off == 0 )
					EditorGUILayout.BeginHorizontal();

				if ( GUILayout.Button(buttons[i].content, GUILayout.Width(width)) )
					CreateShape(buttons[i].name, buttons[i].classname);

				off++;
				if ( off == bstep )
				{
					off = 0;
					EditorGUILayout.EndHorizontal();
				}
			}

			if ( off != 0 )
				EditorGUILayout.EndHorizontal();

			GUI.backgroundColor = c;
			GUI.color = guicol;
		}

		void OnGUI()
		{
			scroll = EditorGUILayout.BeginScrollView(scroll);
			toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, GUILayout.MaxWidth(150.0f));

			float butwidth = 80.0f;
			float width = this.position.width;

			int bstep = (int)(width / butwidth);
			if ( bstep == 0 )
				bstep = 1;

			if ( toolbarInt == 0 )
				DoButtons(mods, (width / bstep) - 6.0f, bstep, true);
			else
			{
				axis		= (MegaAxis)EditorGUILayout.EnumPopup("Axis", axis);
				stepdist	= EditorGUILayout.FloatField("Step Dist", stepdist);
				knotsize	= EditorGUILayout.FloatField("Knot Size", knotsize);
				drawknots	= EditorGUILayout.Toggle("Draw Knots", drawknots);
				drawhandles	= EditorGUILayout.Toggle("Draw Handles", drawhandles);
				col1		= EditorGUILayout.ColorField("Color 1", col1);
				col2		= EditorGUILayout.ColorField("Color 2", col2);
				makemesh	= EditorGUILayout.Toggle("Make Mesh", makemesh);
			}
			EditorGUILayout.EndScrollView();
		}

		static void CreateShape(string type, System.Type classtype)
		{
			Vector3 pos = Vector3.zero;

			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject(type + " Shape");

			MegaShape ms = (MegaShape)go.AddComponent(classtype);

			go.transform.position = pos;
			Selection.activeObject = go;

			if ( ms != null )
			{
				ms.axis			= axis;
				ms.drawHandles	= drawhandles;
				ms.drawKnots	= drawknots;
				ms.col1			= col1;
				ms.col2			= col2;
				ms.KnotSize		= knotsize;
				ms.stepdist		= stepdist;
				ms.makeMesh		= makemesh;
				ms.handleType	= MegaHandleType.Free;
			}
		}
	}
}