using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothFollowTargetProcess: IProcess{}
	public class SmoothFollowTargetProcess : AbsProcess, ISmoothFollowTargetProcess {
		public SmoothFollowTargetProcess(
			ISmoothFollowTargetProcessConstArg arg
		): base(
			arg
		){
			thisSmoothFollower = arg.smoothFollower;
			thisTarget = arg.target;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}
		ISmoothFollower thisSmoothFollower;
		ISmoothFollowTargetMBAdaptor thisTarget;
		float thisSmoothCoefficient;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 smoothFollowerPosition =thisSmoothFollower.GetPosition();
			Vector3 targetPosition = thisTarget.GetPosition();
			Vector3 displacement = targetPosition - smoothFollowerPosition;

			Vector3 deltaPosition = CalcDeltaPosition(
				deltaT,
				displacement
			);
			Vector3 newPosition = smoothFollowerPosition + deltaPosition;
			thisSmoothFollower.SetPosition(newPosition);
		}
		Vector3 CalcDeltaPosition(
			float deltaTime,
			Vector3 displacemenet
		){
			return displacemenet * thisSmoothCoefficient * deltaTime;
		}
	}


	public interface ISmoothFollowTargetProcessConstArg: IProcessConstArg{
		ISmoothFollower smoothFollower{get;}
		ISmoothFollowTargetMBAdaptor target{get;}
		float smoothCoefficient{get;}
	}
	public class SmoothFollowTargetProcessConstArg: ProcessConstArg, ISmoothFollowTargetProcessConstArg{
		public SmoothFollowTargetProcessConstArg(
			IProcessManager processManager,

			ISmoothFollower smoothFollower,
			ISmoothFollowTargetMBAdaptor target,
			float smoothCoefficient
		): base(
			processManager
		){
			thisFollower = smoothFollower;
			thisTarget = target;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly ISmoothFollower thisFollower;
		public ISmoothFollower smoothFollower{get{return thisFollower;}}
		readonly ISmoothFollowTargetMBAdaptor thisTarget;
		public ISmoothFollowTargetMBAdaptor target{get{return thisTarget;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
