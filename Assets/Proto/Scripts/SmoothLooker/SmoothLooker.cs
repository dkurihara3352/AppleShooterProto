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
		Quaternion GetRotation();
		void SetRotation(Quaternion rotation);
		void RotateToward(Quaternion to, float step);
		void SetSmoothCoefficient(float k);
	}
	public class SmoothLooker :ISmoothLooker {
		public SmoothLooker(
			ISmoothLookerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
			thisSmoothCoefficient = arg.smoothCoefficient;
			thisProcessOrder = arg.processOrder;
		}

		readonly ISmoothLookerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		IMonoBehaviourAdaptor thisLookAtTarget;
		public void SetLookAtTarget(IMonoBehaviourAdaptor target){
			thisLookAtTarget = target;
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.UpdateLookAtTarget(thisLookAtTarget);
		}
		ISmoothLookProcess thisProcess;
		float thisSmoothCoefficient;
		public void SetSmoothCoefficient(float k){
			thisSmoothCoefficient = k;
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.UpdateSmoothCoefficient(k);
		}
		readonly int thisProcessOrder;
		public void StartSmoothLook(){
			thisProcess = thisProcessFactory.CreateSmoothLookProcess(
				this,
				thisLookAtTarget,
				thisSmoothCoefficient,
				thisProcessOrder
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
		public Quaternion GetRotation(){
			return thisAdaptor.GetRotation();
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public void RotateToward(Quaternion to, float step){
			Quaternion newRotation = Quaternion.RotateTowards(GetRotation(), to, step);
			thisAdaptor.SetRotation(newRotation);
		}

	}



	public interface ISmoothLookerConstArg{
		ISmoothLookerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float smoothCoefficient{get;}
		int processOrder{get;}
	}
	public struct SmoothLookerConstArg: ISmoothLookerConstArg{
		public SmoothLookerConstArg(
			ISmoothLookerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient,
			int processOrder
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
			thisProcessOrder = processOrder;
		}
		readonly ISmoothLookerAdaptor thisAdaptor;
		public ISmoothLookerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
		readonly int thisProcessOrder;
		public int processOrder{get{return thisProcessOrder;}}
	}
}
