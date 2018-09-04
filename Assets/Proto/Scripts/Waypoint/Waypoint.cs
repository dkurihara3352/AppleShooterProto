using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypoint{
		Vector3 GetPosition();
		void CacheDistanceFromPreviousWaypoint(IWaypoint prevWaypoint);
		void CacheRequiredTime(float speed);
		float GetRequiredTime();
		Vector3 GetPreviousWaypointPosition();
	}
	public class Waypoint: IWaypoint{
		public Waypoint(
			IWaypointAdaptor adaptor,
			IWaypointsManager waypointsManager
		){
			thisAdaptor = adaptor;
			thisWaypointsManager = waypointsManager;
			
		}
		readonly IWaypointAdaptor thisAdaptor;
		readonly IWaypointsManager thisWaypointsManager;
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public Vector3 GetPreviousWaypointPosition(){
			return thisPreviousWaypointPosition;
		}
		Vector3 thisPreviousWaypointPosition{
			get{
				if(thisPrevWaypoint != null)
					return thisPrevWaypoint.GetPosition();
				else{
					IWaypointsFollower follower = thisWaypointsManager.GetFollower();
					return follower.GetPosition();
				}
			}
		}
		IWaypoint thisPrevWaypoint;
		float thisDistanceFromPreviousWaypoint;
		public void CacheDistanceFromPreviousWaypoint(
			IWaypoint prevWaypoint
		){
			thisPrevWaypoint = prevWaypoint;
			Vector3 displacement = this.GetPosition() - thisPreviousWaypointPosition;
			thisDistanceFromPreviousWaypoint = displacement.magnitude;
		}
		float thisRequiredTime;
		public float GetRequiredTime(){
			return thisRequiredTime;
		}
		public void CacheRequiredTime(
			float speed
		){
			thisRequiredTime = thisDistanceFromPreviousWaypoint / speed;
		}
	}
}
