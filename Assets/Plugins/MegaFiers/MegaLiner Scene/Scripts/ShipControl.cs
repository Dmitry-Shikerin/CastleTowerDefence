using UnityEngine;

namespace MegaFiers
{
	public class ShipControl : MonoBehaviour
	{
		public enum LaunchMode
		{
			Slow,
			Fast,
		}

		public PathFollow		follow;
		public float			speed			= 0.0f;
		public float			distance;
		MegaStretch				bounceMod;
		MegaCrumple				crumpleMod;
		public float			mass			= 100.0f;
		public float			maxthrust		= 200.0f;
		float					thrust			= 0.0f;
		float					currentthrust	= 0.0f;
		float					thrustdamp		= 0.5f;
		float					tvel;
		public MegaModifyObject	modobj;
		public float			maxpen			= 1.0f;
		public float			maxsquash		= -0.2f;
		public float			springRate		= 1000.0f;
		public float			springDamp		= 0.5f;
		public float			maxcrumple		= 0.12f;
		float					launchForce;
		float					bvel;
		public LaunchMode		mode			= LaunchMode.Slow;
		public bool				useDeforms		= true;

		void Start()
		{
			if ( modobj )
			{
				bounceMod = modobj.FindMod<MegaStretch>("Bounce");
				crumpleMod = modobj.FindMod<MegaCrumple>("Bounce");
			}
		}

		void Update()
		{
			float len = follow.target.GetCurveLength(0);

			if ( mode == LaunchMode.Fast )
			{
				if ( distance == 0.0f && speed == 0.0f )
				{
					if ( Input.GetKey(KeyCode.S) )
						launchForce += maxthrust * Time.deltaTime;
					else
					{
						if ( launchForce != 0.0f )
						{
							speed += launchForce / mass;
							launchForce = 0.0f;
						}
					}
				}

				if ( bounceMod && useDeforms )
				{
					if ( distance < 0.0f )
					{
						speed += (springRate * Mathf.Abs(distance)) / mass;
						distance += speed * Time.deltaTime;

						if ( speed >= 0.0f )
						{
							speed -= springDamp * speed * Time.deltaTime;
							if ( speed < 0.0f )
								speed = 0.0f;
						}
						else
						{
							speed -= springDamp * speed * Time.deltaTime;
							if ( speed >= 0.0f )
								speed = 0.0f;
						}
					}
					else
					{
						if ( launchForce > 0.0f )
						{
							if ( launchForce > 6000.0f )
								launchForce = 6000.0f;

							bounceMod.amount = (launchForce / 6000.0f) * (maxsquash * 2.0f);
						}
						else
							bounceMod.amount = Mathf.SmoothDamp(bounceMod.amount, 0.0f, ref bvel, 0.125f);

						float acc = 0.0f;
						if ( distance > 0.0f )
							acc = -9.81f;

						speed += acc * Time.deltaTime;

						distance += speed * Time.deltaTime;

						if ( distance <= 0.0f && speed > -0.3f )
						{
							distance = 0.0f;
							speed = 0.0f;
						}
					}
				}
				else
				{
					float acc = 0.0f;
					if ( distance > 0.001f )
						acc = -9.81f + (currentthrust / mass);
					else
						acc = (currentthrust / mass);

					speed += acc * Time.deltaTime;

					distance += speed * Time.deltaTime;

					if ( distance < 0.0f )
					{
						distance = 0.0f;
						speed = -speed * 0.5f;
					}
				}

				if ( distance >= len )
					distance = len;

				if ( useDeforms )
				{
					if ( bounceMod )
					{
						if ( distance < 0.0f )
						{
							if ( bounceMod )
								bounceMod.amount = (Mathf.Abs(distance) / maxpen) * maxsquash;
						}
					}

					if ( crumpleMod )
					{
						if ( distance < 0.0f )
							crumpleMod.scale = (Mathf.Abs(distance) / maxpen) * maxcrumple;
						else
							crumpleMod.scale = 0.0f;
					}
				}
			}
			else
			{
				thrust = 0.0f;
				if ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) )
					thrust = maxthrust;
				else
				{
					if ( Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) )
						thrust = -maxthrust;
				}

				currentthrust = Mathf.SmoothDamp(currentthrust, thrust, ref tvel, thrustdamp);

				if ( bounceMod && useDeforms )
				{
					if ( distance < 0.0f )
					{
						speed += (springRate * Mathf.Abs(distance)) / mass;
						distance += speed * Time.deltaTime;

						if ( speed >= 0.0f )
						{
							speed -= springDamp * speed * Time.deltaTime;
							if ( speed < 0.0f )
								speed = 0.0f;
						}
						else
						{
							speed -= springDamp * speed * Time.deltaTime;
							if ( speed >= 0.0f )
								speed = 0.0f;
						}
					}
					else
					{
						float acc = 0.0f;
						if ( distance > 0.001f )
							acc = -9.81f + (currentthrust / mass);
						else
							acc = (currentthrust / mass);

						speed += acc * Time.deltaTime;

						distance += speed * Time.deltaTime;
						if ( distance <= 0.0f && speed > -0.3f )
						{
							distance = 0.0f;
							speed = 0.0f;
						}
					}
				}
				else
				{
					float acc = 0.0f;
					if ( distance > 0.001f )
						acc = -9.81f + (currentthrust / mass);
					else
						acc = (currentthrust / mass);

					speed += acc * Time.deltaTime;

					distance += speed * Time.deltaTime;

					if ( distance < 0.0f )
					{
						distance = 0.0f;
						speed = -speed * 0.5f;
					}
				}

				if ( distance >= len )
					distance = len;

				if ( useDeforms )
				{
					if ( bounceMod )
					{
						if ( distance < 0.0f )
						{
							if ( bounceMod )
								bounceMod.amount = (Mathf.Abs(distance) / maxpen) * maxsquash;
						}
						else
							bounceMod.amount = 0.0f;
					}

					if ( crumpleMod )
					{
						if ( distance < 0.0f )
							crumpleMod.scale = (Mathf.Abs(distance) / maxpen) * maxcrumple;
						else
							crumpleMod.scale = 0.0f;
					}
				}
			}

			if ( bounceMod )
			{
				if ( Mathf.Abs(bounceMod.amount) < 0.001f )
					bounceMod.ModEnabled = false;
				else
					bounceMod.ModEnabled = true;
			}

			if ( crumpleMod )
			{
				if ( Mathf.Abs(crumpleMod.scale) < 0.001f )
					crumpleMod.ModEnabled = false;
				else
					crumpleMod.ModEnabled = true;
			}

			follow.distance = Mathf.Max(distance, 0.0f);
		}
	}
}