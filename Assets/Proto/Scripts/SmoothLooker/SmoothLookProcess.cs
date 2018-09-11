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
		}
		readonly IMonoBehaviourAdaptor thisTarget;
		readonly ISmoothLooker thisSmoothLooker;
		readonly float thisSmoothCoefficient;
		float angleThreshold = .1f;
		protected override void UpdateProcessImple(float deltaT){
			Vector3 targetLookAtPosition = thisTarget.GetPosition();
			Vector3 currentLookerPosition = thisSmoothLooker.GetPosition();
			Quaternion targetLookAtRotation = Quaternion.LookRotation(
				targetLookAtPosition - currentLookerPosition
			);
			Quaternion currentLookRotation = thisSmoothLooker.GetRotation();

			float angle = Quaternion.Angle(
				targetLookAtRotation,
				currentLookRotation
			);

			Quaternion newRotation;
			
			if(angle < angleThreshold)
				newRotation = targetLookAtRotation;
			else{
				float t = angle * thisSmoothCoefficient * deltaT;
				newRotation = Quaternion.Slerp(
					currentLookRotation,
					targetLookAtRotation,
					t
				);
			}
			thisSmoothLooker.SetRotation(newRotation);
		}
		float displacementThreshold = .05f;
		bool DisplacementIsSmallEnough(Vector3 displacement){
			if(displacement.sqrMagnitude <= displacementThreshold * displacementThreshold)
				return true;
			return false;
		}
	}
	public interface ISmoothLookProcessConstArg: IProcessConstArg{
		IMonoBehaviourAdaptor target{get;}
		ISmoothLooker smoothLooker{get;}
		float smoothCoefficient{get;}
	}
	public class SmoothLookProcessConstArg: ProcessConstArg, ISmoothLookProcessConstArg{
		public SmoothLookProcessConstArg(
			IProcessManager processManager,

			IMonoBehaviourAdaptor target,
			ISmoothLooker looker,
			float smoothCoefficient
		): base(
			processManager
		){
			thisTarget = target;
			thisSmoothLooker = looker;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly IMonoBehaviourAdaptor thisTarget;
		public IMonoBehaviourAdaptor target{get{return thisTarget;}}
		readonly ISmoothLooker thisSmoothLooker;
		public ISmoothLooker smoothLooker{get{return thisSmoothLooker;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
