using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaShapeHelix))]
	public class MegaShapeHelixEditor : MegaShapeEditor
	{
		public override bool Params()
		{
			MegaShapeHelix shape = (MegaShapeHelix)target;

			bool rebuild = false;

			float v = shape.radius1;
			MegaEditorGUILayout.Float(target, "Radius 1", ref shape.radius1);
			if ( v != shape.radius1 )
				rebuild = true;

			v = shape.radius2;
			MegaEditorGUILayout.Float(target, "Radius 2", ref shape.radius2);
			if ( v != shape.radius2 )
				rebuild = true;

			v = shape.height;
			MegaEditorGUILayout.Float(target, "Height", ref shape.height);
			if ( v != shape.height )
				rebuild = true;

			v = shape.turns;
			MegaEditorGUILayout.Float(target, "Turns", ref shape.turns);
			if ( v != shape.turns )
				rebuild = true;

			int iv = shape.PointsPerTurn;
			MegaEditorGUILayout.Int(target, "Points Per Turn", ref shape.PointsPerTurn);
			if ( iv != shape.PointsPerTurn )
				rebuild = true;

			bool bv = shape.clockwise;
			MegaEditorGUILayout.Toggle(target, "Clockwise", ref shape.clockwise);
			if ( bv != shape.clockwise )
				rebuild = true;

			return rebuild;
		}
	}
}