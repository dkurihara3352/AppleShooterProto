using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public class AimDoneMarker : MonoBehaviour {

		public void CallShootingManagerMarkAimDone(){
			IShootingManager manager = shootingManagerAdaptor.GetShootingManager();
			manager.MarkAimDone();
		}
		public ShootingManagerAdaptor shootingManagerAdaptor;
	}
}
