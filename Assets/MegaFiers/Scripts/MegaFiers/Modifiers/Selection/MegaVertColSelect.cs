using UnityEngine;

#if false
namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Selection/Vert Color")]
	public class MegaVertColSelect : MegaSelectionMod
	{
		public override MegaModChannel ChannelsReq() { return MegaModChannel.Col; }
		public override string ModName() { return "Vert Color Select"; }
		public override string GetHelpURL() { return "?page_id=1305"; }

		public float[] GetSel() { return modselection; }

		public float		gizSize			= 0.01f;
		public bool			displayWeights	= true;
		public float		weight			= 1.0f;
		public float		threshold		= 0.0f;
		public bool			update			= true;
		public MegaChannel	channel			= MegaChannel.Red;
		float[]				modselection;

		public override void GetSelection(MegaModifyObject mc)
		{
			if ( ModEnabled )
			{
				if ( modselection == null || modselection.Length != mc.jverts.Length )
					modselection = new float[mc.jverts.Length];

				if ( update )
				{
					update = false;

					if ( mc.Cols != null && mc.Cols.Length > 0 )
					{
						int c = (int)channel;
						for ( int i = 0; i < mc.jverts.Length; i++ )
							modselection[i] = ((mc.Cols[i][c] - threshold) / (1.0f - threshold)) * weight;
					}
					else
					{
						for ( int i = 0; i < mc.jverts.Length; i++ )
							modselection[i] = weight;
					}
				}

				mc.selection = modselection;
			}
		}
	}
}
#endif