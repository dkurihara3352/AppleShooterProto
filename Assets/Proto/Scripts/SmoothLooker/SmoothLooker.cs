using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ISmoothLooker{
		void StartSmoothLook();
		void StopSmoothLook();
		void SetLookAtTarget(ILookAtTarget target);
		Vector3 GetPosition();
		void LookAt(Vector3 position);
	}
	public class SmoothLooker :ISmoothLooker {
		public SmoothLooker(
			ISmoothLookerConstArg arg
		){
			thisAdaptor = arg.adaptor;
			thisProcessFactory = arg.processFactory;
		}

		readonly ISmoothLookerAdaptor thisAdaptor;
		readonly IAppleShooterProcessFactory thisProcessFactory;
		ILookAtTarget thisLookAtTarget;
		public void SetLookAtTarget(ILookAtTarget target){
			thisLookAtTarget = target;
		}
		ILookAtTargetProcess thisProcess;
		public void StartSmoothLook(){
			thisProcess = thisProcessFactory.CreateLookAtTargetProcess(
				this,
				thisLookAtTarget
			);
			thisProcess.Run();
		}
		public void StopSmoothLook(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public void LookAt(Vector3 position){
			thisAdaptor.LookAt(position);
		}
	}



	public interface ISmoothLookerConstArg{
		ISmoothLookerAdaptor adaptor{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public struct SmoothLookerConstArg: ISmoothLookerConstArg{
		public SmoothLookerConstArg(
			ISmoothLookerAdaptor adaptor,
			IAppleShooterProcessFactory processFactory
		){
			thisAdaptor = adaptor;
			thisProcessFactory = processFactory;
		}
		readonly ISmoothLookerAdaptor thisAdaptor;
		public ISmoothLookerAdaptor adaptor{get{return thisAdaptor;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
