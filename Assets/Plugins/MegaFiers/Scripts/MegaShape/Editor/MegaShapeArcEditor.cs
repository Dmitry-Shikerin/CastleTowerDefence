using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeArc))]
	public class MegaShapeArcEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeArc shape = (MegaShapeArc)target;

			bool rebuild = false;

			float v = shape.radius;
			MegaEditorGUILayout.Float(target, "Radius", ref shape.radius);
			if ( v != shape.radius )
				rebuild = true;

			v = shape.from;
			MegaEditorGUILayout.Float(target, "From", ref shape.from);
			if ( v != shape.from )
				rebuild = true;

			v = shape.to;
			MegaEditorGUILayout.Float(target, "To", ref shape.to);
			if ( v != shape.to )
				rebuild = true;

			bool bv = shape.pie;
			MegaEditorGUILayout.Toggle(target, "Pie", ref shape.pie);
			if ( bv != shape.pie )
				rebuild = true;

			return rebuild;
		}
	}
}