using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeStar))]
	public class MegaShapeStarEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeStar shape = (MegaShapeStar)target;

			bool rebuild = false;
			float v = shape.radius1;
			MegaEditorGUILayout.Float(target, "Radius1", ref shape.radius1);
			if ( v != shape.radius1 )
				rebuild = true;

			v = shape.radius2;
			MegaEditorGUILayout.Float(target, "Radius2", ref shape.radius2);
			if ( v != shape.radius2 )
				rebuild = true;

			int iv = shape.points;
			MegaEditorGUILayout.Int(target, "Points", ref shape.points);
			if ( iv != shape.points )
				rebuild = true;

			v = shape.distortion;
			MegaEditorGUILayout.Float(target, "Distortion", ref shape.distortion);
			if ( v != shape.distortion )
				rebuild = true;

			v = shape.fillet1;
			MegaEditorGUILayout.Float(target, "Fillet Radius 1", ref shape.fillet1);
			if ( v != shape.fillet1 )
				rebuild = true;

			v = shape.fillet2;
			MegaEditorGUILayout.Float(target, "Fillet Radius 2", ref shape.fillet2);
			if ( v != shape.fillet2 )
				rebuild = true;

			return rebuild;
		}
	}
}