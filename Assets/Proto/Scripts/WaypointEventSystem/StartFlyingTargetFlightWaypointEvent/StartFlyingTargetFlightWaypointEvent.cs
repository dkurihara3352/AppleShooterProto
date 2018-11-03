using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStartFlyingTargetFlightWaypointEvent: IWaypointEvent{
	
		void SetFlyingTargetWaypointManager(IFlyingTargetWaypointManager manager);
		void SetFlyingTargetReserve(IFlyingTargetReserve reserve);
	}
	public class StartFlyingTargetFlightWaypointEvent : AbsWaypointEvent, IStartFlyingTargetFlightWaypointEvent {
		public StartFlyingTargetFlightWaypointEvent(
			IConstArg arg
		): base(arg){

		}
		IFlyingTargetWaypointManager thisWaypointManager;
		public void SetFlyingTargetWaypointManager(IFlyingTargetWaypointManager manager){
			thisWaypointManager = manager;
		}
		IFlyingTargetReserve thisReserve;
		public void SetFlyingTargetReserve(IFlyingTargetReserve reserve){
			thisReserve = reserve;
		}
		public override void Execute(){
			thisReserve.ActivateFlyingTargetAt(thisWaypointManager);
		}
	}
}
