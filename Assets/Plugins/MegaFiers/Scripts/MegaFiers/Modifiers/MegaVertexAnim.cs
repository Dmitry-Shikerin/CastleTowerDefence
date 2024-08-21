using UnityEngine;

namespace MegaFiers
{
	[System.Serializable]
	public class MegaAnimatedVert
	{
		public int[]				indices;
		public Vector3				startVert;
		public MegaBezVector3KeyControl	con;
	}

	[AddComponentMenu("Modifiers/Vertex Anim")]
	public class MegaVertexAnim : MegaModifier
	{
		public float	time		= 0.0f;
		public bool		animated	= false;
		public float	speed		= 1.0f;
		public float	maxtime		= 4.0f;
		public int[]	NoAnim;
		public float	weight		= 1.0f;

		public MegaAnimatedVert[]	Verts;
		float t;

		public MegaBlendAnimMode	blendMode = MegaBlendAnimMode.Additive;

		public override string ModName()	{ return "AnimatedMesh"; }
		public override string GetHelpURL() { return "?page_id=1350"; }

		void Replace(MegaModifyObject mc, int startvert, int endvert)
		{
			for ( int i = startvert; i < endvert; i++ )
			{
				MegaBezVector3KeyControl bc = (MegaBezVector3KeyControl)Verts[i].con;

				Vector3 off = bc.GetVector3(t);

				for ( int v = 0; v < Verts[i].indices.Length; v++ )
					jsverts[Verts[i].indices[v]] = off;
			}
		}

		void ReplaceWeighted(MegaModifyObject mc, int startvert, int endvert)
		{
			for ( int i = startvert; i < endvert; i++ )
			{
				MegaBezVector3KeyControl bc = (MegaBezVector3KeyControl)Verts[i].con;

				Vector3 off = bc.GetVector3(t);

				float w = mc.selection[Verts[i].indices[0]] * weight;

				Vector3 p1 = jverts[Verts[i].indices[0]];

				off = p1 + ((off - p1) * w);

				for ( int v = 0; v < Verts[i].indices.Length; v++ )
					jsverts[Verts[i].indices[v]] = off;
			}
		}

		void Additive(MegaModifyObject mc, int startvert, int endvert)
		{
			for ( int i = startvert; i < endvert; i++ )
			{
				MegaBezVector3KeyControl bc = (MegaBezVector3KeyControl)Verts[i].con;

				Vector3 basep = mc.jverts[Verts[i].indices[0]];
				Vector3 off = bc.GetVector3(t) - basep;

				off = verts[Verts[i].indices[0]] + (off * weight);

				for ( int v = 0; v < Verts[i].indices.Length; v++ )
				{
					int idx = Verts[i].indices[v];

					jsverts[idx] = off;
				}
			}
		}

		void AdditiveWeighted(MegaModifyObject mc, int startvert, int endvert)
		{
			for ( int i = startvert; i < endvert; i++ )
			{
				MegaBezVector3KeyControl bc = (MegaBezVector3KeyControl)Verts[i].con;

				Vector3 basep = mc.jverts[Verts[i].indices[0]];
				Vector3 off = bc.GetVector3(t) - basep;

				float w = mc.selection[Verts[i].indices[0]] * weight;

				Vector3 p1 = jverts[Verts[i].indices[0]];

				off = p1 + ((off - p1) * w);

				for ( int v = 0; v < Verts[i].indices.Length; v++ )
				{
					int idx = Verts[i].indices[v];

					jsverts[idx] = off;
				}
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			switch ( blendMode )
			{
				case MegaBlendAnimMode.Additive:	Additive(mc, 0, Verts.Length);	break;
				case MegaBlendAnimMode.Replace:		Replace(mc, 0, Verts.Length); break;
			}

			if ( NoAnim != null )
			{
				for ( int i = 0; i < NoAnim.Length; i++ )
				{
					int index = NoAnim[i];
					jsverts[index] = jverts[index];
				}
			}
		}

		public MegaRepeatMode LoopMode = MegaRepeatMode.PingPong;

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( animated )
			{
				if ( Application.isPlaying )
					time += Time.deltaTime * speed;
			}

			switch ( LoopMode )
			{
				case MegaRepeatMode.Loop: t = Mathf.Repeat(time, maxtime); break;
				case MegaRepeatMode.PingPong: t = Mathf.PingPong(time, maxtime); break;
				case MegaRepeatMode.Clamp: t = Mathf.Clamp(time, 0.0f, maxtime); break;
			}

			return true;
		}
	}
}