using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointsManager{
		List<IWaypoint> GetWaypoints();
		void SetFollower(IWaypointsFollower follower);
		IWaypointsFollower GetFollower();
	}
	public class WaypointsManager : MonoBehaviour, IWaypointsManager {
		IWaypointsFollower thisFollower;
		public IWaypointsFollower GetFollower(){
			return thisFollower;
		}
		public void SetFollower(IWaypointsFollower follower){
			thisFollower = follower;
		}
		List<IWaypoint> thisWaypoints;
		public Transform waypointParentTrans;
		public void SetUpWaypoints(){
			List<IWaypoint> waypoints = GetWaypointsUnderParent();
			CacheDistanceOnAllWaypoints(waypoints);
			float speed = thisFollower.GetSpeed();
			CacheRequiredTimeOnAllWaypoints(
				waypoints,
				speed
			);
			thisWaypoints = waypoints;
		}
		List<IWaypoint> GetWaypointsUnderParent(){
			List<IWaypoint> result = new List<IWaypoint>();
			int childCount = waypointParentTrans.childCount;
			
			for(int i = 0; i < childCount; i ++){
				Transform child = waypointParentTrans.GetChild(i);
				WaypointAdaptor waypointAdaptor = child.GetComponent<WaypointAdaptor>();
				if(waypointAdaptor != null){
					waypointAdaptor.SetWaypointManager(this);
					waypointAdaptor.CreateAndSetWaypoint();
					result.Add(waypointAdaptor.GetWaypoint());
				}
			}
			return result;
		}
		void CacheDistanceOnAllWaypoints(List<IWaypoint> waypoints){
			IWaypoint prevWaypoint = null;
			foreach(IWaypoint waypoint in waypoints){
				waypoint.CacheDistanceFromPreviousWaypoint(prevWaypoint);
				prevWaypoint = waypoint;
			}
		}
		void CacheRequiredTimeOnAllWaypoints(
			List<IWaypoint> waypoints,
			float speed
		){
			foreach(IWaypoint waypoint in waypoints){
				waypoint.CacheRequiredTime(speed);
			}
		}
		public List<IWaypoint> GetWaypoints(){
			return thisWaypoints;
		}
		
	}
}
