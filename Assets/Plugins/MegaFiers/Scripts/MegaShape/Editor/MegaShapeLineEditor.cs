using UnityEditor;
using UnityEngine;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeLine))]
	public class MegaShapeLineEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeLine shape = (MegaShapeLine)target;

			bool rebuild = false;

			float v = shape.length;
			MegaEditorGUILayout.Float(target, "Length", ref shape.length);
			if ( v != shape.length )
				rebuild = true;

			float p = shape.points;
			MegaEditorGUILayout.Int(target, "Points", ref shape.points);
			if ( p != shape.points )
				rebuild = true;

			v = shape.dir;
			MegaEditorGUILayout.Float(target, "Dir", ref shape.dir);
			if ( v != shape.dir )
				rebuild = true;

			Transform tm = shape.end;
			MegaEditorGUILayout.Transform(target, "End", ref shape.end, true);
			if ( tm != shape.end )
				rebuild = true;

			return rebuild;
		}
	}
}