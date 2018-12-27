﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public class AimDoneMarker : MonoBehaviour {

		public void CallShootingManagerMarkAimDone(){
			IShootingManager manager = shootingManagerAdaptor.GetShootingManager();
			manager.MarkAimDone();
		}
		public ShootingManagerAdaptor shootingManagerAdaptor;
	}
}
