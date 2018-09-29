using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IEventReferencePointAdaptor: IMonoBehaviourAdaptor{
		IEventReferencePoint GetEventReferencePoint();
	}
	public class EventReferencePointAdaptor : MonoBehaviourAdaptor, IEventReferencePointAdaptor {

		public override void SetUp(){
			IEventReferencePointConstArg arg = new EventReferencePointConstArg(
				this
			);
			thisEventReferencePoint = new EventReferencePoint(arg);
		}
		IEventReferencePoint thisEventReferencePoint;
		public IEventReferencePoint GetEventReferencePoint(){
			return thisEventReferencePoint;
		}
		public WaypointsManager waypointsManager;
		public override void SetUpReference(){

			thisEventReferencePoint.SetWaypointsManager(waypointsManager);
		}
	}
}
