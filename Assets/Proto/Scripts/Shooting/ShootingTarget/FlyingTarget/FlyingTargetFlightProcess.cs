using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IFlyingTargetFlightProcess: IProcess{}
	public class FlyingTargetFlightProcess : AbsProcess, IFlyingTargetFlightProcess {

		public FlyingTargetFlightProcess(
			IConstArg arg
		): base(arg){
			thisFlyingTarget = arg.flyingTarget;
			thisInitialVelocity = arg.initialVelocity;
			thisDistanceThreshold = arg.distanceThreshold;
			thisCurrentVelocity = thisInitialVelocity;
			thisSpeed = arg.speed;
		}
		readonly IFlyingTarget thisFlyingTarget;
		readonly Vector3 thisInitialVelocity;
		float thisDistanceThreshold;
		Vector3 thisCurrentVelocity;
		protected override void RunImple(){
			thisFlyingTarget.SetUpWaypoints();
			thisPrevPos = thisFlyingTarget.GetPosition();
		}
		Vector3 thisPrevPos;
		readonly float thisSpeed = 10f;
		
		protected override void UpdateProcessImple(float deltaT){
			Vector3 curPos = thisFlyingTarget.GetPosition();
			IFlyingTargetWaypoint currentWaypoint = thisFlyingTarget.GetCurrentWaypoint();
			
			Vector3 posDif = currentWaypoint.GetPosition() - curPos;
			if(posDif.sqrMagnitude <= thisDistanceThreshold * thisDistanceThreshold){
				thisFlyingTarget.SetUpNextWaypoint();
			}
			
			Vector3 lookDir = thisFlyingTarget.GetForwardDirection();
			Vector3 deltaPos = lookDir * thisSpeed * deltaT;
			Vector3 newPosition = curPos + deltaPos;
			thisFlyingTarget.SetPosition(newPosition);

		}
		/*  */
		public interface IConstArg: IProcessConstArg{
			IFlyingTarget flyingTarget{get;}
			Vector3 initialVelocity{get;}
			float distanceThreshold{get;}
			float speed{get;}
		}
		public class ConstArg: ProcessConstArg, IConstArg{
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
