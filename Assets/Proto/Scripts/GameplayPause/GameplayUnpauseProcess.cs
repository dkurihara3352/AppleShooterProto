using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IGameplayUnpauseProcess: IProcess{}
	public class GameplayUnpauseProcess : AbsConstrainedProcess, IGameplayUnpauseProcess {
		public GameplayUnpauseProcess(
			IConstArg arg
		): base(arg){
			thisGameplayPause = arg.gameplayPause;
			thisInitialTimeScale = thisGameplayPause.GetTimeScale();
		}
		readonly IGameplayPause thisGameplayPause;
		readonly float thisInitialTimeScale;
		
		
		protected override void UpdateProcessImple(float deltaT){
			float newTimeScale  = Mathf.Lerp(
				thisInitialTimeScale + .1f,// set to 0 and never gonna start!
				1f,
				thisNormalizedTime
			);
			thisGameplayPause.SetTimeScale(newTimeScale);
		}

		protected override void ExpireImple(){
			thisGameplayPause.ActivatePauseButton();
		}

		public new interface IConstArg: AbsConstrainedProcess.IConstArg{
			IGameplayPause gameplayPause{get;}
		}
		public new class ConstArg: AbsConstrainedProcess.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float time,

				IGameplayPause gameplayPause
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				time
			){
				thisPause = gameplayPause;
			}
			readonly IGameplayPause thisPause;
			public IGameplayPause gameplayPause{
				get{
					return thisPause;
				}
			}
		}
	}
}
