using System.Linq;
using Kudan.AR;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class PawnObserver : MonoBehaviour
	{
		private Material _material;
		private AppController _appController;

		public KudanTracker KudanTracker;

		public void SetAsLocal(AppController appController)
		{
			_appController = appController;
			_material = transform.Cast<Transform>().First().GetComponent<Renderer>().material;
			_material.shader = Shader.Find("Unlit/Color");
			_material.color = Color.red;
		}

		public void SetAsRemote(AppController appController)
		{
			_appController = appController;
			_material = transform.Cast<Transform>().First().GetComponent<Renderer>().material;
			_material.shader = Shader.Find("Unlit/Color");
			_material.color = Color.blue;
		}

		// ReSharper disable once UnusedMember.Local
		private void Awake()
		{
			Vector3 position;
			Quaternion orientation;

			KudanTracker = FindObjectOfType<KudanTracker>();
			KudanTracker.FloorPlaceGetPose(out position, out orientation);
			KudanTracker.ArbiTrackStart(position, orientation);
		}
	}
}
