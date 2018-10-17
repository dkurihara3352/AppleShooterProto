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
		public PCWaypointsManagerAdaptor pcWaypointsManagerAdaptor;
		public override void SetUpReference(){
			IPCWaypointsManager pcWaypointsManager = pcWaypointsManagerAdaptor.GetPCWaypointsManager();
			thisEvent.SetPCWaypointsManager(pcWaypointsManager);
		}
		protected ITargetSpawnWaypointEvent thisEvent;
		public IWaypointEvent GetWaypointEvent(){
			return thisEvent;
		}
	}
}
