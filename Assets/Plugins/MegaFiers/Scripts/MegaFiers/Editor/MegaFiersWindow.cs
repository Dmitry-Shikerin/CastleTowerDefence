using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	public class MegaFiersWindow : EditorWindow
	{
		public int		toolbarInt		= 0;
		public string[]	toolbarStrings	= new string[] { "All", "Mod", "Warp", "UV", "Sel", "Util" };
		Vector2			scroll			= Vector2.zero;
		static bool		showcommon;
		static Color	col1			= Color.yellow;
		static Color	col2			= Color.green;
		static Color	modcol			= new Color(0.75f, 0.75f, 1.0f);
		static Color	uvmodscol		= new Color(1.0f, 0.75f, 0.75f);
		static Color	warpcol			= new Color(0.75f, 1.0f, 0.75f);
		static Color	selmodscol		= new Color(1.0f, 1.0f, 0.75f);
		static Color	utilmodscol		= new Color(0.75f, 1.0f, 1.0f);
		static MegaNormalMethod NormalMethod = MegaNormalMethod.Mega;

		static MegaModBut[] mods = new MegaModBut[] {
			new MegaModBut("Bend",			"Bend a mesh",									typeof(MegaBend), modcol),
			new MegaModBut("Bubble",		"Bubble a mesh",								typeof(MegaBubble), modcol),
			new MegaModBut("Bulge",			"Add a Bulge to a mesh",						typeof(MegaBulge), modcol),
			new MegaModBut("Crumple",		"Crumple up a mesh, based on Unity Crumple",	typeof(MegaCrumple), modcol),
			new MegaModBut("Curve",			"Use a curve to bend a mesh",					typeof(MegaCurveDeform), modcol),
			new MegaModBut("Cylindrify",	"Cylindrify a mesh",							typeof(MegaCylindrify), modcol),
			new MegaModBut("Displace",		"Displace vertices using a texture",			typeof(MegaDisplace), modcol),
			new MegaModBut("FFD 2x2x2",		"FFD with a 2x2x2 lattice",						typeof(MegaFFD2x2x2), modcol),
			new MegaModBut("FFD 3x3x3",		"FFD with a 3x3x3 lattice",						typeof(MegaFFD3x3x3), modcol),
			new MegaModBut("FFD 4x4x4",		"FFD with a 4x4x4 lattice",						typeof(MegaFFD4x4x4), modcol),
			new MegaModBut("Hump",			"Add humps to a mesh",							typeof(MegaHump), modcol),
			new MegaModBut("Melt",			"Melt a mesh",									typeof(MegaMelt), modcol),
			//new MegaModBut("Morph",			"Morph a mesh",									typeof(MegaMorph), modcol),
			//new MegaModBut("MorphOMat",		"MorphOMatic a mesh",							typeof(MegaMorphOMatic), modcol),
			new MegaModBut("Noise",			"Add noise to a mesh",							typeof(MegaNoise), modcol),
			new MegaModBut("PageFlip",		"Make a page turning effect",					typeof(MegaPageFlip), modcol),
			new MegaModBut("Paint",			"Paint deformation onto a mesh",				typeof(MegaPaint), modcol),
			new MegaModBut("Path",			"Deform a mesh along a path",					typeof(MegaPathDeform), modcol),
			new MegaModBut("Pivot",			"Alter pivot point on a mesh",					typeof(MegaPivotAdjust), modcol),
			new MegaModBut("PointCache",	"Point cache animation or vertices",			typeof(MegaPointCache), modcol),
			new MegaModBut("Push",			"Push vertices along their normals",			typeof(MegaPush), modcol),
			new MegaModBut("RadialSkew",	"Radial skew a mesh",							typeof(MegaRadialSkew), modcol),
			new MegaModBut("Ripple",		"Add a ripple to a mesh",						typeof(MegaRipple), modcol),
			new MegaModBut("Rope",			"Deform a mesh using 2D Rope physics",			typeof(MegaRopeDeform), modcol),
			new MegaModBut("Rubber",		"Add secondary rubber motion to a mesh",		typeof(MegaRubber), modcol),
			new MegaModBut("Simple",		"Example of a simple mod",						typeof(MegaSimpleMod), modcol),
			new MegaModBut("Sinus",			"Sin wave deformation based on Unity example",	typeof(MegaSinusCurve), modcol),
			new MegaModBut("Skew",			"Skew a mesh",									typeof(MegaSkew), modcol),
			new MegaModBut("Spherify",		"Turn a mesh into a sphere",					typeof(MegaSpherify), modcol),
			new MegaModBut("Squeeze",		"Squeeze a mesh",								typeof(MegaSqueeze), modcol),
			new MegaModBut("Stretch",		"Squash and stretch a mesh",					typeof(MegaStretch), modcol),
			new MegaModBut("Taper",			"Taper a mesh",									typeof(MegaTaper), modcol),
			new MegaModBut("Twist",			"Twist a mesh",									typeof(MegaTwist), modcol),
			new MegaModBut("VertAnim",		"Animate vertices on a mesh",					typeof(MegaVertexAnim), modcol),
			new MegaModBut("Vert Noise",	"Add Vertical noise to a mesh",					typeof(MegaVertNoise), modcol),
			new MegaModBut("Wave",			"Add a wave to a mesh",							typeof(MegaWave), modcol),
			new MegaModBut("World Path",	"Deform a mesh along a path in world space",	typeof(MegaWorldPathDeform), modcol),
		};

		static MegaModBut[] warpmods = new MegaModBut[] {
			new MegaModBut("Warp Bind",		"Bind a mesh to a World Space Warp",			typeof(MegaWarpBind), warpcol),
			new MegaModBut("Bend",			"Bend Warp",									typeof(MegaBendWarp), warpcol),
			new MegaModBut("Noise",			"Noise Warp",									typeof(MegaNoiseWarp), warpcol),
			new MegaModBut("Ripple",		"Ripple Warp",									typeof(MegaRippleWarp), warpcol),
			new MegaModBut("Skew",			"Skew Warp",									typeof(MegaSkewWarp), warpcol),
			new MegaModBut("Stretch",		"Stretch Warp",									typeof(MegaStretchWarp), warpcol),
			new MegaModBut("Taper",			"Taper Warp",									typeof(MegaTaperWarp), warpcol),
			new MegaModBut("Twist",			"Twist Warp",									typeof(MegaTwistWarp), warpcol),
			new MegaModBut("Wave",			"Wave Warp",									typeof(MegaWaveWarp), warpcol),
		};

		//static MegaModBut[] uvmods = new MegaModBut[] {
			//new MegaModBut("UVAdjust",		"Transform a meshes UV coords",					typeof(MegaUVAdjust), uvmodscol),
			//new MegaModBut("UVTile",		"Animate UVs to playback sprite anim",			typeof(MegaUVTiles), uvmodscol),
		//};

		//static MegaModBut[] selmods = new MegaModBut[] {
			//new MegaModBut("VertCol",		"Select vertices by vertex color",				typeof(MegaVertColSelect), selmodscol),
			//new MegaModBut("Vol Select",	"Select vertices by volumes",					typeof(MegaVolSelect), selmodscol),
		//};

		static MegaModBut[] utilmods = new MegaModBut[] {
			//new MegaModBut("Anim",			"Animate morph percents",						typeof(MegaMorphAnim), utilmodscol),
			//new MegaModBut("Animator",		"Use anim clips to animate morphs",				typeof(MegaMorphAnimator), utilmodscol),
			new MegaModBut("BallBounce",	"Simulate soft ball bouncing",					typeof(MegaBallBounce), utilmodscol),
			new MegaModBut("Book",			"Book builder",									typeof(MegaBook), utilmodscol),
			//new MegaModBut("MultiCore",		"Script to toggle multicore support",			typeof(MegaToggleMultiCore), utilmodscol),
			new MegaModBut("Page",			"Build a page mesh for books",					typeof(MegaMeshPage), utilmodscol),
			new MegaModBut("Scroll",		"Simulate an old paper scroll",					typeof(MegaScroll), utilmodscol),
			new MegaModBut("WalkBridge",	"Helper to move a character across a bridge",	typeof(MegaWalkBridge), utilmodscol),
			new MegaModBut("WalkRope",		"Helper to move a character across a rope bridge",	typeof(MegaWalkRope), utilmodscol),
		};

		[MenuItem("Component/MegaFiers")]
		static void Init()
		{
			EditorWindow.GetWindow(typeof(MegaFiersWindow), false, "MegaFiers");
		}

		void AddModType(GameObject go, System.Type name, bool modobj)
		{
			if ( go )
			{
				if ( !name.IsSubclassOf(typeof(MegaModifier)) && !name.IsSubclassOf(typeof(MegaWarp)) )
				{
					go.AddComponent(name);
				}
				else
				{
					MeshFilter mf = go.GetComponent<MeshFilter>();
					if ( mf != null )
					{
						if ( modobj )
						{
							if ( name.IsSubclassOf(typeof(MegaModifier)) )
							{
								MegaModifyObject mod = go.GetComponent<MegaModifyObject>();

								if ( mod == null )
								{
									mod = go.AddComponent<MegaModifyObject>();
									mod.NormalMethod = NormalMethod;
								}
							}
						}

						if ( name != null )
						{
							if ( name.IsSubclassOf(typeof(MegaModifier)) )
							{
								MegaModifier md = (MegaModifier)go.AddComponent(name);
								if ( md )
								{
									md.gizCol1 = col1;
									md.gizCol2 = col2;
								}
							}
							else
							{
								if ( name.IsSubclassOf(typeof(MegaWarp)) )
								{
									MegaWarp md = (MegaWarp)go.AddComponent(name);
									if ( md )
									{
										md.GizCol1 = col1;
										md.GizCol2 = col2;
									}
								}
								else
								{
									go.AddComponent(name);
								}
							}
						}
					}
					else
					{
						if ( name.IsSubclassOf(typeof(MegaWarp)) )
						{
							MegaWarp md = (MegaWarp)go.AddComponent(name);
							if ( md )
							{
								md.GizCol1 = col1;
								md.GizCol2 = col2;
							}
						}
					}
				}
			}
		}

		int DoButtons(GameObject obj, MegaModBut[] buttons, float width, int bstep, bool modobj, int off)
		{
			Color c = GUI.backgroundColor;
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
					AddModType(obj, buttons[i].classname, modobj);

				off++;
				if ( off == bstep )
				{
					off = 0;
					EditorGUILayout.EndHorizontal();
				}
			}

			GUI.backgroundColor = c;
			GUI.color = guicol;
			return off;
		}

		void OnGUI()
		{
			scroll = EditorGUILayout.BeginScrollView(scroll);

			GameObject obj = Selection.activeGameObject;

			float width = this.position.width;

			float butwidth = 80.0f;

			int bstep = (int)(width / butwidth);
			if ( bstep == 0 )
				bstep = 1;

			toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, GUILayout.MaxWidth(250.0f));

			{
				int off = 0;
				switch ( toolbarInt )
				{
					case 0:
						off = DoButtons(obj, mods, (width / bstep) - 6.0f, bstep, true, off);
						off = DoButtons(obj, warpmods, (width / bstep) - 6.0f, bstep, true, off);
						//off = DoButtons(obj, uvmods, (width / bstep) - 6.0f, bstep, true, off);
						//off = DoButtons(obj, selmods, (width / bstep) - 6.0f, bstep, true, off);
						off = DoButtons(obj, utilmods, (width / bstep) - 6.0f, bstep, true, off);
						break;

					case 1:	DoButtons(obj, mods, (width / bstep) - 6.0f, bstep, true, off);	break;
					case 2:	DoButtons(obj, warpmods, (width / bstep) - 6.0f, bstep, true, off);	break;
					case 3: break;	//DoButtons(obj, uvmods, (width / bstep) - 6.0f, bstep, true, off); break;
					case 4: break;	//DoButtons(obj, selmods, (width / bstep) - 6.0f, bstep, true, off); break;
					case 5: DoButtons(obj, utilmods, (width / bstep) - 6.0f, bstep, true, off); break;
				}

				if ( off != 0 )
					EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndScrollView();
		}
	}
}