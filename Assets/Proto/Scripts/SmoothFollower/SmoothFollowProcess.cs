﻿using System.Collections;
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
			thisProcessOrder = arg.processOrder;
		}
		ISmoothFollower thisSmoothFollower;
		IMonoBehaviourAdaptor thisTarget;
		float thisSmoothCoefficient;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 smoothFollowerPosition = thisSmoothFollower.GetPosition();
			Vector3 targetPosition = thisTarget.GetPosition();
			Vector3 displacement = targetPosition - smoothFollowerPosition;
			Vector3 newPosition;
			if(DisplacementIsSmallEnough(displacement))
				newPosition = targetPosition;
			else{
				Vector3 deltaPosition = CalcDeltaPosition(
					deltaT,
					displacement
				);
				newPosition = smoothFollowerPosition + deltaPosition;
			}
			thisSmoothFollower.SetPosition(newPosition);
		}
		float displacemenetThreshold = .05f;
		bool DisplacementIsSmallEnough(Vector3 displacement){
			if(displacement.sqrMagnitude <= displacemenetThreshold * displacemenetThreshold)
				return true;
			return false;
		}
		Vector3 CalcDeltaPosition(
			float deltaTime,
			Vector3 displacemenet
		){
			Vector3 deltaPosition = displacemenet * thisSmoothCoefficient * deltaTime;
			if(deltaPosition.sqrMagnitude > displacemenet.sqrMagnitude)
				deltaPosition = displacemenet;
			return deltaPosition;
		}
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
	}


	public interface ISmoothFollowTargetProcessConstArg: IProcessConstArg{
		ISmoothFollower smoothFollower{get;}
		IMonoBehaviourAdaptor target{get;}
		float smoothCoefficient{get;}
		int processOrder{get;}
	}
	public class SmoothFollowTargetProcessConstArg: ProcessConstArg, ISmoothFollowTargetProcessConstArg{
		public SmoothFollowTargetProcessConstArg(
			IProcessManager processManager,

			ISmoothFollower smoothFollower,
			IMonoBehaviourAdaptor target,
			float smoothCoefficient,
			int processOrder

		): base(
			processManager
		){
			thisFollower = smoothFollower;
			thisTarget = target;
			thisSmoothCoefficient = smoothCoefficient;
			thisProcessOrder = processOrder;
		}
		readonly ISmoothFollower thisFollower;
		public ISmoothFollower smoothFollower{get{return thisFollower;}}
		readonly IMonoBehaviourAdaptor thisTarget;
		public IMonoBehaviourAdaptor target{get{return thisTarget;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}
