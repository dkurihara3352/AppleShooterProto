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
		void SetPreviousWaypoint(IWaypoint prev);

		void SetIndex(int index);
		int GetIndex();
		float GetSegmentLength();
		float GetSumOfAllPrecedingSegments();
		void SetSumOfAllPrecedingSegments(float sum);
		float GetNormalizedPositionOnSegment(Vector3 position);
		void StartGizmosDrawing();
		void StopGizmosDrawing();
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
		public void SetPreviousWaypoint(IWaypoint prev){
			thisPrevWaypoint = prev;
		}
		float thisDistanceFromPreviousWaypoint;
		public float GetSegmentLength(){
			return thisDistanceFromPreviousWaypoint;
		}
		public float GetSumOfAllPrecedingSegments(){
			return thisSumOfAllPrecedingSegments;
		}
		float thisSumOfAllPrecedingSegments;
		public void SetSumOfAllPrecedingSegments(float sum){
			thisSumOfAllPrecedingSegments = sum;
		}
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

		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
		bool initPosIsEvaluated = false;
		Vector3 thisInitPos;
		public float GetNormalizedPositionOnSegment(Vector3 position){
			Vector3 thisPosition = this.GetPosition();
			Vector3 prevWPPosition;
			if(thisPrevWaypoint != null)
			 	prevWPPosition = thisPreviousWaypointPosition;
			else{
				if(!initPosIsEvaluated){
					thisInitPos = position;
					initPosIsEvaluated = true;
				}
				prevWPPosition = thisInitPos;
			}
			Vector3 segment = thisPosition - prevWPPosition;
			float segmentLength = segment.magnitude;
			// Vector3 project = /* Vector3.Project(position, segment); */Vector3.Dot(segment, position)/ position.magnitude;
			// Vector3 prevToProject = project - prevWPPosition;
			float projectLength = Vector3.Dot(position - prevWPPosition, segment)/ segmentLength;
			Vector3 project = prevWPPosition + segment.normalized * projectLength;

			thisAdaptor.UpdateGizmosFields(
				prevWPPosition,
				thisPosition,
				position,
				project
			);

			return projectLength/ segmentLength;
		}
		public void StartGizmosDrawing(){
			thisAdaptor.ToggleGizmosDrawing(true);
		}
		public void StopGizmosDrawing(){
			thisAdaptor.ToggleGizmosDrawing(false);
		}
	}
}
