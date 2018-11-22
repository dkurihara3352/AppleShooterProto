using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPrintStringWaypointEventAdaptor: IAppleShooterMonoBehaviourAdaptor, IWaypointEventAdaptor{
	}
	public class PrintStringWaypointEventAdaptor : AppleShooterMonoBehaviourAdaptor, IPrintStringWaypointEventAdaptor {
		public string stringToPrint;
		public float eventPoint;
		public WaypointsFollowerAdaptor waypointsFollowerAdaptor;
		public override void SetUp(){
			PrintStringWaypointEvent.IConstArg arg = new PrintStringWaypointEvent.ConstArg(
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

