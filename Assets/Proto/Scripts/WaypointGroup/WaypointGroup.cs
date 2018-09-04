using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IWaypointGroup{
		List<IWaypoint> GetWaypoints();
		IWaypoint GetFirstWaypoint();
		IWaypoint GetLastWaypoint();
		IWaypointConnection GetConnection();
		void SetUpWaypoints(float speed);
		void Connect(IWaypointGroup prevWaypointGroup);
	}
	public class WaypointGroup : IWaypointGroup {
		public WaypointGroup(
			IWaypointGroupAdaptor adaptor,
			IWaypointsManager manager,
			IWaypointConnection connection, 
			List<IWaypointAdaptor> childWaypointAdaptors
		){
			thisAdaptor = adaptor;
			thisManager = manager;
			thisConnection = connection;
			thisChildWaypointAdaptors = childWaypointAdaptors;
		}
		readonly IWaypointGroupAdaptor thisAdaptor;
		readonly IWaypointsManager thisManager;
		readonly List<IWaypointAdaptor> thisChildWaypointAdaptors;
		List<IWaypoint> thisWaypoints;
		public void SetUpWaypoints(float speed){
			SetUpWaypointsOnAllAdaptors();
			List<IWaypoint> waypoints = GetWaypointsFromAdaptors();
			CacheDistanceOnAllWaypoints(waypoints);
			CacheRequiredTimeOnAllWaypoints(
				waypoints,
				speed
			);
			thisWaypoints = waypoints;
		}
		void SetUpWaypointsOnAllAdaptors(){
			foreach(IWaypointAdaptor adaptor in thisChildWaypointAdaptors){
				adaptor.SetWaypointManager(
					thisManager
				);
				adaptor.CreateAndSetWaypoint();
			}
		}
		List<IWaypoint> GetWaypointsFromAdaptors(){
			List<IWaypoint> result = new List<IWaypoint>();
			foreach(IWaypointAdaptor adaptor in thisChildWaypointAdaptors){
				result.Add(adaptor.GetWaypoint());
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
			IWaypointConnection connection;
			if(prevWaypointGroup != null){
				connection = prevWaypointGroup.GetConnection();
			}else{
				connection = GetInitialConnection();
			}
			this.SetPosition(connection.position);
			this.SetYRotation(connection.yRotation);
			UpdateConnection();
		}
		IWaypointConnection GetInitialConnection(){
			return thisManager.GetInitialConnection();
		}
		void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		void SetYRotation(float yRotation){
			thisAdaptor.SetYRotation(yRotation);
		}
		void UpdateConnection(){
			Vector3 newConnectionPosition = thisAdaptor.GetConnectionPosition();
			thisConnection.SetPosition(newConnectionPosition);
			float newConnectionEulerY = thisAdaptor.GetConnectionEulerY();
			thisConnection.SetYRotation(newConnectionEulerY);
		}
	}
	public interface IWaypointConnection{
		Vector3 position{get;}
		float yRotation{get;}
		void SetPosition(Vector3 position);
		void SetYRotation(float yRotation);
	}
	public class WaypointConnection: IWaypointConnection{
		public WaypointConnection(
			Vector3 position,
			float yRotation
		){
			thisPosition = position;
			thisYRotation = yRotation;
		}
		Vector3 thisPosition;
		public Vector3 position{get{return thisPosition;}}
		public void SetPosition(Vector3 position){
			thisPosition = position;
		}
		float thisYRotation;
		public float yRotation{get{return thisYRotation;}}
		public void SetYRotation(float yRotation){
			thisYRotation = yRotation;
		}
	}
}
