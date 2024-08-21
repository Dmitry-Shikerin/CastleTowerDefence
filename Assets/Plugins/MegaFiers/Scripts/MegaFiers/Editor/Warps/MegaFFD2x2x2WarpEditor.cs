using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaFFD2x2x2Warp))]
	public class MegaFFD2x2x2WarpEditor : MegaFFDWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/FFD 2x2x2")]
		static void CreateFFDWarp222()
		{
			CreateFFDWarp("FFD 2x2x2", typeof(MegaFFD2x2x2Warp));
		}

		public override string GetHelpString() { return "FFD2x2x2 Warp by Chris West"; }
	}
}