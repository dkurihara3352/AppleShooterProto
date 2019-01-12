using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ITriggerMotorMechanismWaypointEventAdaptor: IWaypointEventAdaptor{
		ITriggerMotorMechanismWaypointEvent GetTriggerMotorMechanismWaypointEvent();
		Vector3 GetMechanismPosition();
	 	float GetEventPoint();
	}
	public class TriggerMotorMechanismWaypointEventAdaptor: SlickBowShootingMonoBehaviourAdaptor, ITriggerMotorMechanismWaypointEventAdaptor{
		public override void SetUp(){
			thisWaypointEvent = CreateTriggerMotorMechanismWaypointEvent();
		}
		ITriggerMotorMechanismWaypointEvent thisWaypointEvent;
		public ITriggerMotorMechanismWaypointEvent GetTriggerMotorMechanismWaypointEvent(){
			return thisWaypointEvent;
		}
		public IWaypointEvent GetWaypointEvent(){
			return thisWaypointEvent;
		}
		ITriggerMotorMechanismWaypointEvent CreateTriggerMotorMechanismWaypointEvent(){
			TriggerMotorMechaismWaypointEvent.IConstArg arg = new TriggerMotorMechaismWaypointEvent.ConstArg(eventPoint);
			return new TriggerMotorMechaismWaypointEvent(arg);
		}
		[Range(0f, 1f)]
		public float eventPoint;
		public float GetEventPoint(){
			return eventPoint;
		}
		public override void SetUpReference(){
			ITriggeredMotorMechanism mechanism = mechanismAdaptor.GetTriggeredMotorMechanism();
			thisWaypointEvent.SetTriggeredMotorMechanism(mechanism);
		}
		public TriggeredMotorMechanismAdaptor mechanismAdaptor;
		public Vector3 GetMechanismPosition(){
			return mechanismAdaptor.GetPosition();
		}
	}
}

