using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeatImageSmoothFollowDeltaImageProcess: IProcess{}
	public class HeatImageSmoothFollowDeltaImageProcess : AbsProcess, IHeatImageSmoothFollowDeltaImageProcess {
		public HeatImageSmoothFollowDeltaImageProcess(
			IConstArg arg
		): base(arg){
			thisTargetHeat = arg.targetHeat;
			thisHeatImage = arg.heatImage;
			thisFollowSmoothTime = arg.followSmoothTime;
		}
		readonly float thisTargetHeat;
		readonly IHeatImage thisHeatImage;
		float thisInitHeat;
		float thisFollowSmoothTime;
		float thisDiffThreshold = .005f;
		protected override void UpdateProcessImple(float deltaT){
			float currentImageHeat = thisHeatImage.GetFollowImageHeat();
			float velocity = 0f;
			float newImageHeat = Mathf.SmoothDamp(
				currentImageHeat,
				thisTargetHeat,
				ref velocity,
				thisFollowSmoothTime,
				10f
			);
			thisHeatImage.SetFollowImageHeat(newImageHeat);
			float diff = thisTargetHeat - currentImageHeat;
			if(diff <= thisDiffThreshold)
				Expire();
		}
		protected override void ExpireImple(){
			thisHeatImage.SetFollowImageHeat(thisTargetHeat);
			thisHeatImage.StartCountingDown();
		}
		public new interface IConstArg: AbsProcess.IConstArg{
			IHeatImage heatImage{get;}
			float targetHeat{get;}
			float followSmoothTime{get;}
		}
		public new class ConstArg: AbsProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				IHeatImage heatImage,
				float targetHeat,
				float followSmoothTime
			): base(
				processManager
			)
			{
				thisHeatImage = heatImage;
				thisTargetHeat = targetHeat;
				thisFollowSmoothTime = followSmoothTime;
			}
			readonly IHeatImage thisHeatImage;
			public IHeatImage heatImage{get{return thisHeatImage;}}
			readonly float thisTargetHeat;
			public float targetHeat{get{return thisTargetHeat;}}
			readonly float thisFollowSmoothTime;
			public float followSmoothTime{get{return thisFollowSmoothTime;}}
		}
	}
}
