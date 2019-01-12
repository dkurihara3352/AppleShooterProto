using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ITriggerMotorMechanismWaypointEvent: IWaypointEvent{
		void SetTriggeredMotorMechanism(ITriggeredMotorMechanism mechanism);
	}
	public class TriggerMotorMechaismWaypointEvent: AbsWaypointEvent, ITriggerMotorMechanismWaypointEvent{
		public TriggerMotorMechaismWaypointEvent(IConstArg arg): base(arg){}
		public override string GetName(){
			return "TriggerMotorMechanismWaypointEvent";
		}
		protected override void ExecuteImple(IWaypointsFollower follower){
			thisTriggeredMotorMechanism.Trigger();
		}
		ITriggeredMotorMechanism thisTriggeredMotorMechanism;
		public void SetTriggeredMotorMechanism(ITriggeredMotorMechanism mechanism){
			thisTriggeredMotorMechanism = mechanism;
		}
	}
}


