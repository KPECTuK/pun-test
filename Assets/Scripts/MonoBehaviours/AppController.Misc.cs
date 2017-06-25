using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	public partial class AppController
	{
		// statistics

		public override void OnLobbyStatisticsUpdate()
		{
			Debug.Log("<color=magenta>OnLobbyStatisticsUpdate</color>");
		}

		public override void OnPhotonMaxCccuReached()
		{
			Debug.Log("<color=magenta>OnPhotonMaxCccuReached</color>");
		}

		// ownership

		public override void OnOwnershipRequest(object[] viewAndPlayer)
		{
			Debug.Log(string.Format("<color=magenta>OnOwnershipRequest {0}</color>", viewAndPlayer));
		}

		public override void OnOwnershipTransfered(object[] viewAndPlayers)
		{
			Debug.Log("<color=magenta>OnOwnershipTransfered</color>");
		}

		// room properties

		public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
			Debug.Log("<color=magenta>OnPhotonCustomRoomPropertiesChanged</color>");
		}

		public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
			Debug.Log("<color=magenta>OnPhotonPlayerPropertiesChanged</color>");
		}
	}
}
