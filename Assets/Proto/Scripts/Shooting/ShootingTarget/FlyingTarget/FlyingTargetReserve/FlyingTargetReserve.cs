﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			IFlyingTargetWaypointManager waypointManager = (IFlyingTargetWaypointManager)point;
			IFlyingTarget target = GetNext();
			target.ActivateAt(waypointManager);
		}
		public IFlyingTarget[] GetFlyingTargets(){
			return thisSceneObjects;
		}
		public override TargetType GetTargetType(){
			return TargetType.Flier;
		}
	}
}
