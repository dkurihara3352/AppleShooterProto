using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetReserveAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IShootingTargetReserve GetReserve();
		int GetSpawnValue();
		TargetType GetTargetType();
	}
	public abstract class AbsShootingTargetReserveAdaptor : AppleShooterMonoBehaviourAdaptor, IShootingTargetReserveAdaptor {
		protected IShootingTargetReserve thisReserve;
		public IShootingTargetReserve GetReserve(){
			return thisReserve;
		}
		public float reservedSpace;
		public int targetCount;
		public GameStatsTrackerAdaptor gameStatsTrackerAdaptor;
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public DestroyedTargetReserveAdaptor destroyedTargetReserveAdaptor;
		public UnityBase.PopUIReserveAdaptor popUIReserveAdaptor;
		public GameObject shootingTargetPrefab;
		public int GetSpawnValue(){
			IShootingTargetAdaptor adaptor = (IShootingTargetAdaptor)shootingTargetPrefab.GetComponent(typeof(IShootingTargetAdaptor));
			return adaptor.GetSpawnValue();
		}
		public TargetType targetType;
		public TargetType GetTargetType(){
			return targetType;
		}
	}
}
