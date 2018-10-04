using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPrintStringWaypointEventAdaptor: IMonoBehaviourAdaptor, IWaypointEventAdaptor{
	}
	public class PrintStringWaypointEventAdaptor : MonoBehaviourAdaptor, IPrintStringWaypointEventAdaptor {
		public string stringToPrint;
		public float eventPoint;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public override void SetUp(){
			IPrintStringWaypointEventConstArg arg = new PrintStringWaypointEventConstArg(
				stringToPrint,
				eventPoint
			);
			thisWaypointEvent = new PrintStringWaypointEvent(arg);
		}
		IPrintStringWaypointEvent thisWaypointEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisWaypointEvent;
		}
		public override void SetUpReference(){
			IWaypointsFollower follower = waypointsFollowerAdaptor.GetWaypointsFollower();
			thisWaypointEvent.SetFollower(follower);
		}

	}
}

