using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace AppleShooterProto{
	public interface ISmoothZoomProcess: IProcess{}
	public class SmoothZoomProcess : AbsProcess, ISmoothZoomProcess {

		public SmoothZoomProcess(
			ISmoothZoomProcessConstArg arg
		):base(
			arg
		){
			thisPlayerCamera = arg.playerCamera;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}
		readonly IPlayerCamera thisPlayerCamera;
		readonly float thisSmoothCoefficient;
		protected override void UpdateProcessImple(float deltaT){
			float targetFOV = thisPlayerCamera.GetTargetFOV();
			float currentFOV = thisPlayerCamera.GetCurrentFOV();
			float difference = targetFOV - currentFOV;
			float deltaFOV = difference * thisSmoothCoefficient * deltaT;

			float newFOV = currentFOV + deltaFOV;
			thisPlayerCamera.SetFOV(newFOV);
		}
	}


	public interface ISmoothZoomProcessConstArg: IProcessConstArg{
		IPlayerCamera playerCamera{get;}
		float smoothCoefficient{get;}
	}
	public class SmoothZoomProcessConstArg: ProcessConstArg, ISmoothZoomProcessConstArg{
		public SmoothZoomProcessConstArg(
			IPlayerCamera playerCamera,
			float smoothCoefficient,
			IProcessManager processManager
		):base(
			processManager
		){
			thisPlayerCamera = playerCamera;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly IPlayerCamera thisPlayerCamera;
		public IPlayerCamera playerCamera{get{return thisPlayerCamera;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
