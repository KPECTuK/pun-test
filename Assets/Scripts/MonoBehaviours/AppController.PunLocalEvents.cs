using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	public sealed partial class AppController
	{
		// auth

		public override void OnCustomAuthenticationFailed(string message)
		{
			Debug.Log(string.Format("<color=magenta>OnCustomAuthenticationFailed {0}</color>", message));
		}

		public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			Debug.Log(string.Format("<color=magenta>OnCustomAuthenticationResponse {0}</color>", data));
		}

		// local events

		public override void OnConnectedToPhoton()
		{
			Debug.Log("<color=magenta>OnConnectedToPhoton</color>");
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			Debug.Log(string.Format("<color=magenta>OnConnectionFail {0}</color>", cause));
			_connectionStateCode = PhotonStateCode.Error;
		}

		public override void OnDisconnectedFromPhoton()
		{
			Debug.Log("<color=magenta>OnDisconnectedFromPhoton</color>");
			_connectionStateCode = PhotonStateCode.Disconnected;
		}

		public override void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			Debug.Log(string.Format("<color=magenta>OnFailedToConnectToPhoton {0}</color>", cause));
			_connectionStateCode = PhotonStateCode.Error;
		}

		public override void OnConnectedToMaster()
		{
			Debug.Log("<color=magenta>OnConnectedToMaster</color>");
			_connectionStateCode = PhotonStateCode.AtMaster;
		}

		public override void OnJoinedLobby()
		{
			Debug.Log("<color=magenta>OnJoinedLobby</color>");
		}

		public override void OnLeftLobby()
		{
			Debug.Log("<color=magenta>OnLeftLobby</color>");
		}

		public override void OnCreatedRoom()
		{
			Debug.Log("<color=magenta>OnCreatedRoom</color>");
		}

		public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
			Debug.Log(string.Format("<color=magenta>OnPhotonCreateRoomFailed {0}</color>", codeAndMsg));
			_connectionStateCode = PhotonStateCode.Error;
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("<color=magenta>OnJoinedRoom</color>");
			_connectionStateCode = PhotonStateCode.AtRoom;
		}

		public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
			Debug.Log(string.Format("<color=magenta>OnPhotonJoinRoomFailed {0}</color>", codeAndMsg));
			_connectionStateCode = PhotonStateCode.Error;
		}

		public override void OnLeftRoom()
		{
			Debug.Log("<color=magenta>OnLeftRoom</color>");
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			Debug.Log(string.Format("<color=magenta>OnPhotonRandomJoinFailed {0}</color>", codeAndMsg));
		}

		public override void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			Debug.Log(string.Format("<color=magenta>OnPhotonInstantiate {0}</color>", info));
		}
	}
}
