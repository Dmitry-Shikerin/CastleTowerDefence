using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWorldPathDeform))]
	public class MegaWorldPathDeformEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			MegaWorldPathDeform mod = (MegaWorldPathDeform)target;

			MegaEditorGUILayout.Toggle(target, "Use Distance", ref mod.usedist);

			if ( mod.usedist )
				MegaEditorGUILayout.Float(target, "Distance", ref mod.distance);
			else
				MegaEditorGUILayout.Float(target, "Percent", ref mod.percent);

			MegaEditorGUILayout.Float(target, "Stretch", ref mod.stretch);
			MegaEditorGUILayout.Float(target, "Twist", ref mod.twist);
			MegaEditorGUILayout.Float(target, "Rotate", ref mod.rotate);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Toggle(target, "Flip", ref mod.flip);
			MegaEditorGUILayout.ShapeObject(target, "Path", ref mod.path, true);

			if ( mod.path != null && mod.path.splines.Count > 1 )
			{
				MegaEditorGUILayout.Int(target, "Curve", ref mod.curve, 0, mod.path.splines.Count - 1);
				mod.curve = Mathf.Clamp(mod.curve, 0, mod.path.splines.Count - 1);
			}

			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.LoopMode(target, "Loop Mode", ref mod.loopmode);
			MegaEditorGUILayout.Float(target, "Tangent", ref mod.tangent);
			MegaEditorGUILayout.Toggle(target, "Use Twist Curve", ref mod.UseTwistCurve);
			MegaEditorGUILayout.Curve(target, "Twist Curve", ref mod.twistCurve);
			MegaEditorGUILayout.Toggle(target, "Use Stretch Curve", ref mod.UseStretchCurve);
			MegaEditorGUILayout.Curve(target, "Stretch Curve", ref mod.stretchCurve);
			MegaEditorGUILayout.Vector3(target, "Up", ref mod.Up);
			return false;
		}
	}
}