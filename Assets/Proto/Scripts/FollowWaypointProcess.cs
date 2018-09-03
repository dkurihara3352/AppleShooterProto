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
			SetNewTargetWaypoint();
		}
		readonly IWaypointsFollower thisFollower;
		readonly float thisSpeed;
		IWaypoint thisTargetWaypoint;
		float elapsedTimeForCurrentWaypoint;
		float requiredTimeForCurrentWaypoint;
		Vector3 thisTargetPosition;
		Vector3 thisInitPosition;
		void SetNewTargetWaypoint(){
			thisFollower.SetNextWaypoint();
			IWaypoint nextWaypoint = thisFollower.GetCurrentWaypoint();
			if(nextWaypoint != null){
				thisTargetWaypoint = nextWaypoint;
				thisInitPosition = thisTargetWaypoint.GetPreviousWaypointPosition();
				thisTargetPosition = thisTargetWaypoint.GetPosition();
				elapsedTimeForCurrentWaypoint = 0f;
				requiredTimeForCurrentWaypoint = thisTargetWaypoint.GetRequiredTime();
			}
			else
				this.Expire();
		}
		protected override void UpdateProcessImple(float deltaT){
			if(RequiredTimeForCurrentWaypointHasPassed(deltaT))
				SetNewTargetWaypoint();
			MoveFollowerToTargetWaypoint(deltaT);
		}
		bool RequiredTimeForCurrentWaypointHasPassed(float deltaTime){
			elapsedTimeForCurrentWaypoint += deltaTime;
			if(requiredTimeForCurrentWaypoint <= elapsedTimeForCurrentWaypoint)
				return true;
			return false;
		}

		void MoveFollowerToTargetWaypoint(float deltaTime){
			float normalizedTime = elapsedTimeForCurrentWaypoint/ requiredTimeForCurrentWaypoint;
			Vector3 newPosition = Vector3.Lerp(
				thisInitPosition, 
				thisTargetPosition, 
				normalizedTime
			);
			thisFollower.SetPosition(newPosition);
		}
	}

	
	public interface IFollowWaypointProcessConstArg: IProcessConstArg{
		IWaypointsFollower follower{get;}
		float speed{get;}
	}
	public struct FollowWaypointProcessConstArg: IFollowWaypointProcessConstArg{
		public FollowWaypointProcessConstArg(
			IProcessManager processManager,
			IWaypointsFollower follwer,
			float speed
		){
			thisProcessManager = processManager;
			thisFollower = follwer;
			thisSpeed = speed;
		}
		readonly IProcessManager thisProcessManager;
		public IProcessManager processManager{get{return thisProcessManager;}}
		readonly IWaypointsFollower thisFollower;
		public IWaypointsFollower follower{get{return thisFollower;}}
		readonly float thisSpeed;
		public float speed{get{return thisSpeed;}}
	}
	
}
