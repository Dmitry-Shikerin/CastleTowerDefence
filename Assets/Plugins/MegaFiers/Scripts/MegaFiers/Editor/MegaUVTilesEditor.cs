using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaUVTiles))]
	public class MegaUVTilesEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			MegaUVTiles mod = (MegaUVTiles)target;

			MegaEditorGUILayout.Int(target, "Frame", ref mod.Frame);
			MegaEditorGUILayout.Int(target, "Tile Width", ref mod.TileWidth);
			MegaEditorGUILayout.Int(target, "Tile Height", ref mod.TileHeight);
			MegaEditorGUILayout.Vector2(target, "Off", ref mod.off);
			MegaEditorGUILayout.Vector2(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.Animate);
			MegaEditorGUILayout.Int(target, "End Frame", ref mod.EndFrame);
			MegaEditorGUILayout.Float(target, "Fps", ref mod.fps);
			MegaEditorGUILayout.Float(target, "Anim Time", ref mod.AnimTime);
			MegaEditorGUILayout.Toggle(target, "Flip X", ref mod.flipx);
			MegaEditorGUILayout.Toggle(target, "Flip Y", ref mod.flipy);
			MegaEditorGUILayout.RepeatMode(target, "Loop Mode", ref mod.loopMode);

			return false;
		}
	}
}
#endif