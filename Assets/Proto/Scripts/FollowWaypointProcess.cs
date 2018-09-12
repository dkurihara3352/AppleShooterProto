using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFollowWaypointProcess: IProcess{
	}
	public class FollowWaypointProcess: AbsProcess, IFollowWaypointProcess{
		public FollowWaypointProcess(
			IFollowWaypointProcessConstArg arg
		): base(
			arg
		){
			thisFollower = arg.follower;
			thisSpeed = arg.speed;
			thisProcessOrder = arg.processOrder;
			SetNewTargetWaypoint();
		}
		readonly IWaypointsFollower thisFollower;
		readonly float thisSpeed;
		protected IWaypoint thisTargetWaypoint;
		protected float thisElapsedTimeForCurrentWaypoint;
		protected float thisRequiredTimeForCurrentWaypoint;
		protected Vector3 thisTargetPosition;
		protected Vector3 thisInitPosition;
		protected void SetNewTargetWaypoint(){
			thisFollower.SetNextWaypoint();
			IWaypoint nextWaypoint = thisFollower.GetCurrentWaypoint();
			if(nextWaypoint != null){
				SetUpWaypoint(nextWaypoint);
			}
			else
				this.Expire();
		}
		protected void SetUpWaypoint(IWaypoint waypoint){
			thisTargetWaypoint = waypoint;
			thisInitPosition = thisTargetWaypoint.GetPreviousWaypointPosition();
			thisTargetPosition = thisTargetWaypoint.GetPosition();
			thisElapsedTimeForCurrentWaypoint = 0f;
			thisRequiredTimeForCurrentWaypoint = thisTargetWaypoint.GetRequiredTime();
		}
		protected override void UpdateProcessImple(float deltaT){
			if(RequiredTimeForCurrentWaypointHasPassed(deltaT))
				SetNewTargetWaypoint();
			MoveFollowerToTargetWaypoint();
		}
		protected bool RequiredTimeForCurrentWaypointHasPassed(float deltaTime){
			thisElapsedTimeForCurrentWaypoint += deltaTime;
			if(thisRequiredTimeForCurrentWaypoint <= thisElapsedTimeForCurrentWaypoint)
				return true;
			return false;
		}

		protected void MoveFollowerToTargetWaypoint(){
			float normalizedTime = thisElapsedTimeForCurrentWaypoint/ thisRequiredTimeForCurrentWaypoint;
			Vector3 newPosition = Vector3.Lerp(
				thisInitPosition, 
				thisTargetPosition, 
				normalizedTime
			);
			thisFollower.SetPosition(newPosition);
		}
		protected float GetNormalizedTime(){
			return thisElapsedTimeForCurrentWaypoint/ thisRequiredTimeForCurrentWaypoint;
		}
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
	}

	
	public interface IFollowWaypointProcessConstArg: IProcessConstArg{
		IWaypointsFollower follower{get;}
		float speed{get;}
		int processOrder{get;}
	}
	public struct FollowWaypointProcessConstArg: IFollowWaypointProcessConstArg{
		public FollowWaypointProcessConstArg(
			IProcessManager processManager,
			IWaypointsFollower follwer,
			float speed,
			int processOrder
		){
			thisProcessManager = processManager;
			thisFollower = follwer;
			thisSpeed = speed;
			thisProcessOrder = processOrder;
		}
		readonly IProcessManager thisProcessManager;
		public IProcessManager processManager{get{return thisProcessManager;}}
		readonly IWaypointsFollower thisFollower;
		public IWaypointsFollower follower{get{return thisFollower;}}
		readonly float thisSpeed;
		public float speed{get{return thisSpeed;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
	
}
