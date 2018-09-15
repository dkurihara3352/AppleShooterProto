using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UISystem;


namespace AppleShooterProto{
	public interface IPlayerInputHandler{
		void OnTouch(int touchCount);
		void OnDrag(ICustomEventData eventData);
		void OnTap(int tapCount);
		void OnRelease();
		void ProcessSwipe(ICustomEventData eventData);
		void OnScrollerElementDisplace(float normalizedCursoredPosition, int axis);
	}
	public interface IPlayerInputStateEngine: ISwitchableStateEngine<IPlayerInputState>, IPlayerInputHandler{

		IPlayerInputState GetCurrentState();

		void SwitchToIdleState();
		void SwitchToLookingAroundState();
		void SwitchToDrawingState();
		void SwitchToWaitingForNextTouchState();

		void ResetCameraZoom();
		void ResetCameraPan();
		void StopResetCameraPan();
		void StartDraw();
		void HoldDraw();
		void Fire();
		void PanCamera(
			float normalizedCurosredPosition,
			int axis
		);
	}
	public class PlayerInputStateEngine : AbsSwitchableStateEngine<IPlayerInputState>, IPlayerInputStateEngine {

		public PlayerInputStateEngine(
			IPlayerInputStateEngineConstArg arg
		){
			IPlayerInputStateConstArg stateArg = new PlayerInputStateConstArg(
				this,
				arg.drawDeltaThreshold,
				arg.processFactory
			);
			thisIdleState = new PlayerInputIdleState(stateArg);
			thisDrawingState = new PlayerInputDrawingState(stateArg);
			thisLookingAroundState = new PlayerInputLookingAroundState(stateArg);
			thisWaitingForNextTouchState = new PlayerInputWaitingForNextTouchState(stateArg);

			thisPlayerInputManager = arg.playerInputManager;

			// SwitchToIdleState();
			thisCurState = thisIdleState;
		}
		readonly IPlayerInputManager thisPlayerInputManager;
		readonly IPlayerInputIdleState thisIdleState;
		readonly IPlayerInputDrawingState thisDrawingState;
		readonly IPlayerInputLookingAroundState thisLookingAroundState;
		readonly IPlayerInputWaitingForNextTouchState thisWaitingForNextTouchState;

		public IPlayerInputState GetCurrentState(){
			return thisCurState;
		}

		/* Delegate To States */
			public void OnTouch(int touchCount){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Touched"
				);
				thisCurState.OnTouch(touchCount);
			}
			public void OnDrag(ICustomEventData eventData){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Dragged"
				);
				thisCurState.OnDrag(eventData);
			}
			public void OnTap(int tapCount){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Tapped"
				);
				thisCurState.OnTap(tapCount);
			}
			public void OnRelease(){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Released"
				);
				thisCurState.OnRelease();
			}
			public void ProcessSwipe(ICustomEventData eventData){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Swiped"
				);
				thisCurState.ProcessSwipe(eventData);
			}
			public void OnScrollerElementDisplace(
				float normalizedCurosredPosition,
				int axis
			){
				Debug.Log(
					GetCurrentState().GetName() + " " + 
					"Displaced"
				);
				thisCurState.OnScrollerElementDisplace(
					normalizedCurosredPosition,
					axis
				);
			}
		/* Switch */
			public void SwitchToIdleState(
			){
				TrySwitchState(thisIdleState);
			}
			public void SwitchToDrawingState(){
				TrySwitchState(thisDrawingState);
			}
			public void SwitchToLookingAroundState(){
				TrySwitchState(thisLookingAroundState);
			}
			public void SwitchToWaitingForNextTouchState(){
				TrySwitchState(thisWaitingForNextTouchState);
			}
		/* Delegate To PlayerInputManager */
			public void ResetCameraZoom(){
				thisPlayerInputManager.ResetCameraZoom();
			}
			public void ResetCameraPan(){
				thisPlayerInputManager.ResetCameraPan();
			}
			public void StopResetCameraPan(){
				thisPlayerInputManager.StopResetCameraPan();
			}

			public void StartDraw(){
				thisPlayerInputManager.StartDraw();
			}
			public void HoldDraw(){
				thisPlayerInputManager.HoldDraw();
			}
			public void Fire(){
				thisPlayerInputManager.Fire();
			}
			public void PanCamera(
				float normalizedCursoredPosition,
				int axis
			){
				thisPlayerInputManager.PanCamera(
					normalizedCursoredPosition,
					axis
				);
			}
		/*  */
	}

	public interface IPlayerInputStateEngineConstArg{
		IPlayerInputManager playerInputManager{get;}
		float drawDeltaThreshold{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public struct PlayerInputStateEngineConstArg: IPlayerInputStateEngineConstArg{
		public PlayerInputStateEngineConstArg(
			IPlayerInputManager playerInputManager,
			float drawDeltaThreshold,
			IAppleShooterProcessFactory processFactory
		){
			thisInputManager = playerInputManager;
			thisDrawDeltaThreshold = drawDeltaThreshold;
			thisProcessFactory = processFactory;
		}
		readonly IPlayerInputManager thisInputManager;
		public IPlayerInputManager playerInputManager{
			get{return thisInputManager;}
		}
		readonly float thisDrawDeltaThreshold;
		public float drawDeltaThreshold{get{return thisDrawDeltaThreshold;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
