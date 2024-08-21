using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

namespace MegaFiers
{
	[System.Serializable]
	public class BlendShapeFrame
	{
		public float		weight;
		public Vector3[]	deltaverts;
		public Vector3[]	deltanorms;
		public Vector3[]	deltatans;
	}

	[System.Serializable]
	public class BlendShapeChannel
	{
		public string					name;
		public int						index;
		public List<BlendShapeFrame>	frames = new List<BlendShapeFrame>();
	}

	public class MegaBlendshapeWorkshop
	{
		static public List<BlendShapeChannel> GrabBlendShapes(MegaModifyObject mod)
		{
			Mesh mesh = mod.mesh;
			int bcount = mesh.blendShapeCount;
			List<BlendShapeChannel> channels = new List<BlendShapeChannel>();

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

		static public void SetBlendShapes(MegaModifyObject mod, List<BlendShapeChannel> channels)
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

		static public void RemoveChannel(MegaModifyObject mod, string name)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mod);

			for ( int i = 0; i < channels.Count; i++ )
			{
				if ( channels[i].name == name )
				{
					channels.RemoveAt(i);
					break;
				}
			}

			// Set the blendshapes back
			SetBlendShapes(mod, channels);
		}

		static public BlendShapeChannel GetChannel(string name, List<BlendShapeChannel> channels)
		{
			for ( int i = 0; i < channels.Count; i++ )
			{
				if ( channels[i].name == name )
					return channels[i];
			}

			return null;
		}

		static int CompareFrames(BlendShapeFrame f1, BlendShapeFrame f2)
		{
			if ( f1.weight < f2.weight )
				return -1;

			if ( f1.weight > f2.weight )
				return 1;

			return 0;
		}

		static public void AddBlendShapeFrame(MegaModifyObject mod, string name, float weight)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mod);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel == null )
			{
				channel = new BlendShapeChannel();
				channel.name = name;
				channels.Add(channel);
			}

			BlendShapeFrame frame = new BlendShapeFrame();
			frame.weight = weight;
			frame.deltaverts = new Vector3[mod.startverts.Length];
			frame.deltanorms = new Vector3[mod.startverts.Length];
			frame.deltatans = new Vector3[mod.startverts.Length];

			channel.frames.Add(frame);

			NativeArray<Vector3> verts = mod.GetVerts();
			Vector3[] origVerts = mod.originalMesh.vertices;
			Vector3[] origNorms = mod.originalMesh.normals;
			Vector4[] origTans = mod.originalMesh.tangents;

			Vector4[] tans = mod.mesh.tangents;

			for ( int i = 0; i < verts.Length; i++ )
			{
				frame.deltaverts[i] = verts[i] - origVerts[i];
				frame.deltanorms[i] = mod.normals[i] - origNorms[i];
				if ( tans.Length > 0 )
					frame.deltatans[i] = tans[i] - origTans[i];
			}

			channel.frames.Sort(CompareFrames);

			SetBlendShapes(mod, channels);
		}

		static public void ChangeFrameWeight(MegaModifyObject mod, string name, int f, float weight)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mod);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel != null )
				channel.frames[f].weight = weight;

			SetBlendShapes(mod, channels);
		}

		static public void RemoveBlendShapeFrame(MegaModifyObject mod, string name, int frame)
		{
			List<BlendShapeChannel> channels = GrabBlendShapes(mod);

			BlendShapeChannel channel = GetChannel(name, channels);

			if ( channel != null )
				channel.frames.RemoveAt(frame);

			SetBlendShapes(mod, channels);
		}
	}
}