using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	[RequireComponent(typeof(PhotonView))]
	public abstract class NetworkObserver : MonoBehaviour, IPunObservable
	{
		public PhotonView PhotonView { get; private set; }

		// ReSharper disable once UnusedMember.Global
		protected virtual void Awake()
		{
			PhotonView = GetComponent<PhotonView>();
			PhotonView.ObservedComponents.Clear();
			PhotonView.ObservedComponents.Add(this);
			PhotonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
			PhotonView.viewID = PhotonNetwork.AllocateViewID();

			var targets = PhotonNetwork.SendMonoMessageTargets ?? new HashSet<GameObject>();
			targets.Add(gameObject);
			PhotonNetwork.SendMonoMessageTargets = targets;
		}

		public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
	}
}
