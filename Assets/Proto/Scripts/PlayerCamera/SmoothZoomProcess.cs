using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace SlickBowShooting{
	public interface ISmoothZoomProcess: IProcess{}
	public class SmoothZoomProcess : AbsProcess, ISmoothZoomProcess {

		public SmoothZoomProcess(
			IConstArg arg
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
		public new interface IConstArg: AbsProcess.IConstArg{
			IPlayerCamera playerCamera{get;}
			float smoothCoefficient{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
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


}
