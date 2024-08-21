using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	namespace MegaFiers
	{
		[CustomEditor(typeof(MegaShapeRectangle))]
		public class MegaShapeRectangleEditor : MegaShapeEditor
		{
			public override bool Params()
			{
				MegaShapeRectangle shape = (MegaShapeRectangle)target;

				bool rebuild = false;

				float v = shape.length;
				MegaEditorGUILayout.Float(target, "Length", ref shape.length);
				if ( v != shape.length )
					rebuild = true;

				v = shape.width;
				MegaEditorGUILayout.Float(target, "Width", ref shape.width);
				if ( v != shape.width )
					rebuild = true;

				v = shape.fillet;
				MegaEditorGUILayout.Float(target, "Fillet", ref shape.fillet);
				if ( v != shape.fillet )
					rebuild = true;

				return rebuild;
			}
		}
	}
}