using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFollowWaypointProcess: IProcess{
		float GetNormalizedPositionOnCurve();
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
			thisCycleManager = arg.cycleManager;
			thisWaypointEventManager = new WaypointEventManager();
			SetNewCurve(
				arg.initialCurve,
				0f
			);
			if(thisCycleManager != null)
				thisWaypointEventManager.SetNewCurve(arg.initialCurve);
		}
		readonly IWaypointsFollower thisFollower;
		readonly float thisSpeed;
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
		readonly IWaypointCurveCycleManager thisCycleManager;
		readonly IWaypointEventManager thisWaypointEventManager;
		


		IWaypointCurve thisCurrentCurve;
		void SetNewCurve(
			IWaypointCurve curve,
			float initialTime
		){
			thisCurrentCurve = curve;
			thisFollower.SetWaypointCurve(curve);
			thisRequiredTimeForCurrentCurve = curve.GetTotalDistance() / thisSpeed;
			thisTotalElapsedTimeOnCurrentCurve = initialTime;
			thisWaypointEventManager.SetNewCurve(curve);
		}
		float thisTotalElapsedTimeOnCurrentCurve = 0f;
		float thisRequiredTimeForCurrentCurve;
		protected override void UpdateProcessImple(float deltaT){
			thisTotalElapsedTimeOnCurrentCurve += deltaT;
			if(RequiredTimeForCurrentCurveHasPassed()){
				if(thisCycleManager != null){
					IWaypointCurve nextCurve = thisCycleManager.GetNextCurve(thisCurrentCurve);
					if(nextCurve != null){
						float residualTime = thisTotalElapsedTimeOnCurrentCurve - thisRequiredTimeForCurrentCurve;
						SetNewCurve(
							nextCurve,
							residualTime
						);
						thisCycleManager.CycleCurve();
						MoveFollower();
					}else{
						Expire();
					}
				}else
					Expire();
			}else{
				MoveFollower();
			}
		}
		bool RequiredTimeForCurrentCurveHasPassed(){
			if(thisTotalElapsedTimeOnCurrentCurve >= thisRequiredTimeForCurrentCurve)
				return true;
			else
				return false;
		}
		void MoveFollower(){
			float totalDistanceCoveredInCurrentCurve = thisTotalElapsedTimeOnCurrentCurve * thisSpeed;
			int ceilingIndex = thisCurrentCurve.GetIndexOfCeilingCurvePoint(totalDistanceCoveredInCurrentCurve);
			float normalizedPositionBetweenPoints = thisCurrentCurve.GetNormalizedPositionBetweenPoints(
				ceilingIndex,
				totalDistanceCoveredInCurrentCurve
			);
			Vector3 targetPosition = thisCurrentCurve.CalculatePositionOnCurve(
				ceilingIndex,
				normalizedPositionBetweenPoints
			);
			Vector3 targetForwardDirection = thisCurrentCurve.CalculateForwardDirectionOnCurve(
				ceilingIndex,
				normalizedPositionBetweenPoints
			);
			Vector3 targetUpDirection = thisCurrentCurve.CalculateUpDirectionOnCurve(
				ceilingIndex,
				normalizedPositionBetweenPoints
			);
			thisFollower.SetPosition(targetPosition);
			thisFollower.LookAt(
				targetForwardDirection,
				targetUpDirection
			);
			thisWaypointEventManager.CheckForWaypointEvent(GetNormalizedPositionOnCurve());
		}
		public float GetNormalizedPositionOnCurve(){
			return thisTotalElapsedTimeOnCurrentCurve/ thisRequiredTimeForCurrentCurve;
		}
	}

	
	public interface IFollowWaypointProcessConstArg: IProcessConstArg{
		IWaypointsFollower follower{get;}
		float speed{get;}
		int processOrder{get;}
		IWaypointCurve initialCurve{get;}
		IWaypointCurveCycleManager cycleManager{get;}
	}
	public struct FollowWaypointProcessConstArg: IFollowWaypointProcessConstArg{
		public FollowWaypointProcessConstArg(
			IProcessManager processManager,
			IWaypointsFollower follwer,
			float speed,
			int processOrder,
			IWaypointCurve initialCurve,
			IWaypointCurveCycleManager waypointsManager
		){
			thisProcessManager = processManager;
			thisFollower = follwer;
			thisSpeed = speed;
			thisProcessOrder = processOrder;
			thisInitialCurve = initialCurve;
			thisWaypointsManager = waypointsManager;
		}
		readonly IProcessManager thisProcessManager;
		public IProcessManager processManager{get{return thisProcessManager;}}
		readonly IWaypointsFollower thisFollower;
		public IWaypointsFollower follower{get{return thisFollower;}}
		readonly float thisSpeed;
		public float speed{get{return thisSpeed;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
		readonly IWaypointCurve thisInitialCurve;
		public IWaypointCurve initialCurve{
			get{return thisInitialCurve;}
		}
		readonly IWaypointCurveCycleManager thisWaypointsManager;
		public IWaypointCurveCycleManager cycleManager{get{return thisWaypointsManager;}}
	}
	
}
