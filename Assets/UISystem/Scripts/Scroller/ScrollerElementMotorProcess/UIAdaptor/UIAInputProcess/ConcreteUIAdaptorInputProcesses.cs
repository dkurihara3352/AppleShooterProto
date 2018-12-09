using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUIAWaitForTapProcess: IUIAdaptorInputProcess{}
	public class UIAWaitForTapProcess : AbsUIAdaptorInputProcess, IUIAWaitForTapProcess {
		public UIAWaitForTapProcess(IConstArg arg): base(arg){

		}
		IWaitingForTapState typedState{
			get{
				return (IWaitingForTapState)thisState;
			}
		}
		protected override void UpdateProcessImple(float deltaT){
			thisEngine.HoldUIE(deltaT);
		}
		protected override void ExpireImple(){
			thisEngine.WaitForRelease();
			thisEngine.DelayTouchUIE();
		}
	}
	public interface IUIAWaitForReleaseProcess: IUIAdaptorInputProcess{}
	public class UIAWaitForReleaseProcess: AbsUIAdaptorInputProcess, IUIAWaitForReleaseProcess{
		public UIAWaitForReleaseProcess(IConstArg arg): base(arg){}
		protected override void UpdateProcessImple(float deltaT){
			thisEngine.HoldUIE(deltaT);
		}
	}
	public interface IUIAWaitForNextTouchProcess: IUIAdaptorInputProcess{}
	public class UIAWaitForNextTouchProcess: AbsUIAdaptorInputProcess, IUIAWaitForNextTouchProcess{
		public UIAWaitForNextTouchProcess(IConstArg arg): base(arg){}
		protected override void ExpireImple(){
			thisEngine.WaitForFirstTouch();
			thisEngine.DelayedReleaseUIE();
		}
	}
}
