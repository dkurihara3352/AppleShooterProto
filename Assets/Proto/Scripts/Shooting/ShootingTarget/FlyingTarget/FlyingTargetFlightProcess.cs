using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IFlyingTargetFlightProcess: IProcess{}
	public class FlyingTargetFlightProcess : AbsProcess, IFlyingTargetFlightProcess {

		public FlyingTargetFlightProcess(
			IConstArg arg
		): base(arg){
			thisFlyingTarget = arg.flyingTarget;
			thisInitialVelocity = arg.initialVelocity;
			thisOriginalDistanceThreshold = arg.distanceThreshold;
			thisCurrentVelocity = thisInitialVelocity;
			thisMaxSpeed = arg.speed;
		}
		readonly IFlyingTarget thisFlyingTarget;
		readonly Vector3 thisInitialVelocity;
		float thisOriginalDistanceThreshold;
		float thisDistanceThreshold;
		Vector3 thisCurrentVelocity;
		protected override void RunImple(){
			thisFlyingTarget.SetUpWaypoints();
			thisPrevPos = thisFlyingTarget.GetPosition();
			thisDistanceThreshold = thisOriginalDistanceThreshold;
		}
		Vector3 thisPrevPos;
		readonly float thisMaxSpeed = 10f;
		float thisCurrentSpeed = 0f;

		protected override void UpdateProcessImple(float deltaT){
			Vector3 curPos = thisFlyingTarget.GetPosition();
			IFlyingTargetWaypoint currentWaypoint = thisFlyingTarget.GetCurrentWaypoint();
			
			Vector3 posDif = currentWaypoint.GetPosition() - curPos;
			if(posDif.sqrMagnitude <= thisDistanceThreshold * thisDistanceThreshold){
				thisFlyingTarget.SetUpNextWaypoint();
				thisDistanceThreshold = thisOriginalDistanceThreshold;
			}
			Vector3 lookDir = thisFlyingTarget.GetForwardDirection();

			float targetSpeed = CalculateTargetSpeed(posDif, lookDir);
			float actualSpeedDelta = (targetSpeed - thisCurrentSpeed) * deltaT;
			float actualSpeed = thisCurrentSpeed + actualSpeedDelta;
			// float speed = CalculateTargetSpeed(posDif, lookDir);

			Vector3 deltaPos = lookDir * actualSpeed * deltaT /* * posDif.magnitude * .1f */;
			Vector3 newPosition = curPos + deltaPos;

			thisFlyingTarget.SetPosition(newPosition);
			thisCurrentSpeed = actualSpeed;
			thisDistanceThreshold += deltaT * distanceThreshDelta;

			thisFlyingTarget.SetDistanceThresholdForGizmo(thisDistanceThreshold);
		}
		float distanceThreshDelta = .3f;
		float CalculateTargetSpeed(
			Vector3 posDif,
			Vector3 lookDir
		){
			float dot = Vector3.Dot(
				posDif.normalized, lookDir
			);
			// if(dot < 0f)
			// 	dot = 0f;
			dot = (dot + 1f) /2f;
			return thisMaxSpeed * dot;
		}
		/*  */
		public new interface IConstArg: AbsProcess.IConstArg{
			IFlyingTarget flyingTarget{get;}
			Vector3 initialVelocity{get;}
			float distanceThreshold{get;}
			float speed{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IFlyingTarget flyingTarget,
				Vector3 initialVelocity,
				float distanceThreshold,
				float speed,


				IProcessManager processManager
			): base(
				processManager
			){
				thisFlyingTarget = flyingTarget;
				thisInitialVelocity = initialVelocity;
				thisDistanceThreshold = distanceThreshold;
				thisSpeed = speed;
			}
			readonly IFlyingTarget thisFlyingTarget;
			public IFlyingTarget flyingTarget{get{return thisFlyingTarget;}}
			readonly Vector3 thisInitialVelocity;
			public Vector3 initialVelocity{get{return thisInitialVelocity;}}
			readonly float thisDistanceThreshold;
			public float distanceThreshold{get{return thisDistanceThreshold;}}
			readonly float thisSpeed;
			public float speed{get{return thisSpeed;}}
		}
	}
}
