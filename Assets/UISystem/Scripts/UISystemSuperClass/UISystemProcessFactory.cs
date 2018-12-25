using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityBase;

namespace UISystem{
	public interface IUISystemProcessFactory: IUnityBaseProcessFactory{
		IUIAWaitForTapProcess CreateUIAWaitForTapProcess(
			IWaitingForTapState state,
			IUIAdaptorInputStateEngine engine,
			float waitTime
		);
		IUIAWaitForReleaseProcess CreateUIAWaitForReleaseProcess(
			IWaitingForReleaseState state,
			IUIAdaptorInputStateEngine engine
		);
		IUIAWaitForNextTouchProcess CreateUIAWaitForNextTouchProcess(
			IWaitingForNextTouchState state,
			IUIAdaptorInputStateEngine engine,
			float waitTime
		);
		IAlphaActivatorUIEActivationProcess CreateAlphaActivatorUIEActivationProcess(
			IUIElement uiElement, 
			IUIEActivationStateEngine engine, 
			bool doesActivate,
			float expireTime
		);
		INonActivatorUIEActivationProcess CreateNonActivatorUIEActivationProcess(
			IUIEActivationStateEngine engine, 
			bool doesActivate,
			float expireTime
		);
		IScrollerElementSnapProcess CreateScrollerElementSnapProcess(
			IScroller scroller, 
			IUIElement scrollerElement, 
			float targetElementLocalPosOnAxis, 
			float initialVelOnAxis, 
			int dimension
		);
		IInertialScrollProcess CreateInertialScrollProcess(
			float deltaPosOnAxis, 
			float decelerationOnAxis, 
			IScroller scroller, 
			IUIElement scrollerElement, 
			int dimension,
			float inertiaDecay
		);
		IImageColorTurnProcess CreateGenericImageColorTurnProcess(
			IUIImage uiImage, 
			Color targetColor,
			float time
		);
		IImageColorTurnProcess CreateFalshColorProcess(
			IUIImage uiImage, 
			Color targetColor,
			float time
		);
		IAlphaPopUpProcess CreateAlphaPopUpProcess(
			IPopUpStateEngine engine,
			float time,
			bool hides
		);
	}
	public class UISystemProcessFactory: UnityBaseProcessFactory, IUISystemProcessFactory{
		public UISystemProcessFactory(
			IProcessManager procManager,
			IUISystemMonoBehaviourAdaptorManager adaptorManager
		): base(
			procManager
		){
			
			thisAdaptorManager = adaptorManager;
		}
		readonly IUISystemMonoBehaviourAdaptorManager thisAdaptorManager;
		IUIManager thisUIManager{
			get{
				return thisAdaptorManager.GetUIManager();
			}
		}

		public IUIAWaitForTapProcess CreateUIAWaitForTapProcess(
			IWaitingForTapState state,
			IUIAdaptorInputStateEngine engine,
			float waitTime//.5f
		){
			AbsUIAdaptorInputProcess.IConstArg arg = new AbsUIAdaptorInputProcess.ConstArg(
				thisProcessManager,
				ProcessConstraint.ExpireTime,
				waitTime,
				state,
				engine
			);
			return new UIAWaitForTapProcess(arg);
		}
		public IUIAWaitForReleaseProcess CreateUIAWaitForReleaseProcess(
			IWaitingForReleaseState state,
			IUIAdaptorInputStateEngine engine
		){
			UIAWaitForReleaseProcess.IConstArg arg = new UIAWaitForReleaseProcess.ConstArg(
				thisProcessManager,
				ProcessConstraint.None,
				1f,
				state,
				engine
			);
			return new UIAWaitForReleaseProcess(arg);
		}
		public IUIAWaitForNextTouchProcess CreateUIAWaitForNextTouchProcess(
			IWaitingForNextTouchState state,
			IUIAdaptorInputStateEngine engine,
			float waitTime//.5
		){
			AbsUIAdaptorInputProcess.IConstArg arg = new AbsUIAdaptorInputProcess.ConstArg(
				thisProcessManager,
				ProcessConstraint.ExpireTime,
				waitTime,
				state,
				engine
			);
			return new UIAWaitForNextTouchProcess(arg);
		}
		public IAlphaActivatorUIEActivationProcess CreateAlphaActivatorUIEActivationProcess(
			IUIElement uiElement, 
			IUIEActivationStateEngine engine, 
			bool doesActivate,
			float expireTime//.2

		){
			AlphaActivatorUIEActivationProcess.IConstArg arg = new AlphaActivatorUIEActivationProcess.ConstArg(
				thisProcessManager, 
				expireTime,

				engine,
				uiElement, 
				doesActivate
			);
			IAlphaActivatorUIEActivationProcess process = new AlphaActivatorUIEActivationProcess(
				arg
			);
			return process;
		}
		public INonActivatorUIEActivationProcess CreateNonActivatorUIEActivationProcess(
			IUIEActivationStateEngine engine, 
			bool doesActivate,
			float expireTime//.2
		){
			NonActivatorUIEActivationProcess.IConstArg arg = new NonActivatorUIEActivationProcess.ConstArg(
				thisProcessManager, 
				expireTime,
				engine,
				doesActivate
			);
			INonActivatorUIEActivationProcess process = new NonActivatorUIEActivationProcess(
				arg
			);
			return process;
		}
		public IScrollerElementSnapProcess CreateScrollerElementSnapProcess(
			IScroller scroller, 
			IUIElement scrollerElement, 
			float targetElementLocalPosOnAxis, 
			float initialVelOnAxis, 
			int dimension
		){
			ScrollerElementSnapProcess.IConstArg arg = new ScrollerElementSnapProcess.ConstArg(
				thisProcessManager,
				scrollerElement,
				scroller,
				dimension,
				targetElementLocalPosOnAxis,
				initialVelOnAxis
			);
			return new ScrollerElementSnapProcess(arg);
		}
		public IInertialScrollProcess CreateInertialScrollProcess(
			float deltaPosOnAxis, 
			float decelerationAxisFactor, 
			IScroller scroller, 
			IUIElement scrollerElement, 
			int dimension,
			float inertiaDecay
		){
			InertialScrollProcess.IConstArg arg = new InertialScrollProcess.ConstArg(
				thisProcessManager,
				scroller,
				scrollerElement,
				dimension,
				deltaPosOnAxis,
				inertiaDecay,
				decelerationAxisFactor
			);
			return new InertialScrollProcess(arg);
		}
		public IImageColorTurnProcess CreateGenericImageColorTurnProcess(
			IUIImage uiImage, 
			Color targetColor,
			float time//.1
		){
			GenericImageColorTurnProcess.IConstArg arg = new GenericImageColorTurnProcess.ConstArg(
				thisProcessManager,
				time,
				uiImage,
				targetColor,
				false
			);
			return new GenericImageColorTurnProcess(arg);
		}
		public IImageColorTurnProcess CreateFalshColorProcess(
			IUIImage uiImage, 
			Color targetColor,
			float time//.1
		){
			GenericImageColorTurnProcess.IConstArg arg = new GenericImageColorTurnProcess.ConstArg(
				thisProcessManager,
				time,
				uiImage,
				targetColor,
				true
			);
			return new GenericImageColorTurnProcess(arg);
		}
		public IAlphaPopUpProcess CreateAlphaPopUpProcess(
			IPopUpStateEngine engine,
			float time,
			bool hides
		){
			AlphaPopUpProcess.IConstArg arg = new AlphaPopUpProcess.ConstArg(
				thisProcessManager,
				time,
				engine,
				hides
			);
			IAlphaPopUpProcess process = new AlphaPopUpProcess(arg);
			return process;
		}
	}
}
