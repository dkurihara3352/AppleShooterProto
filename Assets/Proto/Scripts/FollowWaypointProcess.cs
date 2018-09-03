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
		readonly float diffThreshold = .1f;
		readonly float thisSpeed;
		IWaypoint thisTargetWaypoint;
		Vector3 thisTargetPosition{
			get{return thisTargetWaypoint.GetPosition();}
		}
		void SetNewTargetWaypoint(){
			thisFollower.SetNextWaypoint();
			IWaypoint nextWaypoint = thisFollower.GetCurrentWaypoint();
			if(nextWaypoint != null){
				CacheDirection(
					nextWaypoint
				);
				thisTargetWaypoint = nextWaypoint;
			}
			else
				this.Expire();
		}
		Vector3 thisDirection;
		void CacheDirection(
			IWaypoint nextWaypoint
		){
			Vector3 prevPos = new Vector3();
			if(thisTargetWaypoint != null)
				prevPos = thisTargetWaypoint.GetPosition();
			else
				prevPos = thisFollower.GetPosition();

			Vector3 targetPos = nextWaypoint.GetPosition();

			thisDirection = (targetPos - prevPos).normalized;
		}
		protected override void UpdateProcessImple(float deltaT){
			if(CurPosIsCloseEnoughToTargetPos(
				thisFollower.GetPosition(),
				thisTargetPosition
			)){
				SetNewTargetWaypoint();
			}
			MoveFollowerToTargetWaypoint(deltaT);
		}
		bool CurPosIsCloseEnoughToTargetPos(
			Vector3 currentPosition,
			Vector3 targetPosition
		){
			Vector3 deltaPositioin = targetPosition - currentPosition;
			if(deltaPositioin.sqrMagnitude <= diffThreshold * diffThreshold)
				return true;
			return false;
		}
		void MoveFollowerToTargetWaypoint(float deltaTime){
			Vector3 deltaPos = (thisDirection * thisSpeed) * deltaTime;
			Vector3 newPosition = thisFollower.GetPosition() + deltaPos;
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
