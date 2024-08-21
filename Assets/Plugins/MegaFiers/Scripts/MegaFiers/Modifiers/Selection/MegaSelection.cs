using UnityEngine;

#if false
namespace MegaFiers
{
	public class MegaSelectionMod : MegaModifier
	{
		public override MegaModChannel ChannelsChanged()	{ return MegaModChannel.Selection; }

		public virtual	void	GetSelection(MegaModifyObject mc)	{ }
		public override bool	ModLateUpdate(MegaModContext mc)
		{
			GetSelection(mc.mod);
			return prepared;
		}

		public override void DrawGizmo(MegaModContext context)
		{
		}

		// TEST this is needed
#if false
		public override void DoWork(MegaModifyObject mc, int index, int start, int end, int cores)
		{
			for ( int i = start; i < end; i++ )
				sverts[i] = verts[i];
		}
#endif
	}
}
#endif