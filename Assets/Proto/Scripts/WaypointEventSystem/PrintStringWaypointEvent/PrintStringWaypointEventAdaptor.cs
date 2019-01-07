using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPrintStringWaypointEventAdaptor: ISlickBowShootingMonoBehaviourAdaptor, IWaypointEventAdaptor{
	}
	public class PrintStringWaypointEventAdaptor : SlickBowShootingMonoBehaviourAdaptor, IPrintStringWaypointEventAdaptor {
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

