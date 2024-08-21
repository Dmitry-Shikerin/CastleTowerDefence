using UnityEngine;
using System.Collections.Generic;

namespace MegaFiers
{
	public class MegaBlendShapeHolder : MonoBehaviour
	{
		public List<BlendShapeChannel>	channels;

		public List<BlendShapeChannel> GrabBlendShapes(MegaModifyObject mod)
		{
			Mesh mesh = mod.mesh;
			int bcount = mesh.blendShapeCount;
			channels = new List<BlendShapeChannel>();

			for ( int j = 0; j < bcount; j++ )
			{
				BlendShapeChannel channel = new BlendShapeChannel();

				int frames = mesh.GetBlendShapeFrameCount(j);

				channel.name = mesh.GetBlendShapeName(j);
				channel.index = j;

				channel.frames = new List<BlendShapeFrame>();

				string bname = mesh.GetBlendShapeName(j);

				for ( int f = 0; f < frames; f++ )
				{
					BlendShapeFrame frame = new BlendShapeFrame();
					frame.weight = mesh.GetBlendShapeFrameWeight(j, f);
					frame.deltaverts = new Vector3[mesh.vertexCount];
					frame.deltanorms = new Vector3[mesh.vertexCount];
					frame.deltatans = new Vector3[mesh.vertexCount];

					channel.frames.Add(frame);
					mod.mesh.GetBlendShapeFrameVertices(j, f, frame.deltaverts, frame.deltanorms, frame.deltatans);
				}

				channels.Add(channel);
			}

			return channels;
		}

		public void SetBlendShapes(MegaModifyObject mod)
		{
			mod.mesh.ClearBlendShapes();

			for ( int j = 0; j < channels.Count; j++ )
			{
				int frames = channels[j].frames.Count;
				string bname = channels[j].name;

				for ( int f = 0; f < frames; f++ )
				{
					float weight = channels[j].frames[f].weight;
					mod.mesh.AddBlendShapeFrame(bname, weight, channels[j].frames[f].deltaverts, channels[j].frames[f].deltanorms, channels[j].frames[f].deltatans);
				}
			}

			mod.originalMesh.ClearBlendShapes();
			MegaUtils.CopyBlendShapes(mod.mesh, mod.originalMesh);
		}

		void Start()
		{
			MegaModifyObject mod = GetComponent<MegaModifyObject>();	
			if ( mod )
			{
				if ( mod.mesh.blendShapeCount != channels.Count )
					SetBlendShapes(mod);
			}
		}
	}
}