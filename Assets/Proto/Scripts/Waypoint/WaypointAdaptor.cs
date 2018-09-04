using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointAdaptor{
		IWaypoint GetWaypoint();
		Vector3 GetPosition();
		void SetWaypointManager(IWaypointsManager manager);
		void CreateAndSetWaypoint();
	}
	public class WaypointAdaptor: MonoBehaviour, IWaypointAdaptor{
		public void SetWaypointManager(IWaypointsManager manager){
			thisWaypointsManager = manager;
		}
		IWaypointsManager thisWaypointsManager;
		public void CreateAndSetWaypoint(){
			thisWaypoint = new Waypoint(
				this,
				thisWaypointsManager
			);
		}
		IWaypoint thisWaypoint;
		public IWaypoint GetWaypoint(){
			return thisWaypoint;
		}
		public Vector3 GetPosition(){
			return this.transform.position;
		}
	}
}
