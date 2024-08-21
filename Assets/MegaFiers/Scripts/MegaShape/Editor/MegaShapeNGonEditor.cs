using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeNGon))]
	public class MegaShapeNGonEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeNGon shape = (MegaShapeNGon)target;

			bool rebuild = false;

			float v = shape.radius;
			MegaEditorGUILayout.Float(target, "Radius", ref shape.radius);
			if ( v != shape.radius )
				rebuild = true;

			v = shape.fillet;
			MegaEditorGUILayout.Float(target, "Fillet", ref shape.fillet);
			if ( v != shape.fillet )
				rebuild = true;

			int iv = shape.sides;
			MegaEditorGUILayout.Int(target, "Side", ref shape.sides);
			if ( iv != shape.sides )
				rebuild = true;

			bool bv = shape.circular;
			MegaEditorGUILayout.Toggle(target, "Circular", ref shape.circular);
			if ( bv != shape.circular )
				rebuild = true;

			bv = shape.scribe;
			MegaEditorGUILayout.Toggle(target, "Circumscribed", ref shape.scribe);
			if ( bv != shape.scribe )
				rebuild = true;

			return rebuild;
		}
	}
}