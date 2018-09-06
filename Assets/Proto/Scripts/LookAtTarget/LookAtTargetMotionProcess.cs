using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ILookAtTargetMotionProcess: IProcess{}
	public class LookAtTargetMotionProcess: AbsProcess, ILookAtTargetMotionProcess{
		public LookAtTargetMotionProcess(
			ILookAtTargetMotionProcessConstArg arg
		):base(
			arg
		){
			thisSmoothLooker = arg.smoothLooker;
			thisLookAtTarget = arg.lookAtTarget;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}
		readonly ISmoothLooker thisSmoothLooker;
		readonly ILookAtTarget thisLookAtTarget;
		readonly float thisSmoothCoefficient;
		Vector3 prevPosition;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 lookerCurPosition = thisSmoothLooker.GetPosition();
			Vector3 smoothLookerMovingDirection = (lookerCurPosition - prevPosition).normalized;
			Vector3 currentTargetDirection = thisLookAtTarget.GetDirection();
			Vector3 directionDiff = smoothLookerMovingDirection - currentTargetDirection;
			Vector3 deltaDirection = directionDiff * thisSmoothCoefficient * deltaT;
			Vector3 newDirection = currentTargetDirection + deltaDirection;
			thisLookAtTarget.SetDirection(newDirection);
			prevPosition = lookerCurPosition;
		}
	}
	public interface ILookAtTargetMotionProcessConstArg: IProcessConstArg{
		ILookAtTarget lookAtTarget{get;}
		ISmoothLooker smoothLooker{get;}
		float smoothCoefficient{get;}
	}
	public class LookAtTargetMotionProcessConstArg: ProcessConstArg, ILookAtTargetMotionProcessConstArg{
		public LookAtTargetMotionProcessConstArg(
			IProcessManager processManager,

			ILookAtTarget lookAtTarget,
			ISmoothLooker smoothLooker,
			float smoothCoefficient
		): base(
			processManager
		){
			thisLookAtTarget = lookAtTarget;
			thisSmoothLooker = smoothLooker;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly ILookAtTarget thisLookAtTarget;
		public ILookAtTarget lookAtTarget{get{return thisLookAtTarget;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}

