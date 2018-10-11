using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ITargetSpawnWaypointEventAdaptor: IWaypointEventAdaptor{
	}
	public class TargetSpawnWaypointEventAdaptor : MonoBehaviourAdaptor, ITargetSpawnWaypointEventAdaptor {
		public float eventPoint;
		public override void SetUp(){
			TargetSpawnWaypointEvent.IConstArg arg = new TargetSpawnWaypointEvent.ConstArg(
				eventPoint
			);
			thisEvent = new TargetSpawnWaypointEvent(arg);
		}
		public WaypointsManagerAdaptor waypointsManagerAdaptor;
		public override void SetUpReference(){
			IWaypointsManager waypointsManager = waypointsManagerAdaptor.GetWaypointsManager();
			thisEvent.SetWaypointsManager(waypointsManager);
		}
		protected ITargetSpawnWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
	}
}
