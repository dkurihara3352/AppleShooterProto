using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace AppleShooterProto{
	public interface ISmoothLookProcess: IProcess{}
	public class SmoothLookProcess : AbsProcess, ISmoothLookProcess{
		public SmoothLookProcess(
			ISmoothLookProcessConstArg arg
		): base(
			arg
		){
			thisTarget = arg.target;
			thisSmoothLooker = arg.smoothLooker;
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisProcessOrder = arg.processOrder;
		}
		readonly IMonoBehaviourAdaptor thisTarget;
		readonly ISmoothLooker thisSmoothLooker;
		readonly float thisSmoothCoefficient;
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
	}
	public interface ISmoothLookProcessConstArg: IProcessConstArg{
		IMonoBehaviourAdaptor target{get;}
		ISmoothLooker smoothLooker{get;}
		float smoothCoefficient{get;}
		int processOrder{get;}
	}
	public class SmoothLookProcessConstArg: ProcessConstArg, ISmoothLookProcessConstArg{
		public SmoothLookProcessConstArg(
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
