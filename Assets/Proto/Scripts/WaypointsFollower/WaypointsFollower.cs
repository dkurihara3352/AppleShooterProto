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
		void SetWaypointGroup(IWaypointGroup group);
		IWaypointGroup GetCurrentWaypointGroup();
	}
	public class WaypointsFollower: IWaypointsFollower{
		public WaypointsFollower(
			IWaypointsFollowerConstArg arg
		){
			thisWaypointsManager = arg.waypointsManager;
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisFollowSpeed = arg.followSpeed;
			thisProcessOrder = arg.processOrder;
		}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisFollowSpeed;
		readonly IWaypointsManager thisWaypointsManager;
		public float GetSpeed(){
			return thisFollowSpeed;
		}
		IFollowWaypointProcess thisProcess;
		readonly int thisProcessOrder;
		public void StartFollowing(){
			thisProcess = thisProcessFactory.CreateFollowWaypointProcess(
				this,
				thisFollowSpeed,
				thisProcessOrder
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


		/* Waypoint access */
		IWaypointGroup thisCurrentWaypointGroup;
		public void SetWaypointGroup(IWaypointGroup group){
			thisCurrentWaypointGroup = group;
		}
		public IWaypointGroup GetCurrentWaypointGroup(){
			return thisCurrentWaypointGroup;
		}
		IWaypoint thisCurWaypoint;
		public IWaypoint GetCurrentWaypoint(){
			return thisCurWaypoint;
		}
		List<IWaypoint> thisWaypointList;
		public void SetNextWaypoint(){
			IWaypoint nextWaypoint = null;

			if(thisCurWaypoint == null){
				thisWaypointList = thisCurrentWaypointGroup.GetWaypoints();
				nextWaypoint = thisWaypointList[0];
			}else{
				int curIndex = thisWaypointList.IndexOf(thisCurWaypoint);
				if(curIndex != thisWaypointList.Count -1){
					nextWaypoint = thisWaypointList[curIndex + 1];
				}else{
					IWaypointGroup nextWaypointGroup = thisWaypointsManager.GetNextWaypointGroup(thisCurrentWaypointGroup);
					thisWaypointsManager.CycleGroup();
					thisCurrentWaypointGroup = nextWaypointGroup;
					thisWaypointList = nextWaypointGroup.GetWaypoints();
					nextWaypoint = thisWaypointList[0];
				}
			}
			thisCurWaypoint = nextWaypoint;
		}
		public void SetWaypoints(List<IWaypoint> waypoints){
			thisWaypointList = waypoints;
		}
	}


	public interface IWaypointsFollowerConstArg{
		IWaypointsManager waypointsManager{get;}
		IWaypointsFollowerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float followSpeed{get;}
		int processOrder{get;}
	}
	public struct WaypointsFollowerConstArg: IWaypointsFollowerConstArg{
		public WaypointsFollowerConstArg(
			IWaypointsManager waypointsManager,
			IWaypointsFollowerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float followSpeed,
			int processOrder
		){
			thisManager = waypointsManager;
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisFollowSpeed = followSpeed;
			thisProcessOrder = processOrder;
		}
		readonly IWaypointsManager thisManager;
		public IWaypointsManager waypointsManager{get{return thisManager;}}
		readonly IWaypointsFollowerAdaptor thisAdaptor;
		public IWaypointsFollowerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisFollowSpeed;
		public float followSpeed{get{return thisFollowSpeed;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}

