﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserveAdaptor: IMonoBehaviourAdaptor{
		IShootingTargetReserve GetReserve();
		float GetSpawnValue();
	}
	public abstract class AbsShootingTargetReserveAdaptor : MonoBehaviourAdaptor, IShootingTargetReserveAdaptor {
		protected IShootingTargetReserve thisReserve;
		public IShootingTargetReserve GetReserve(){
			return thisReserve;
		}
		public float reservedSpace;
		public GameStatsTrackerAdaptor gameStatsTrackerAdaptor;
		public GameObject shootingTargetPrefab;
		public float GetSpawnValue(){
			IShootingTargetAdaptor adaptor = (IShootingTargetAdaptor)shootingTargetPrefab.GetComponent(typeof(IShootingTargetAdaptor));
			return adaptor.GetSpawnValue();
		}
	}
}
