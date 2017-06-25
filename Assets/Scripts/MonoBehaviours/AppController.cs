using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours
{
	// ReSharper disable once UnusedMember.Global
	public sealed partial class AppController : PunBehaviour
	{
		private const string PHOTON_ROOM_NAME = "test";
		private const float DISTANDE = 5f;

		private Coroutine _initialize;
		private PhotonStateCode _connectionStateCode;

		public GameController GameController { get; private set; }
		public UIController UIController { get; private set; }

		private IEnumerator Initialize()
		{
			GameController = new GameController(this);
			var prepare = StartCoroutine(PrepareScene());
			yield return CreateChannel();
			yield return prepare;
			yield return SpawnPawn(null);
			yield return AcquireClients();
			_initialize = null;
		}

		private IEnumerator CreateChannel()
		{
			var targets = PhotonNetwork.SendMonoMessageTargets ?? new HashSet<GameObject>();
			targets.Add(gameObject);
			PhotonNetwork.SendMonoMessageTargets = targets;

			PhotonNetwork.ConnectUsingSettings("v4.2");
			yield return new WaitWhile(() => _connectionStateCode == PhotonStateCode.Disconnected);
			if(_connectionStateCode == PhotonStateCode.Error)
				throw new Exception("photon connection error");
			//? как это говно вообще может чтото возвращать
			PhotonNetwork.JoinRoom(PHOTON_ROOM_NAME);
			yield return new WaitWhile(() => _connectionStateCode == PhotonStateCode.AtMaster);
			if(_connectionStateCode == PhotonStateCode.AtRoom)
				yield break;
			_connectionStateCode = PhotonStateCode.AtMaster;
			//? и это тоже
			PhotonNetwork.CreateRoom(PHOTON_ROOM_NAME);
			yield return new WaitWhile(() => _connectionStateCode == PhotonStateCode.AtMaster);
			if(_connectionStateCode == PhotonStateCode.AtRoom)
				yield break;

			throw new Exception("connection fail with: ..");
		}

		private IEnumerator PrepareScene()
		{
			var uiRequest = Resources.LoadAsync<GameObject>("ui_root");
			yield return uiRequest;
			// ui factory
			UIController = Instantiate(uiRequest.asset as GameObject, Vector3.zero, Quaternion.identity).GetComponent<UIController>();
		}

		private IEnumerator AcquireClients()
		{
			Array.ForEach(PhotonNetwork.otherPlayers, _ => StartCoroutine(SpawnPawn(_)));
			yield break;
		}

		private IEnumerator SpawnPawn(PhotonPlayer player)
		{
#if UNITY_EDITOR
			var pawnRequest = Resources.LoadAsync<GameObject>("pawn_editor");
#else
			var pawnRequest = Resources.LoadAsync<GameObject>("pawn");
#endif
			yield return pawnRequest;

			PawnObserver observer = null;
			// pawn factory
			if(player == null)
			{
				observer = Instantiate(pawnRequest.asset as GameObject).GetComponent<PawnObserver>();
				observer.transform.position = Vector3.left * (observer.GetComponent<SphereCollider>().radius + DISTANDE * .5f);
				GameController.SetLocalPawn(observer, UIController.LocalPawnStatusBar);
				observer.SetAsLocal(this);
				UIController.LocalPawnStatusBar.SetForLocal(PawnState.HEALTH_START_I / (float)PawnState.HEALTH_MAX_I);
			}
			else
			{
				observer = Instantiate(pawnRequest.asset as GameObject).GetComponent<PawnObserver>();
				observer.transform.position = Vector3.right * (observer.GetComponent<SphereCollider>().radius + DISTANDE * .5f);
				GameController.SetPawn(player, observer, UIController.RemotePawnStatusBar);
				observer.SetAsRemote(this);
				UIController.RemotePawnStatusBar.SetFroRemote(PawnState.HEALTH_START_I / (float)PawnState.HEALTH_MAX_I);
			}

			Debug.Log("<color=blue>SpawnPawn</color>");
		}

		[PunRPC]
		public void UpdateHealth(IDictionary<int, int> data)
		{
			foreach(var pair in data)
			{
				GameController.RemoteUpdate(GameController.PayerIdToPawn(pair.Key), pair.Value);
			}

			Debug.Log("<color=blue>DistributeId</color>");
		}

		public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

		private IEnumerator DropPawn(PhotonPlayer player)
		{
			// потому что отключение это процесс, что тоже не про фотон
			var asset = GameController.AssetForPlayer(player);
			if(asset == null)
				yield break;
			GameController.DropPawn(asset);
			DestroyImmediate(asset.gameObject);

			Debug.Log("<color=blue>DropPawn</color>");
		}

		private void ProcessMouseInput()
		{
			if(Input.GetMouseButton(0))
			{
				var array = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
				var components = array.Select(_ => _.transform.GetComponent<PawnObserver>()).ToArray();
				var component = components.FirstOrDefault(_ => !ReferenceEquals(_, null));
				GameController.HitPawn(component);
			}
		}

		private void ProcessTouchInput()
		{
			var touch = Input.GetTouch(0); 
			if(touch.phase == TouchPhase.Began)
			{
				var array = Physics.RaycastAll(Camera.main.ScreenPointToRay(touch.position));
				var components = array.Select(_ => _.transform.GetComponent<PawnObserver>()).ToArray();
				var component = components.FirstOrDefault(_ => !ReferenceEquals(_, null));
				GameController.HitPawn(component);
			}
		}

		// ReSharper disable once UnusedMember.Local
		private void Awake()
		{
			_initialize = StartCoroutine(Initialize());
		}

		// ReSharper disable once UnusedMember.Local
		private void OnDestroy()
		{
			GameController.Release();
			if(_initialize != null)
				StopCoroutine(_initialize);
			_initialize = null;

			PhotonNetwork.Disconnect();
		}

		// ReSharper disable once UnusedMember.Local
		private void Update()
		{
			if(Input.mousePresent)
				ProcessMouseInput();
			if(Input.touchSupported)
				ProcessTouchInput();
		}
	}
}
