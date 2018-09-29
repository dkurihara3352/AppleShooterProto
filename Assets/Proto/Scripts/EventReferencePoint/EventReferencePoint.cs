using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IEventReferencePoint{
		void SetInitialWaypointGroup(IWaypointGroup group);
		void SetWaypointsManager(IWaypointsManager waypointsManager);
		void SetInitialWaypointIndex(int index);
		int GetCurrentWaypointGroupIndex();
		float GetNormalizedPositionOnSegment();
		float GetNormalizedPositionInGroup();
	}
	public class EventReferencePoint : IEventReferencePoint {
		public EventReferencePoint(
			IEventReferencePointConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		IWaypointGroup thisCurrentWaypointGroup;
		public void SetInitialWaypointGroup(IWaypointGroup group){
			thisCurrentWaypointGroup = group;
		}
		IWaypointsManager thisWaypointsManager;
		public void SetWaypointsManager(IWaypointsManager waypointsManager){
			thisWaypointsManager = waypointsManager;
		}
		public int GetCurrentWaypointGroupIndex(){
			return thisWaypointsManager.GetWaypointGroupIndex(thisCurrentWaypointGroup);
		}
		readonly IEventReferencePointAdaptor thisAdaptor;
		Vector3 thisPosition{
			get{return thisAdaptor.GetPosition();}
		}
		IWaypoint thisCurrentWaypoint;
		public void SetInitialWaypointIndex(int index){
			thisCurrentWaypoint = thisCurrentWaypointGroup.GetWaypoints()[index];
			thisCurrentWaypoint.StartGizmosDrawing();
		}
		public float GetNormalizedPositionOnSegment(){
			return thisCurrentWaypoint.GetNormalizedPositionOnSegment(thisPosition);
		}
		public float GetNormalizedPositionInGroup(){
			float normalizedPositionOnSegment = GetNormalizedPositionOnSegment();
			while(normalizedPositionOnSegment > 1f){
				thisCurrentWaypoint = GetNextWaypoint();
				normalizedPositionOnSegment = GetNormalizedPositionOnSegment();
			}
			float sumOfAllPrecedingSegments = thisCurrentWaypoint.GetSumOfAllPrecedingSegments();
			float segmentLength = thisCurrentWaypoint.GetSegmentLength();
			float segmentTravelDistance = segmentLength * normalizedPositionOnSegment;
			float traveledDistance = segmentTravelDistance + sumOfAllPrecedingSegments;
			float result = traveledDistance/ thisCurrentWaypointGroup.GetSumOfAllSegments();
			return result;
		}
		IWaypoint GetNextWaypoint(){
			thisCurrentWaypoint.StopGizmosDrawing();
			IWaypoint result;
			if(thisCurrentWaypoint != thisCurrentWaypointGroup.GetLastWaypoint()){
				result = thisCurrentWaypointGroup.GetWaypoints()[thisCurrentWaypoint.GetIndex() + 1];
			}else{
				IWaypointGroup nextWaypointGroup = thisWaypointsManager.GetNextWaypointGroup(
					thisCurrentWaypointGroup
				);
				thisCurrentWaypointGroup = nextWaypointGroup;
				result = nextWaypointGroup.GetWaypoints()[0];
			}
			result.StartGizmosDrawing();
			return result;
		}
	}


	public interface IEventReferencePointConstArg{
		IEventReferencePointAdaptor adaptor{get;}
	}
	public struct EventReferencePointConstArg: IEventReferencePointConstArg{
		public EventReferencePointConstArg(
			IEventReferencePointAdaptor adaptor
		){
			thisAdaptor = adaptor;
		}
		readonly IEventReferencePointAdaptor thisAdaptor;
		public IEventReferencePointAdaptor adaptor{get{return thisAdaptor;}}
	}
}
