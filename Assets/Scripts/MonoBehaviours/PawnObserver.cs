using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class PawnObserver : MonoBehaviour
	{
		private Material _material;
		private AppController _appController;

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

		//[PunRPC]
		//// ReSharper disable once UnusedMember.Global
		//public void HitPawnRPC()
		//{
		//	if(!PhotonView.isMine)
		//		_appController.GameController.RemoteHit(this);
		//}

		//public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
	}
}
