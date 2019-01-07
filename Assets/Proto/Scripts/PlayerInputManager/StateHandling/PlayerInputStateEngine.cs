﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UISystem;


namespace SlickBowShooting{
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
		void Nock();
		void StartDraw();
		void HoldDraw();
		bool IsHeld();
		void ReleaseHold();
		void Release();
		void PanCamera(
			float normalizedCurosredPosition,
			int axis
		);
		// void TryResetArrow();
		void DeactivateArrow();
		void ClearAndDeactivateShotInBuffer();
		void Aim();
		void Unaim();

		bool IsDrawing();
		bool IsLookingAround();
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
				thisCurState.OnTouch(touchCount);
			}
			public void OnDrag(ICustomEventData eventData){
				thisCurState.OnDrag(eventData);
			}
			public void OnTap(int tapCount){
				thisCurState.OnTap(tapCount);
			}
			public void OnRelease(){
				thisCurState.OnRelease();
			}
			public void ProcessSwipe(ICustomEventData eventData){
				thisCurState.ProcessSwipe(eventData);
			}
			public void OnScrollerElementDisplace(
				float normalizedCurosredPosition,
				int axis
			){
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
			public void Nock(){
				thisPlayerInputManager.Nock();
			}
			public void StartDraw(){
				thisPlayerInputManager.StartDraw();
			}
			public void HoldDraw(){
				thisPlayerInputManager.HoldDraw();
			}
			public bool IsHeld(){
				return thisPlayerInputManager.IsHeld();
			}
			public void ReleaseHold(){
				thisPlayerInputManager.ReleaseHold();
			}
			public void StopDraw(){
				thisPlayerInputManager.StopDraw();
			}
			public void Release(){
				thisPlayerInputManager.Release();
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
			public void DeactivateArrow(){
				thisPlayerInputManager.DeactivateArrow();
			}
		/*  */
			public void ClearAndDeactivateShotInBuffer(){
				thisPlayerInputManager.ClearAndDeactivateShotInBuffer();
			}
			public void Aim(){
				thisPlayerInputManager.Aim();
			}
			public void Unaim(){
				thisPlayerInputManager.Unaim();
			}
		/*  */
			public bool IsDrawing(){
				return thisCurState == thisDrawingState;
			}
			public bool IsLookingAround(){
				return thisCurState == thisLookingAroundState;
			}
	}

	public interface IPlayerInputStateEngineConstArg{
		IPlayerInputManager playerInputManager{get;}
		float drawDeltaThreshold{get;}
		ISlickBowShootingProcessFactory processFactory{get;}
	}
	public struct PlayerInputStateEngineConstArg: IPlayerInputStateEngineConstArg{
		public PlayerInputStateEngineConstArg(
			IPlayerInputManager playerInputManager,
			float drawDeltaThreshold,
			ISlickBowShootingProcessFactory processFactory
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
		readonly ISlickBowShootingProcessFactory thisProcessFactory;
		public ISlickBowShootingProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
