using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IAlphaActivatorUIEActivationProcess: IUIEActivationProcess{
	}
	public class AlphaActivatorUIEActivationProcess: AbsInterpolatorProcess<IGroupAlphaInterpolator>, IAlphaActivatorUIEActivationProcess{
		public AlphaActivatorUIEActivationProcess(
			IConstArg arg
		): base(
			arg
		){
			thisEngine = arg.engine;
			thisUIElement = arg.uiElement;
			thisDoesActivate = arg.doesActivate;
			thisUIAdaptor = thisUIElement.GetUIAdaptor();
		}
		readonly IUIEActivationStateEngine thisEngine;
		readonly IUIElement thisUIElement;
		readonly bool thisDoesActivate;
		readonly IUIAdaptor thisUIAdaptor;
		protected override float GetLatestInitialValueDifference(){
			/*  not called anyway
			 */
			float currentGroupAlpha = thisUIAdaptor.GetGroupAlpha();
			currentGroupAlpha = MakeGroupAlphaValueInRange(currentGroupAlpha);
			float targetGroupAlpha = thisDoesActivate? 1f: 0f;
			float valueDifference = targetGroupAlpha - currentGroupAlpha;
			return valueDifference;
		}
		float MakeGroupAlphaValueInRange(float source){
			if(source < 0f)
				return 0f;
			else if(source > 1f)
				return 1f;
			else return source;
		}
		protected override IGroupAlphaInterpolator CreateInterpolator(){
			float targetGroupAlpha = thisDoesActivate? 1f: 0f;
			IGroupAlphaInterpolator irper = new GroupAlphaInterpolator(thisUIAdaptor, targetGroupAlpha);
			return irper;
		}
		protected override void ExpireImple(){
			if(thisDoesActivate)
				thisEngine.SetToActivationCompletedState();
			else
				thisEngine.SetToDeactivationCompletedState();
		}
		public new interface IConstArg: AbsInterpolatorProcess<IGroupAlphaInterpolator>.IConstArg{
			IUIEActivationStateEngine engine{get;}
			IUIElement uiElement{get;}
			bool doesActivate{get;}
		}
		public new class ConstArg: AbsInterpolatorProcess<IGroupAlphaInterpolator>.ConstArg, IConstArg{
			public ConstArg(
				IProcessManager processManager,
				float expireTime,

				IUIEActivationStateEngine engine,
				IUIElement uiElement,
				bool doesActivate
			): base(
				processManager,
				ProcessConstraint.ExpireTime,
				expireTime,
				false
			){
				thisEngine = engine;
				thisUIElement = uiElement;
				thisDoesActivate = doesActivate;
			}
			readonly IUIEActivationStateEngine thisEngine;
			public IUIEActivationStateEngine engine{get{return thisEngine;}}
			readonly IUIElement thisUIElement;
			public IUIElement uiElement{get{return thisUIElement;}}
			readonly bool thisDoesActivate;
			public bool doesActivate{get{return thisDoesActivate;}}
		}
	}
}

