using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothLooker{
		void StartSmoothLook();
		void StopSmoothLook();
		void SetLookAtTarget(IMonoBehaviourAdaptor target);
		Vector3 GetPosition();
		void LookAt(Vector3 position);
		Vector3 GetLookAtPosition();
		Quaternion GetRotation();
		void SetRotation(Quaternion rotation);
	}
	public class SmoothLooker :ISmoothLooker {
		public SmoothLooker(
			ISmoothLookerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}

		readonly ISmoothLookerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		IMonoBehaviourAdaptor thisLookAtTarget;
		public void SetLookAtTarget(IMonoBehaviourAdaptor target){
			thisLookAtTarget = target;
		}
		ISmoothLookProcess thisProcess;
		readonly float thisSmoothCoefficient;
		public void StartSmoothLook(){
			thisProcess = thisProcessFactory.CreateSmoothLookProcess(
				this,
				thisLookAtTarget,
				thisSmoothCoefficient
			);
			thisProcess.Run();
			thisAdaptor.SetReady();
		}
		public void StopSmoothLook(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		Vector3 thisLookAtPosition;
		public Vector3 GetLookAtPosition(){
			return thisLookAtPosition;
		}
		public void LookAt(Vector3 position){
			thisLookAtPosition = position;
			thisAdaptor.LookAt(position);
		}
		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}

	}



	public interface ISmoothLookerConstArg{
		ISmoothLookerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float smoothCoefficient{get;}
	}
	public struct SmoothLookerConstArg: ISmoothLookerConstArg{
		public SmoothLookerConstArg(
			ISmoothLookerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly ISmoothLookerAdaptor thisAdaptor;
		public ISmoothLookerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
