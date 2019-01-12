using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ITriggerMotorMechanismWaypointEventAdaptor: IWaypointEventAdaptor{
	}
	public class TriggerMotorMechanismWaypointEventAdaptor: SlickBowShootingMonoBehaviourAdaptor, ITriggerMotorMechanismWaypointEventAdaptor{
		public override void SetUp(){
			thisWaypointEvent = CreateTriggerMotorMechanismWaypointEvent();
		}
		ITriggerMotorMechanismWaypointEvent thisWaypointEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisWaypointEvent;
		}
		ITriggerMotorMechanismWaypointEvent CreateTriggerMotorMechanismWaypointEvent(){
			TriggerMotorMechaismWaypointEvent.IConstArg arg = new TriggerMotorMechaismWaypointEvent.ConstArg(eventPoint);
			return new TriggerMotorMechaismWaypointEvent(arg);
		}
		public float eventPoint;
		public override void SetUpReference(){
			ITriggeredMotorMechanism mechanism = mechanismAdaptor.GetTriggeredMotorMechanism();
			thisWaypointEvent.SetTriggeredMotorMechanism(mechanism);
		}
		public TriggeredMotorMechanismAdaptor mechanismAdaptor;
		
	}
}

