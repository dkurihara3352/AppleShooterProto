using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IGlidingTargetReserve: ISceneObjectReserve<IGlidingTarget>, IShootingTargetReserve{
		IGlidingTarget[] GetGlidingTargets();
	}
	public class GlidingTargetReserve: AbsShootingTargetReserve<IGlidingTarget>, IGlidingTargetReserve{
		public GlidingTargetReserve(
			IConstArg arg
		): base(
			arg
		){
		}
		public override void ActivateShootingTargetAt(IShootingTargetSpawnPoint point){
			IGlidingTargetSpawnPoint typedPoint = (IGlidingTargetSpawnPoint)point;
			IGlidingTarget nextTarget = GetNext();
			nextTarget.ActivateAt(typedPoint);
		}
		public IGlidingTarget[] GetGlidingTargets(){
			return thisSceneObjects;
		}
		public override TargetType GetTargetType(){
			return TargetType.Glider;
		}
	}
}

