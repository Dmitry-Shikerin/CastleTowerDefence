using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MegaFiers
{
	using UnityEngine;
	using UnityEditor;

	public class EditorUtil
	{
		static public void SetDirty(Object target)
		{
			if ( target )
			{
				EditorUtility.SetDirty(target);
				if ( !Application.isPlaying )
					EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	public class MegaHandles
	{
		public static void DotCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.DotHandleCap(id, pos, rot, size, EventType.Repaint);
		}

		public static void SphereCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.SphereHandleCap(id, pos, rot, size, EventType.Repaint);
		}

		public static void ArrowCap(int id, Vector3 pos, Quaternion rot, float size)
		{
			Handles.ArrowHandleCap(id, pos, rot, size, EventType.Repaint);
		}
	}

	public class MegaEditorGUILayout
	{
		static public void Int(Object target, string name, ref int val)
		{
			EditorGUI.BeginChangeCheck();
			int newval = EditorGUILayout.IntField(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Float(Object target, string name, ref float val)
		{
			EditorGUI.BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public float Float(Object target, float val, params GUILayoutOption[] options)
		{
			EditorGUI.BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(val, options);
			if ( EditorGUI.EndChangeCheck() )
				Undo.RecordObject(target, "Float");

			return newval;
		}

		static public void Vector3(Object target, string name, ref Vector3 val)
		{
			EditorGUI.BeginChangeCheck();
			Vector3 newval = EditorGUILayout.Vector3Field(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Vector2(Object target, string name, ref Vector2 val)
		{
			EditorGUI.BeginChangeCheck();
			Vector2 newval = EditorGUILayout.Vector2Field(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Vector3Field2(Object target, string name, ref Vector3 val)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			Vector2 v2 = UnityEngine.Vector2.zero;
			v2.x = val.x;
			v2.y = val.y;
			v2 = EditorGUILayout.Vector2Field(name, v2);
			EditorGUILayout.EndHorizontal();
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val.x = v2.x;
				val.y = v2.y;
			}
		}

		static public void Transform(Object target, string name, ref Transform val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			Transform newobj = (Transform)EditorGUILayout.ObjectField(name, val, typeof(Transform), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Toggle(Object target, string name, ref bool val)
		{
			EditorGUI.BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Text(Object target, string name, ref string val)
		{
			EditorGUI.BeginChangeCheck();
			string newval = EditorGUILayout.TextField(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Curve(Object target, string name, ref AnimationCurve crv)
		{
			EditorGUI.BeginChangeCheck();
			AnimationCurve newcrv = EditorGUILayout.CurveField(name, crv);
			if ( EditorGUI.EndChangeCheck() )
			{
				//Undo.RecordObject(target, "Changed " + name);
				Undo.RegisterCompleteObjectUndo(target, "Changed " + name);
				crv = newcrv;
			}
		}

		static public void Color(Object target, string name, ref Color val)
		{
			EditorGUI.BeginChangeCheck();
			Color newval = EditorGUILayout.ColorField(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Color32(Object target, string name, ref Color32 val)
		{
			EditorGUI.BeginChangeCheck();
			Color32 newval = EditorGUILayout.ColorField(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Color(Object target, ref Color val)
		{
			EditorGUI.BeginChangeCheck();
			Color newval = EditorGUILayout.ColorField(val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Color Changed ");
				val = newval;
			}
		}

		static public void Slider(Object target, string name, ref float val, float min, float max)
		{
			EditorGUI.BeginChangeCheck();
			float newval = EditorGUILayout.Slider(name, val, min, max);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void IntSlider(Object target, string name, ref int val, int min, int max)
		{
			EditorGUI.BeginChangeCheck();
			int newval = EditorGUILayout.IntSlider(name, val, min, max);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void Int(Object target, string name, ref int val, int min, int max)
		{
			EditorGUI.BeginChangeCheck();
			int newval = EditorGUILayout.IntSlider(name, val, min, max);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void MinMax(Object target, string name, ref float min, ref float max, float minlim, float maxlim)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.MinMaxSlider(name, ref min, ref max, minlim, maxlim);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				if ( min > max )
				{
					float t = min;
					min = max;
					max = t;
				}
			}
		}

		static public void Object(Object target, string name, ref Object val, System.Type type, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			Object newobj = EditorGUILayout.ObjectField(name, val, type, flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void GameObject(Object target, string name, ref GameObject val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			GameObject newobj = (GameObject)EditorGUILayout.ObjectField(name, val, typeof(GameObject), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void WarpObject(Object target, string name, ref MegaWarp val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			MegaWarp newobj = (MegaWarp)EditorGUILayout.ObjectField(name, val, typeof(MegaWarp), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Axis(Object target, string name, ref MegaAxis val)
		{
			EditorGUI.BeginChangeCheck();
			MegaAxis newaxis = (MegaAxis)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void Falloff(Object target, string name, ref Falloff val)
		{
			EditorGUI.BeginChangeCheck();
			Falloff fall = (Falloff)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = fall;
			}
		}

		static public void EffectAxis(Object target, string name, ref MegaEffectAxis val)
		{
			EditorGUI.BeginChangeCheck();
			MegaEffectAxis newaxis = (MegaEffectAxis)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void Channel(Object target, string name, ref MegaColor val)
		{
			EditorGUI.BeginChangeCheck();
			MegaColor newcol = (MegaColor)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newcol;
			}
		}

		static public void EnumPopup(Object target, string name, ref System.Enum val)
		{
			EditorGUI.BeginChangeCheck();
			System.Enum newval = EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void BeginToggle(Object target, string name, ref bool val)
		{
			EditorGUI.BeginChangeCheck();
			bool newval = EditorGUILayout.BeginToggleGroup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

		static public void AffectPopup(Object target, string name, ref MegaAffect val)
		{
			EditorGUI.BeginChangeCheck();
			MegaAffect newaxis = (MegaAffect)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newaxis;
			}
		}

		static public void ShapeObject(Object target, string name, ref MegaShape val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			MegaShape newobj = (MegaShape)EditorGUILayout.ObjectField(name, val, typeof(MegaShape), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void AttractType(Object target, string name, ref MegaAttractType val)
		{
			EditorGUI.BeginChangeCheck();
			MegaAttractType newtype = (MegaAttractType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void ModifierObject(Object target, string name, ref MegaModifyObject val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			MegaModifyObject newobj = (MegaModifyObject)EditorGUILayout.ObjectField(name, val, typeof(MegaModifyObject), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void WrapObject(Object target, string name, ref MegaWrap val, bool flag)
		{
			EditorGUI.BeginChangeCheck();
			MegaWrap newobj = (MegaWrap)EditorGUILayout.ObjectField(name, val, typeof(MegaWrap), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public Vector3 PositionHandle(Object target, Vector3 pos, Quaternion rot)
		{
			Vector3 npos = Handles.PositionHandle(pos, rot);
			if ( npos != pos )
				Undo.RecordObject(target, "Position Handle Changed");
			return npos;
		}

		static public Vector3 FreeHandle(Object target, Vector3 pos, Quaternion rot, float size, Vector3 snap, Handles.CapFunction cap, int id = 0)
		{
#if UNITY_2022_1_OR_NEWER
			Vector3 npos = Handles.FreeMoveHandle(id, pos, size, snap, cap);
#else
			Vector3 npos = Handles.FreeMoveHandle(id, pos, rot, size, snap, cap);
#endif
			if ( npos != pos )
				Undo.RecordObject(target, "Free Handle Changed");
			return npos;
		}

		static public Vector3 SliderHandle(Object target, Vector3 pos, Vector3 dir, float size, Handles.CapFunction cap, float snap, int id = 21)
		{
			Vector3 npos = Handles.Slider(id, pos, dir, size, cap, snap);	//FreeMoveHandle(pos, rot, size, snap, cap);
			if ( npos != pos )
				Undo.RecordObject(target, "Free Handle Changed");
			return npos;
		}

		static public void MeltMat(Object target, string name, ref MegaMeltMat val)
		{
			EditorGUI.BeginChangeCheck();
			MegaMeltMat newtype = (MegaMeltMat)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void Texture2D(Object target, string name, ref Texture2D val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(name, val, typeof(Texture2D), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

		static public void Texture(Object target, string name, ref Texture val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			Texture newobj = (Texture)EditorGUILayout.ObjectField(name, val, typeof(Texture), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newobj;
			}
		}

#if false
		static public void Morph(Object target, string name, ref MegaMorph val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			MegaMorph morph = (MegaMorph)EditorGUILayout.ObjectField(name, val, typeof(MegaMorph), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = morph;
			}
		}
#endif
		static public void EndToggle()
		{
			EditorGUILayout.EndToggleGroup();
		}

		static public void Channel(Object target, string name, ref MegaChannel val)
		{
			EditorGUI.BeginChangeCheck();
			MegaChannel newchan = (MegaChannel)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newchan;
			}
		}

		static public void LoopMode(Object target, string name, ref MegaLoopMode val)
		{
			EditorGUI.BeginChangeCheck();
			MegaLoopMode newchan = (MegaLoopMode)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newchan;
			}
		}

		static public void RepeatMode(Object target, string name, ref MegaRepeatMode val)
		{
			EditorGUI.BeginChangeCheck();
			MegaRepeatMode newmode = (MegaRepeatMode)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newmode;
			}
		}

		static public void BlendMode(Object target, string name, ref MegaBlendAnimMode val)
		{
			EditorGUI.BeginChangeCheck();
			MegaBlendAnimMode newmode = (MegaBlendAnimMode)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newmode;
			}
		}

#if false
		static public void VolType(Object target, string name, ref MegaVolumeType val)
		{
			EditorGUI.BeginChangeCheck();
			MegaVolumeType newtype = (MegaVolumeType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}
#endif
		static public void NormalMethod(Object target, string name, ref MegaNormalMethod val)
		{
			EditorGUI.BeginChangeCheck();
			MegaNormalMethod newtype = (MegaNormalMethod)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void TangentMethod(Object target, string name, ref MegaTangentMethod val)
		{
			EditorGUI.BeginChangeCheck();
			MegaTangentMethod newtype = (MegaTangentMethod)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void NormType(Object target, string name, ref MegaNormType val)
		{
			EditorGUI.BeginChangeCheck();
			MegaNormType newtype = (MegaNormType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void WeightChannel(Object target, string name, ref MegaWeightChannel val)
		{
			EditorGUI.BeginChangeCheck();
			MegaWeightChannel newtype = (MegaWeightChannel)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void RubberType(Object target, string name, ref MegaRubberType val)
		{
			EditorGUI.BeginChangeCheck();
			MegaRubberType newtype = (MegaRubberType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void InterpMethod(Object target, string name, ref MegaInterpMethod val)
		{
			EditorGUI.BeginChangeCheck();
			MegaInterpMethod newtype = (MegaInterpMethod)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void PointCache(Object target, string name, ref MegaPointCache val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			MegaPointCache newpc = (MegaPointCache)EditorGUILayout.ObjectField(name, val, typeof(MegaPointCache), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newpc;
			}
		}

		static public void Popup(Object target, string name, ref int val, string[] values)
		{
			EditorGUI.BeginChangeCheck();
			int newval = EditorGUILayout.Popup(name, val, values);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newval;
			}
		}

#if false
		static public void LinkSrc(Object target, string name, ref MegaLinkSrc val)
		{
			EditorGUI.BeginChangeCheck();
			MegaLinkSrc newtype = (MegaLinkSrc)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}
#endif

		static public void FallOff(Object target, string name, ref MegaFallOff val)
		{
			EditorGUI.BeginChangeCheck();
			MegaFallOff newtype = (MegaFallOff)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void PaintMode(Object target, string name, ref MegaPaintMode val)
		{
			EditorGUI.BeginChangeCheck();
			MegaPaintMode newtype = (MegaPaintMode)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void Material(Object target, string name, ref Material val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			Material newmat = (Material)EditorGUILayout.ObjectField(name, val, typeof(Material), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newmat;
			}
		}

		static public void MeshShapeType(Object target, string name, ref MeshShapeType val)
		{
			EditorGUI.BeginChangeCheck();
			MeshShapeType newtype = (MeshShapeType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void HoseType(Object target, string name, ref MegaHoseType val)
		{
			EditorGUI.BeginChangeCheck();
			MegaHoseType newtype = (MegaHoseType)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}

		static public void Hose(Object target, string name, ref MegaHose val, bool flag = true)
		{
			EditorGUI.BeginChangeCheck();
			MegaHose newhose = (MegaHose)EditorGUILayout.ObjectField(name, val, typeof(MegaHose), flag);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newhose;
			}
		}

		static public void Integrator(Object target, string name, ref MegaIntegrator val)
		{
			EditorGUI.BeginChangeCheck();
			MegaIntegrator newtype = (MegaIntegrator)EditorGUILayout.EnumPopup(name, val);
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject(target, "Changed " + name);
				val = newtype;
			}
		}
	}
}