using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeEllipse))]
	public class MegaShapeEllipseEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeEllipse shape = (MegaShapeEllipse)target;

			bool rebuild = false;

			float v = shape.length;
			MegaEditorGUILayout.Float(target, "Length", ref shape.length);
			if ( v != shape.length )
				rebuild = true;

			v = shape.width;
			MegaEditorGUILayout.Float(target, "Width", ref shape.width);
			if ( v != shape.width )
				rebuild = true;

			return rebuild;
		}
	}
}