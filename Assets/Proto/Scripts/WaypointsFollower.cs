using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollower{
		void SetPosition(Vector3 position);
		Vector3 GetPosition();
		float GetSpeed();
		IWaypoint GetCurrentWaypoint();
		void SetNextWaypoint();
		void SetWaypoints(
			List<IWaypoint> waypoints
		);
		void StartFollowing();
	}
	public class WaypointsFollower: IWaypointsFollower{
		public WaypointsFollower(
			IWaypointsFollowerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisFollowSpeed = arg.followSpeed;
		}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisFollowSpeed;
		public float GetSpeed(){
			return thisFollowSpeed;
		}
		IFollowWaypointProcess thisProcess;
		public void StartFollowing(){
			thisProcess = thisProcessFactory.CreateFollowWaypointProcess(
				this,
				thisFollowSpeed
			);
			thisProcess.Run();
		}
		void StopFollowing(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
		public void SetPosition(
			Vector3 position
		){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		IWaypoint thisCurWaypoint;
		public IWaypoint GetCurrentWaypoint(){
			return thisCurWaypoint;
		}
		List<IWaypoint> thisWaypointList;
		public void SetNextWaypoint(){
			IWaypoint nextWaypoint = null;

			if(thisCurWaypoint == null){
				nextWaypoint = thisWaypointList[0];
			}else{
				int curIndex = thisWaypointList.IndexOf(thisCurWaypoint);
				if(curIndex != thisWaypointList.Count -1)
					nextWaypoint = thisWaypointList[curIndex + 1];
			}
			
			thisCurWaypoint = nextWaypoint;
		}
		public void SetWaypoints(List<IWaypoint> waypoints){
			thisWaypointList = waypoints;
		}
	}


	public interface IWaypointsFollowerConstArg{
		IWaypointsFollowerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float followSpeed{get;}
	}
	public struct WaypointsFollowerConstArg: IWaypointsFollowerConstArg{
		public WaypointsFollowerConstArg(
			IWaypointsFollowerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float followSpeed
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisFollowSpeed = followSpeed;
		}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		public IWaypointsFollowerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisFollowSpeed;
		public float followSpeed{get{return thisFollowSpeed;}}
	}
}

