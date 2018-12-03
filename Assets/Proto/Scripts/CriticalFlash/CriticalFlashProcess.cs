using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ICriticalFlashProcess: IProcess{}
	public class CriticalFlashProcess : AbsConstrainedProcess, ICriticalFlashProcess {
		public CriticalFlashProcess(
			IConstArg arg
		): base(arg){
			thisFlash = arg.flash;
			thisFlashCurve = arg.flashCurve;
		}
		readonly ICriticalFlash thisFlash;
		readonly AnimationCurve thisFlashCurve;

		protected override void UpdateProcessImple(float deltaT){
			float flashValue = thisFlashCurve.Evaluate(thisNormalizedTime);
			thisFlash.SetFlashValue(flashValue);
		}

		public new interface IConstArg: AbsConstrainedProcess.IConstArg{
			ICriticalFlash flash{get;}
			AnimationCurve flashCurve{get;}
		}
		public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float flashTime,

				ICriticalFlash flash,
				AnimationCurve flashCurve
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				flashTime
			){
				thisFlash = flash;
				thisFlashCurve = flashCurve;
			}
			readonly ICriticalFlash thisFlash;
			public ICriticalFlash flash{get{return thisFlash;}}
			readonly AnimationCurve thisFlashCurve;
			public AnimationCurve flashCurve{get{return thisFlashCurve;}} 
		}
	}
}
