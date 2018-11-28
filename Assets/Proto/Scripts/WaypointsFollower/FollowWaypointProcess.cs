using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFollowWaypointProcess: IProcess{
		float GetNormalizedPositionOnCurve();
		void SetTimeScale(float timeScale);
		float GetTimeScale();
	}
	public class FollowWaypointProcess: AbsProcess, IFollowWaypointProcess{
		public FollowWaypointProcess(
			IConstArg arg
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
				arg.initialTime
			);
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
			float initialEventPoint = 0f;
			if(initialTime != 0f)
				initialEventPoint = initialTime/thisRequiredTimeForCurrentCurve;
			thisWaypointEventManager.SetInitialEventPoint(initialEventPoint);
		}
		float thisTotalElapsedTimeOnCurrentCurve = 0f;
		float thisRequiredTimeForCurrentCurve;
		float thisTimeScale = 1f;
		public void SetTimeScale(float timeScale){
			thisTimeScale = timeScale;
		}
		public float GetTimeScale(){
			return thisTimeScale;
		}
		protected override void UpdateProcessImple(float deltaT){
			thisTotalElapsedTimeOnCurrentCurve += deltaT * thisTimeScale;
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
			float normalizedTime = thisTotalElapsedTimeOnCurrentCurve/ thisRequiredTimeForCurrentCurve;
			Vector3 targetPosition;
			Vector3 targetForwardDirection;
			Vector3 targetUpDirection;

			thisCurrentCurve.OutputFollowData(
				normalizedTime,
				out targetPosition,
				out targetForwardDirection,
				out targetUpDirection
			);

			thisFollower.SetPosition(targetPosition);
			thisFollower.LookAt(
				targetForwardDirection,
				targetUpDirection
			);
			thisWaypointEventManager.CheckForWaypointEvent(normalizedTime);

			thisFollower.SetElapsedTimeOnCurrentCurve(thisTotalElapsedTimeOnCurrentCurve);
		}
		public float GetNormalizedPositionOnCurve(){
			return thisTotalElapsedTimeOnCurrentCurve/ thisRequiredTimeForCurrentCurve;
		}


		public new interface IConstArg: AbsProcess.IConstArg{
			IWaypointsFollower follower{get;}
			float speed{get;}
			int processOrder{get;}
			IWaypointCurve initialCurve{get;}
			IWaypointCurveCycleManager cycleManager{get;}
			float initialTime{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				IWaypointsFollower follwer,
				float speed,
				int processOrder,
				IWaypointCurve initialCurve,
				IWaypointCurveCycleManager waypointsManager,

				float initialTime
			): base(
				processManager
			){
				thisFollower = follwer;
				thisSpeed = speed;
				thisProcessOrder = processOrder;
				thisInitialCurve = initialCurve;
				thisWaypointsManager = waypointsManager;

				thisInitialTime = initialTime;
			}
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

			readonly float thisInitialTime;
			public float initialTime{get{return thisInitialTime;}}
		}
	}

	
	
}
