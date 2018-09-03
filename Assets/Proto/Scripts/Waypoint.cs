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
		Vector3 thisPreviousWaypointPosition;
		float thisDistanceFromPreviousWaypoint;
		public void CacheDistanceFromPreviousWaypoint(
			IWaypoint prevWaypoint
		){
			Vector3 prevWaypointPosition = new Vector3();
			if(prevWaypoint == null){
				IWaypointsFollower follower = thisWaypointsManager.GetFollower();
				prevWaypointPosition = follower.GetPosition();
			}else
				prevWaypointPosition = prevWaypoint.GetPosition();

			thisPreviousWaypointPosition = prevWaypointPosition;
			Vector3 displacement = this.GetPosition() - prevWaypointPosition;
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
