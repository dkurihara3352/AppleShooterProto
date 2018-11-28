using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatLevelUpProcess: IProcess{}
	public class HeatLevelUpProcess : AbsProcess, IHeatLevelUpProcess {
		public HeatLevelUpProcess(
			IConstArg arg
		): base(arg){
			thisHeatManager = arg.heatManager;
			thisTargetMaxHeat = arg.targetMaxHeat;
			thisSmoothTime  = arg.smoothTime;
		}
		readonly IHeatManager thisHeatManager;
		readonly float thisTargetMaxHeat;
		float thisDiffThreshold = .001f;
		float thisSmoothTime;

		protected override void UpdateProcessImple(float deltaT){
			float currentMaxHeat = thisHeatManager.GetMaxHeat();
			float diff = thisTargetMaxHeat - currentMaxHeat;
			if(diff <= thisDiffThreshold){
				Expire();
				return;
			}

			float velocity = 0f;
			float newMaxHeat = Mathf.SmoothDamp(
				currentMaxHeat,
				thisTargetMaxHeat,
				ref velocity,
				thisSmoothTime
			);
			thisHeatManager.SetMaxHeat(newMaxHeat);
		}
		protected override void ExpireImple(){

		}

		public new interface IConstArg: AbsProcess.IConstArg{
			IHeatManager heatManager{get;}
			float targetMaxHeat{get;}
			float smoothTime{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				IHeatManager heatManager,
				float targetMaxHeat,
				float smoothTime
			): base(
				processManager
			){
				thisHeatManager = heatManager;
				thisTargetMaxHeat = targetMaxHeat;
				thisSmoothTime = smoothTime;
			}
			readonly IHeatManager thisHeatManager;
			public IHeatManager heatManager{get{return thisHeatManager;}}
			readonly float thisTargetMaxHeat;
			public float targetMaxHeat{get{return thisTargetMaxHeat;}}
			readonly float thisSmoothTime;
			public float smoothTime{get{return thisSmoothTime;}}

		}
	}
}
