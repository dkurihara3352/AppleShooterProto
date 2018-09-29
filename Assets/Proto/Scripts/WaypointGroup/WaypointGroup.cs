using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointGroup{
		List<IWaypoint> GetWaypoints();
		void SetWaypoints(List<IWaypoint> waypoints);
		IWaypoint GetFirstWaypoint();
		IWaypoint GetLastWaypoint();
		IWaypointConnection GetConnection();
		void CacheTravelData(float speed);
		void Connect(IWaypointGroup prevWaypointGroup);
		void UpdateConnectedWaypointCacheData(
			IWaypointGroup nextGroup, 
			float speed
		);
		void SetPosition(Vector3 position);
		float GetSumOfAllSegments();
	}
	public class WaypointGroup : IWaypointGroup {
		public WaypointGroup(
			IWaypointGroupAdaptor adaptor,
			IWaypointsManager manager,
			IWaypointConnection connection
		){
			thisAdaptor = adaptor;
			thisManager = manager;
			thisConnection = connection;
		}
		readonly IWaypointGroupAdaptor thisAdaptor;
		readonly IWaypointsManager thisManager;
		List<IWaypoint> thisWaypoints;
		public void SetWaypoints(List<IWaypoint> waypoints){
			thisWaypoints = waypoints;
		}
		public void CacheTravelData(float speed){
			CacheIndex(thisWaypoints);
			CacheDistanceOnAllWaypoints(thisWaypoints);
			CacheSumOfAllPrecedingSegmentsOnAllWaypoints(thisWaypoints);
			CacheRequiredTimeOnAllWaypoints(
				thisWaypoints,
				speed
			);
		}
		void CacheIndex(List<IWaypoint> waypoints){
			foreach(IWaypoint waypoint in waypoints){
				waypoint.SetIndex(waypoints.IndexOf(waypoint));
			}
		}
		void CacheSumOfAllPrecedingSegmentsOnAllWaypoints(List<IWaypoint> waypoints){
			float sum = 0f;
			foreach(IWaypoint waypoint in waypoints){
				waypoint.SetSumOfAllPrecedingSegments(sum);
				sum += waypoint.GetSegmentLength();
			}
			this.SetSumOfAllSegments(sum);
		}
		float thisSumOfAllSegments;
		void SetSumOfAllSegments(float sum){
			thisSumOfAllSegments = sum;
		}
		public float GetSumOfAllSegments(){
			return thisSumOfAllSegments;
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
		public IWaypoint GetFirstWaypoint(){
			return thisWaypoints[0];
		}
		public IWaypoint GetLastWaypoint(){
			return thisWaypoints[thisWaypoints.Count - 1];
		}
		readonly IWaypointConnection thisConnection;
		public IWaypointConnection GetConnection(){
			return thisConnection;
		}
		public void Connect(IWaypointGroup prevWaypointGroup){
			IWaypointConnection prevConnection;
			if(prevWaypointGroup != null){
				prevConnection = prevWaypointGroup.GetConnection();
			}else{
				prevConnection = GetInitialConnection();
			}
			this.SetPosition(prevConnection.position);
			this.SetRotation(prevConnection.rotation);
			UpdateConnection();
		}
		IWaypointConnection GetInitialConnection(){
			return thisManager.GetInitialConnection();
		}
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		void UpdateConnection(){
			Vector3 newConnectionPosition = thisAdaptor.GetConnectionPosition();
			thisConnection.SetPosition(newConnectionPosition);
			Quaternion newConnectionRotation = thisAdaptor.GetConnectionRotation();
			thisConnection.SetRotation(newConnectionRotation);
		}
		public void UpdateConnectedWaypointCacheData(IWaypointGroup prevGroup, float speed){
			IWaypoint prevGroupLastWP;
			if(prevGroup != null){
				prevGroupLastWP = prevGroup.GetLastWaypoint();
			}else{//init
				prevGroupLastWP = null;
			}
				IWaypoint thisFirstWP = this.GetFirstWaypoint();
				thisFirstWP.SetPreviousWaypoint(prevGroupLastWP);
				thisFirstWP.CacheDistanceFromPreviousWaypoint(prevGroupLastWP);
				thisFirstWP.CacheRequiredTime(speed);
		}
	}
	public interface IWaypointConnection{
		Vector3 position{get;}
		Quaternion rotation{get;}
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
	}
	public class WaypointConnection: IWaypointConnection{
		public WaypointConnection(
			Vector3 position,
			Quaternion rotation
		){
			thisPosition = position;
			thisRotation = rotation;
		}
		Vector3 thisPosition;
		public Vector3 position{get{return thisPosition;}}
		public void SetPosition(Vector3 position){
			thisPosition = position;
		}
		Quaternion thisRotation;
		public Quaternion rotation{get{return thisRotation;}}
		public void SetRotation(Quaternion rotation){
			thisRotation = rotation;
		}

	}
}
