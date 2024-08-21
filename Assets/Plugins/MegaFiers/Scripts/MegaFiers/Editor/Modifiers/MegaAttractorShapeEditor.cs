using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaAttractorShape))]
	public class MegaAttractorShapeEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Spline Attractor Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaAttractorShape mod = (MegaAttractorShape)target;

			MegaEditorGUILayout.ShapeObject(target, "Shape", ref mod.shape, true);
			if ( mod.shape != null && mod.shape.splines.Count > 1 )
			{
				MegaEditorGUILayout.Int(target, "Curve", ref mod.curve, 0, mod.shape.splines.Count - 1);
				if ( mod.curve < 0 )
					mod.curve = 0;

				if ( mod.curve > mod.shape.splines.Count - 1 )
					mod.curve = mod.shape.splines.Count - 1;
			}

			MegaEditorGUILayout.Int(target, "Iter Count", ref mod.itercount, 1, 5);
			MegaEditorGUILayout.AttractType(target, "Type", ref mod.attractType);
			MegaEditorGUILayout.Float(target, "Limit", ref mod.limit);
			MegaEditorGUILayout.Float(target, "Distance", ref mod.distance);

			if ( mod.distance < 0.0f )
				mod.distance = 0.0f;

			if ( mod.attractType != MegaAttractType.Rotate )
				MegaEditorGUILayout.Float(target, "Force", ref mod.force);
			else
			{
				MegaEditorGUILayout.Float(target, "Force", ref mod.rotate);
				MegaEditorGUILayout.Float(target, "Slide", ref mod.slide);
			}
			MegaEditorGUILayout.Curve(target, "Influence Curve", ref mod.crv);
			MegaEditorGUILayout.Toggle(target, "Spline Changed", ref mod.splinechanged);
			MegaEditorGUILayout.Toggle(target, "Mesh is Flat", ref mod.flat);

			return false;
		}
	}
}