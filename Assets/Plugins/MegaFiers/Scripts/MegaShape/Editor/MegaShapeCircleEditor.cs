using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeCircle))]
	public class MegaShapeCircleEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeCircle shape = (MegaShapeCircle)target;

			bool rebuild = false;

			float radius = shape.Radius;
			MegaEditorGUILayout.Float(target, "Radius", ref shape.Radius);
			if ( radius != shape.Radius )
			{
				if ( shape.Radius < 0.001f )
					shape.Radius = 0.001f;
				rebuild = true;
			}

			return rebuild;
		}
	}
}