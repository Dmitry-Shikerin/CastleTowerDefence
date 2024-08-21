using UnityEngine;

namespace MegaFiers
{
	public class MegaPointCacheInstance : MonoBehaviour
	{
		[HideInInspector]
		public MegaPointCache	mod;
		[HideInInspector]
		public MegaModifyObject	modobj;

		public GameObject		obj;
		[HideInInspector]
		public Mesh				mesh;
		public float			time		= 0.0f;		// Animation time
		public float			speed		= 1.0f;		// Speed of animation playback
		public int				updateRate	= 0;		// update rate of mesh, 0 is every frame 1 every other etc
		public int				frame		= 0;		// current frame, can be used to stagger updates
		public bool				recalcNorms = true;		// recalc normals
		public bool				recalcBounds = true;	// recalc bounds

		public void SetSource(GameObject srcobj)
		{
			if ( srcobj )
			{
				if ( mesh == null )
					mesh = MegaUtils.GetMesh(gameObject);

				Mesh srcmesh = MegaUtils.GetMesh(srcobj);

				if ( srcmesh.vertexCount == mesh.vertexCount )
				{
					obj		= srcobj;
					mod		= (MegaPointCache)srcobj.GetComponent<MegaPointCache>();
					modobj	= (MegaModifyObject)srcobj.GetComponent<MegaModifyObject>();
					mesh	= MegaUtils.GetMesh(gameObject);
				}
			}
		}

		void Update()
		{
			if ( mod && modobj && mesh )
			{
				if ( Application.isPlaying )
					time += Time.deltaTime * speed;

				frame--;
				if ( frame < 0 )
				{
					frame = updateRate;

					mod.ModifyInstance(modobj, time);

					mesh.vertices = mod.sverts;

					if ( recalcNorms )
						mesh.RecalculateNormals();

					if ( recalcBounds )
						mesh.RecalculateBounds();
				}
			}
		}
	}
}