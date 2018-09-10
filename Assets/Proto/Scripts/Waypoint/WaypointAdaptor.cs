using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointAdaptor: IMonoBehaviourAdaptor{
		IWaypoint GetWaypoint();
		void SetWaypointsManager(IWaypointsManager manager);
	}
	public class WaypointAdaptor: MonoBehaviourAdaptor, IWaypointAdaptor{

		/* disregard SetUp, it is taken care by group
		 */
		 IWaypointsManager thisWaypointsManager;
		 public void SetWaypointsManager(IWaypointsManager manager){
			 thisWaypointsManager = manager;
		 }
		public override void SetUp(){
			thisWaypoint = new Waypoint(
				this,
				thisWaypointsManager
			);
		}
		IWaypoint thisWaypoint;
		public IWaypoint GetWaypoint(){
			return thisWaypoint;
		}
	}
}
