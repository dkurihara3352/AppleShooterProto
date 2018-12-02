using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IGlidingTargetStartGlideWaypointEvent: IWaypointEvent{
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		void SetGlidingTargetSpawnPoint(IGlidingTargetSpawnPoint point);
	}
	public class GlidingTargetStartGlideWaypointEvent: AbsWaypointEvent, IGlidingTargetStartGlideWaypointEvent{
		public GlidingTargetStartGlideWaypointEvent(
			IConstArg arg
		): base(arg){}
		IGlidingTargetReserve thisGlidingTargetReserve;
		public void SetGlidingTargetReserve(IGlidingTargetReserve reserve){
			thisGlidingTargetReserve = reserve;
		}
		IGlidingTargetSpawnPoint thisGlidingTargetSpawnPoint;
		public void SetGlidingTargetSpawnPoint(IGlidingTargetSpawnPoint point){
			thisGlidingTargetSpawnPoint = point;
		}
		protected override void ExecuteImple(){
			thisGlidingTargetReserve.ActivateShootingTargetAt(
				thisGlidingTargetSpawnPoint
			);
		}
		public override string GetName(){
			return "GlidingTargetStartGlideWPEvent";
		}
	}
}

