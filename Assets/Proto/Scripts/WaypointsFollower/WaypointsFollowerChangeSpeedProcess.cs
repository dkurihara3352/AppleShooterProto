using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IWaypointsFollowerChangeSpeedProcess: IProcess{}
	public class WaypointsFollowerChangeSpeedProcess: AbsConstrainedProcess, IWaypointsFollowerChangeSpeedProcess{
		public WaypointsFollowerChangeSpeedProcess(
			IConstArg arg
		): base(arg){
			thisFollowProcess = arg.followProcess;
			thisSpeedCurve = arg.speedCurve;
		}
		readonly IFollowWaypointProcess thisFollowProcess;
		readonly AnimationCurve thisSpeedCurve;

		protected override void UpdateProcessImple(float deltaT){
			float timeScale = thisSpeedCurve.Evaluate(thisNormalizedTime);
			thisFollowProcess.SetTimeScale(timeScale);
		}
		float thisStopThreshold = .01f;
		protected override void StopImple(){
			if(thisFollowProcess.GetTimeScale() <= thisStopThreshold)
				thisFollowProcess.Stop();
		}




		public interface IConstArg: IConstrainedProcessConstArg{
			IFollowWaypointProcess followProcess{get;}
			AnimationCurve speedCurve{get;}
		}
		public class ConstArg: ConstrainedProcessConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float time,
				IFollowWaypointProcess followProcess,
				AnimationCurve speedCurve
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				time
			){
				thisFollowProcess = followProcess;
				thisSpeedCurve = speedCurve;
			}
			readonly IFollowWaypointProcess thisFollowProcess;
			public IFollowWaypointProcess followProcess{get{return thisFollowProcess;}} 
			readonly AnimationCurve thisSpeedCurve;
			public AnimationCurve speedCurve{get{return thisSpeedCurve;}}
		}
	}
}

