using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	public sealed partial class AppController
	{
		// remote events

		public override void OnMasterClientSwitched(PhotonPlayer player)
		{
			Debug.Log(string.Format("<color=magenta>OnMasterClientSwitched {0}</color>", player));
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer player)
		{
			StartCoroutine(SpawnPawn(player));
			Debug.Log(string.Format("<color=magenta>OnPhotonPlayerConnected {0}</color>", player));
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
		{
			StartCoroutine(DropPawn(player));
			Debug.Log(string.Format("<color=magenta>OnPhotonPlayerDisconnected {0}</color>", player));
		}

		public override void OnPhotonPlayerActivityChanged(PhotonPlayer player)
		{
			Debug.Log(string.Format("<color=magenta>OnPhotonPlayerActivityChanged {0}</color>", player));
		}

		public override void OnUpdatedFriendList()
		{
			Debug.Log("<color=magenta>OnUpdatedFriendList</color>");
		}

		public override void OnReceivedRoomListUpdate()
		{
			Debug.Log("<color=magenta>OnReceivedRoomListUpdate</color>");
		}

		public override void OnWebRpcResponse(OperationResponse response)
		{
			Debug.Log(string.Format("<color=magenta>OnWebRpcResponse {0}</color>", response));
		}
	}
}
