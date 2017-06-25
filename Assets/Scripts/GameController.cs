using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.MonoBehaviours;
using UnityEngine;

namespace Assets.Scripts
{
	public class GameController
	{
		private const int UPDATE_DELTA_I = 10;
		private const float HIT_COOLDOWN_F = .3f;

		private readonly AppController _appController;

		private readonly Dictionary<PawnObserver, PawnState> _pawns = new Dictionary<PawnObserver, PawnState>();

		public PawnObserver LocalAsset { get; private set; }

		public GameController(AppController appController)
		{
			_appController = appController;
		}

		public PawnState GetState(PawnObserver pawn)
		{
			return pawn != null && _pawns.ContainsKey(pawn) ? _pawns[pawn] : null;
		}

		public PawnObserver AssetForPlayer(PhotonPlayer player)
		{
			return _pawns.FirstOrDefault(_ => _.Value.Player.Equals(player)).Key;
		}

		public PawnObserver PayerIdToPawn(int playerId)
		{
			return _pawns.FirstOrDefault(_ => _.Value.Player.ID == playerId).Key;
		}

		public void SetLocalPawn(PawnObserver pawn, UIHealthBarController healthBar)
		{
			if(LocalAsset != null)
				UnityEngine.Object.Destroy(LocalAsset);
			LocalAsset = pawn;
			SetPawn(PhotonNetwork.player, pawn, healthBar);
		}

		public void SetPawn(PhotonPlayer player, PawnObserver pawn, UIHealthBarController healthBar)
		{
			_pawns[pawn] = new PawnState(player, healthBar);
		}

		public void DropPawn(PawnObserver pawn)
		{
			var state = GetState(pawn);
			if(state == null)
				return;
			state.Release();
			_pawns.Remove(pawn);
		}

		public void HitPawn(PawnObserver pawn)
		{
			if(pawn == null)
				return;

			var state = GetState(pawn);
			if(Time.realtimeSinceStartup - state.PawnLastHit < HIT_COOLDOWN_F)
				return;

			var direction = ReferenceEquals(pawn, LocalAsset) ? 1 : -1;
			state.UpdateHealth(UPDATE_DELTA_I * direction);

			var data = _pawns.Cast<KeyValuePair<PawnObserver, PawnState>>().ToDictionary(_ => _.Value.Player.ID, _ => _.Value.Health);
			_appController.GetComponent<PhotonView>().RPC("UpdateHealth", PhotonTargets.Others, data);

			Debug.Log("<color=blue>HitPawn LOCAL</color>");
		}

		public void RemoteUpdate(PawnObserver pawn, int health)
		{
			if(pawn == null)
				return;

			var state = GetState(pawn);
			state.UpdateHealth(health - _pawns[pawn].Health);

			Debug.Log("<color=blue>HitPawn REMOTE</color>");
		}

		public void Release()
		{
			LocalAsset = null;
			var pawns = _pawns.Keys.ToArray();
			_pawns.Clear();
			Array.ForEach(pawns, UnityEngine.Object.Destroy);
		}
	}
}