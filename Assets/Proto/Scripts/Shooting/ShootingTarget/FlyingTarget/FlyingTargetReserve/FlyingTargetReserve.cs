using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IFlyingTargetReserve: ISceneObjectReserve<IFlyingTarget>, IShootingTargetReserve{
		IFlyingTarget[] GetFlyingTargets();
	}
	public class FlyingTargetReserve : AbsShootingTargetReserve<IFlyingTarget>, IFlyingTargetReserve {
		public FlyingTargetReserve(
			IConstArg arg
		): base(arg){

		}
		public override void ActivateShootingTargetAt(IShootingTargetSpawnPoint point){
			IFlyingTargetWaypointManager waypointManager = ((IFlyingTargetSpawnPoint)point).GetWaypointManager();
			IFlyingTarget target = GetNext();
			target.ActivateAt(waypointManager);
		}
		public IFlyingTarget[] GetFlyingTargets(){
			return thisSceneObjects;
		}
		public override  TargetType GetTargetType(){
			return TargetType.Flyer;
		}
	}
}
