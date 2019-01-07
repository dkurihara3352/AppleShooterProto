using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UISystem;

namespace SlickBowShooting{
	public interface IPlayerInputState: ISwitchableState, IPlayerInputHandler{
		string GetName();
	}
	public abstract class AbsPlayerInputState: IPlayerInputState{
		public AbsPlayerInputState(
			IPlayerInputStateConstArg arg
		){
			thisEngine = arg.engine;
			thisDrawDeltaThreshold = arg.drawDeltaThreshold;
			thisProcessFactory  = arg.processFactory;
		}
		readonly protected IPlayerInputStateEngine thisEngine;
		readonly float thisDrawDeltaThreshold;
		readonly protected ISlickBowShootingProcessFactory thisProcessFactory;
		protected bool PointerDeltaIsWithinDrawThreshold(Vector3 velocity){
			return velocity.sqrMagnitude <= thisDrawDeltaThreshold * thisDrawDeltaThreshold;
		}
		public abstract void OnEnter();
		public virtual void OnExit(){}

		public abstract void OnTouch(int touchCount);

		public abstract void OnDrag(ICustomEventData eventData);
		public abstract void OnTap(int tapCount);
		public abstract void OnRelease();
		public abstract void ProcessSwipe(ICustomEventData eventData);

		public abstract void OnScrollerElementDisplace(
			float normalizedCursoredPosition, 
			int axis
		);
		public abstract string GetName();
	}
	public interface IPlayerInputStateConstArg{
		IPlayerInputStateEngine engine{get;}
		float drawDeltaThreshold{get;}
		ISlickBowShootingProcessFactory processFactory{get;}
	}
	public class PlayerInputStateConstArg: IPlayerInputStateConstArg{
		public PlayerInputStateConstArg(
			IPlayerInputStateEngine engine,
			float drawDeltaThreshold,
			ISlickBowShootingProcessFactory processFactory
		){
			thisEngine = engine;
			thisDrawDeltaThreshold = drawDeltaThreshold;
			thisProcessFactory = processFactory;
		}
		readonly IPlayerInputStateEngine thisEngine;
		public IPlayerInputStateEngine engine{get{return thisEngine;}}
		readonly float thisDrawDeltaThreshold;
		public float drawDeltaThreshold{get{return thisDrawDeltaThreshold;}}
		readonly ISlickBowShootingProcessFactory thisProcessFactory;
		public ISlickBowShootingProcessFactory processFactory{get{return thisProcessFactory;}}
	}
	public abstract class AbsPlayerInputPointerUpState: AbsPlayerInputState{
		public AbsPlayerInputPointerUpState(
			IPlayerInputStateConstArg arg
		): base(arg){
		}
		public override void OnRelease(){
			// throw new System.InvalidOperationException(
			// 	"OnRelease: cannot happen while pointer is up"
			// );
			OnTouch(1);
			thisEngine.OnRelease();
			
			
		}
		public override void OnDrag(ICustomEventData eventData){
			// throw new System.InvalidOperationException(
			// 	"OnDrag: cannot happen while pointer is up"
			// );
			thisEngine.OnTouch(1);
			thisEngine.OnDrag(eventData);
		}
		public override void OnTap(int tapCount){
			// throw new System.InvalidOperationException(
			// 	"OnTap: cannot happen while pointer is up"
			// );
			thisEngine.OnTouch(1);
			thisEngine.OnTap(tapCount);
		}
		public override void ProcessSwipe(ICustomEventData eventData){
			// throw new System.InvalidOperationException(
			// 	"OnTap: cannot happen while pointer is up"
			// );
			thisEngine.OnTouch(1);
			thisEngine.ProcessSwipe(eventData);
		}
	}
	public abstract class AbsPlayerInputPointerDownState: AbsPlayerInputState{
		public AbsPlayerInputPointerDownState(IPlayerInputStateConstArg arg): base(arg){}
		public override void OnTouch(int touchCount){
			throw new System.InvalidOperationException(
				GetName() + ": " +
				"OnTouch: cannot happen while pointer is down"
			);
		}
	}
	public interface IPlayerInputIdleState: IPlayerInputState{}
	public class PlayerInputIdleState: AbsPlayerInputPointerUpState, IPlayerInputIdleState{
		public PlayerInputIdleState(IPlayerInputStateConstArg arg): base(arg){}
		public override void OnEnter(){
			thisEngine.ResetCameraZoom();
			thisEngine.ResetCameraPan();
			thisEngine.Unaim();
		}
		public override void OnTouch(int touchCount){
			thisEngine.ClearAndDeactivateShotInBuffer();
			thisEngine.Nock();
			thisEngine.SwitchToDrawingState();
		}
		public override void OnScrollerElementDisplace(
			float normalizedCursoredPosition, 
			int axis
		){
			thisEngine.PanCamera(
				normalizedCursoredPosition, 
				axis
			);
		}
		public override string GetName(){
			return "Idle State";
		}
	}
	public interface IPlayerInputDrawingState: IPlayerInputState{}
	public class PlayerInputDrawingState: AbsPlayerInputPointerDownState, IPlayerInputDrawingState{
		public PlayerInputDrawingState(IPlayerInputStateConstArg arg): base(arg){}
		public override void OnEnter(){
			if(thisEngine.IsHeld())
				thisEngine.ReleaseHold();
			else{
				thisEngine.Aim();
				thisEngine.StartDraw();
			}
		}
		public override void OnDrag(ICustomEventData eventData){
			if(PointerDeltaIsWithinDrawThreshold(eventData.velocity)){
				return;
			}else{
				thisEngine.HoldDraw();
				thisEngine.SwitchToLookingAroundState();
			}
		}
		public override void ProcessSwipe(ICustomEventData eventData){
			thisEngine.HoldDraw();
			thisEngine.DeactivateArrow();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnRelease(){
			thisEngine.Release();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnTap(int tapCount){
			thisEngine.Release();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnScrollerElementDisplace(
			float normalizedCursoredPosition, 
			int axis
		){
			thisEngine.PanCamera(
				normalizedCursoredPosition,
				axis
			);
		}
		public override string GetName(){
			return "Drawing State";
		}
	}
	public interface IPlayerInputLookingAroundState: IPlayerInputState{}
	public class PlayerInputLookingAroundState: AbsPlayerInputPointerDownState, IPlayerInputLookingAroundState{
		public PlayerInputLookingAroundState(IPlayerInputStateConstArg arg): base(arg){}
		public override void OnEnter(){
			// thisEngine.Unaim();
			// thisEngine.ResetCameraZoom();
			return;
		}
		public override void OnDrag(ICustomEventData eventData){
			if(PointerDeltaIsWithinDrawThreshold(eventData.velocity)){
				thisEngine.SwitchToDrawingState();
			}
		}
		public override void ProcessSwipe(ICustomEventData eventData){
			thisEngine.DeactivateArrow();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnRelease(){
			// thisEngine.Release();
			thisEngine.DeactivateArrow();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnTap(int tapCount){
			// thisEngine.Release();
			thisEngine.DeactivateArrow();
			thisEngine.SwitchToWaitingForNextTouchState();
		}
		public override void OnScrollerElementDisplace(
			float normalizedCursoredPosition, 
			int axis
		){
			thisEngine.PanCamera(
				normalizedCursoredPosition,
				axis
			);
		}
		public override string GetName(){
			return "Looking Around State";
		}
	}
	public interface IPlayerInputWaitingForNextTouchState: IPlayerInputState{}
	public class PlayerInputWaitingForNextTouchState: AbsPlayerInputPointerUpState, IPlayerInputWaitingForNextTouchState{
		public PlayerInputWaitingForNextTouchState(IPlayerInputStateConstArg arg): base(arg){
		}
		public override void OnEnter(){
			thisEngine.ResetCameraZoom();
			StartCountingDownToSwitchIdleState();
		}
		IWaitAndSwitchToIdleStateProcess thisProcess;
		float waitTime = 2f;
		void StartCountingDownToSwitchIdleState(){
			thisProcess = thisProcessFactory.CreateWaitAndSwitchToIdleStateProcess(
				thisEngine,
				waitTime
			);
			thisProcess.Run();
		}
		public override void OnTouch(int touchCount){
			thisEngine.ClearAndDeactivateShotInBuffer();
			thisEngine.Nock();
			thisEngine.SwitchToDrawingState();
			StopWaitAndSwitchToIdleStateProcess();
		} 
		void StopWaitAndSwitchToIdleStateProcess(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
		}
		public override void OnScrollerElementDisplace(float normalizedCursoredPosition, int axis){
			thisEngine.PanCamera(
				normalizedCursoredPosition,
				axis
			);
		}
		public override string GetName(){
			return "Waiting For Next Touch State";
		}
	}
}
