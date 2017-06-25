using Assets.Scripts.MonoBehaviours;
using UnityEngine;

namespace Assets.Scripts
{
	public class PawnState
	{
		public const int HEALTH_MAX_I = 100;
		public const int HEALTH_START_I = 50;

		private readonly UIHealthBarController _statusBar;

		public PhotonPlayer Player { get; private set; }
		public int Health { get; private set; }
		public float PawnLastHit { get; private set; }

		public PawnState(PhotonPlayer player, UIHealthBarController statusBar)
		{
			Player = player;
			_statusBar = statusBar;
			Health = HEALTH_START_I;
		}

		public void UpdateHealth(int delta)
		{
			Health = Health + delta;
			Health = Health < 0 ? 0 : Health;
			Health = Health > HEALTH_MAX_I ? HEALTH_MAX_I : Health;
			_statusBar.SetValue(Health / (float)HEALTH_MAX_I);
			PawnLastHit = Time.realtimeSinceStartup;
		}

		public void Release()
		{
			_statusBar.SetForNobody();
		}
	}
}