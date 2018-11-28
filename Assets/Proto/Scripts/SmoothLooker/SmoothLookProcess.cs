﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;
namespace AppleShooterProto{
	public interface ISmoothLookProcess: IProcess{
		void UpdateSmoothCoefficient(float k);
		void UpdateLookAtTarget(IMonoBehaviourAdaptor target);
	}
	public class SmoothLookProcess : AbsProcess, ISmoothLookProcess{
		public SmoothLookProcess(
			IConstArg arg
		): base(
			arg
		){
			thisTarget = arg.target;
			thisSmoothLooker = arg.smoothLooker;
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisProcessOrder = arg.processOrder;
		}
		IMonoBehaviourAdaptor thisTarget;
		public void UpdateLookAtTarget(IMonoBehaviourAdaptor target){
			thisTarget = target;
		}
		readonly ISmoothLooker thisSmoothLooker;
		float thisSmoothCoefficient;
		public void UpdateSmoothCoefficient(float k){
			thisSmoothCoefficient = k;
		}
		float angleThreshold = .1f;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 targetLookAtPosition = thisTarget.GetPosition();
			Vector3 currentLookerPosition = thisSmoothLooker.GetPosition();
			Vector3 displacement = targetLookAtPosition - currentLookerPosition;
			if(displacement.sqrMagnitude >= displacementThreshold * displacementThreshold){
				Quaternion targetLookAtRotation = Quaternion.LookRotation(
					displacement
				);
				Quaternion currentLookRotation = thisSmoothLooker.GetRotation();

				float angle = Quaternion.Angle(
					targetLookAtRotation,
					currentLookRotation
				);

				float t = angle * thisSmoothCoefficient * deltaT;
					
				thisSmoothLooker.RotateToward(targetLookAtRotation, t);
			}

		}
		float displacementThreshold = .05f;
		readonly int thisProcessOrder;
		public override int GetProcessOrder(){
			return thisProcessOrder;
		}
		public new interface IConstArg: AbsProcess.IConstArg{
			IMonoBehaviourAdaptor target{get;}
			ISmoothLooker smoothLooker{get;}
			float smoothCoefficient{get;}
			int processOrder{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,

				IMonoBehaviourAdaptor target,
				ISmoothLooker looker,
				float smoothCoefficient,
				int processOrder
			): base(
				processManager
			){
				thisTarget = target;
				thisSmoothLooker = looker;
				thisSmoothCoefficient = smoothCoefficient;
				thisProcessOrder = processOrder;
			}
			readonly IMonoBehaviourAdaptor thisTarget;
			public IMonoBehaviourAdaptor target{get{return thisTarget;}}
			readonly ISmoothLooker thisSmoothLooker;
			public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
			readonly float thisSmoothCoefficient;
			public float smoothCoefficient{get{return thisSmoothCoefficient;}}
			readonly int thisProcessOrder;
			public int processOrder{get{return thisProcessOrder;}}
		}
	}



}
