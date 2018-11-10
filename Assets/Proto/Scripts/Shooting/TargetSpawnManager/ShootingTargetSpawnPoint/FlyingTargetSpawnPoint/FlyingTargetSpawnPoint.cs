using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFlyingTargetSpawnPoint: IShootingTargetSpawnPoint{
		void SetWaypointManager(IFlyingTargetWaypointManager manager);
		IFlyingTargetWaypointManager GetWaypointManager();
	}
	public class FlyingTargetSpawnPoint : AbsShootingTargetSpawnPoint, IFlyingTargetSpawnPoint {

		public FlyingTargetSpawnPoint(
			IConstArg arg
		): base(arg){

		}
		IFlyingTargetWaypointManager thisWaypointManager;
		public void SetWaypointManager(IFlyingTargetWaypointManager manager){
			thisWaypointManager = manager;
		}
		public IFlyingTargetWaypointManager GetWaypointManager(){
			return thisWaypointManager;
		}
		public new interface IConstArg: AbsShootingTargetSpawnPoint.IConstArg{}
		public new class ConstArg: AbsShootingTargetSpawnPoint.ConstArg, IConstArg{
			public ConstArg(
				float eventPoint,
				IFlyingTargetSpawnPointAdaptor adaptor
			): base(
				eventPoint,
				adaptor
			){

			}
		}
	}
}
