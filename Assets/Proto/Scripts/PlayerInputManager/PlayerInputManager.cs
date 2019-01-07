using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IPlayerInputManager: IPlayerInputHandler{
		void SetPlayerCamera(IPlayerCamera playerCamera);
		void SetInputScroller(ICoreGameplayInputScroller inputScroller);
		void SetShootingManager(IShootingManager shootingManager);
		void SetCameraPivotSmoothFollower(ISmoothFollower follower);

		void ResetCameraZoom();
		void ResetCameraPan();
		void PanCamera(
			float normalizedCameraPosition,
			int axis
		);

		bool IsDrawing();
		bool IsLookingAround();
		void Nock();
		void StartDraw();
		void HoldDraw();
		bool IsHeld();
		void ReleaseHold();
		void StopDraw();
		void Release();
		// void TryResetArrow();
		void DeactivateArrow();
		void ClearAndDeactivateShotInBuffer();
		void Aim();
		void Unaim();

		IPlayerInputState GetCurrentState();
		float GetMaxZoom();
		void Zoom(float normalizedZoom);
		Vector3 GetLauncherVelocity();
	}
	public class PlayerInputManager : IPlayerInputManager {
		public PlayerInputManager(IPlayerInputManagerConstArg arg){
			thisDefaultFOV = arg.defaultFOV;
			thisAdaptor = arg.adaptor;


			IPlayerInputStateEngineConstArg engineConstArg = new PlayerInputStateEngineConstArg(
				this,
				arg.drawDeltaThreshold,
				arg.processFactory
			);
			thisEngine = new PlayerInputStateEngine(engineConstArg);
		}
		readonly IPlayerInputManagerAdaptor thisAdaptor;
		readonly float thisDefaultFOV;
		IPlayerInputStateEngine thisEngine;
		public IPlayerInputState GetCurrentState(){
			return thisEngine.GetCurrentState();
		}
		/* Engine delegate */
			public void OnTouch(int touchCount){
				thisEngine.OnTouch(touchCount);
			}
			public void OnDrag(ICustomEventData eventData){
				thisEngine.OnDrag(eventData);
			}
			public void OnTap(int tapCount){
				thisEngine.OnTap(tapCount);
			}
			public void OnRelease(){
				thisEngine.OnRelease();
			}
			public void ProcessSwipe(ICustomEventData eventData){
				thisEngine.ProcessSwipe(eventData);
			}
			public void OnScrollerElementDisplace(
				float normalizedCursoredPosition,
				int axis
			){
				thisEngine.OnScrollerElementDisplace(
					normalizedCursoredPosition,
					axis
				);
			}
			public bool IsDrawing(){
				return thisEngine.IsDrawing();
			}
			public bool IsLookingAround(){
				return thisEngine.IsLookingAround();
			}
		/* Camera & Scroller */
			IPlayerCamera thisPlayerCamera;
			public void SetPlayerCamera(IPlayerCamera playerCamera){
				thisPlayerCamera = playerCamera;
			}
			ICoreGameplayInputScroller thisInputScroller;
			public void SetInputScroller(ICoreGameplayInputScroller inputScroller){
				thisInputScroller = inputScroller;
			}
			public void PanCamera(
				float normalizedCameraPosition,
				int axis
			){
				thisPlayerCamera.Pan(
					normalizedCameraPosition,
					axis
				);
			}
			public void ResetCameraZoom(){
				/*  camera's actual FOV always smooth follow target value
					when zooming in/ drawing, the target value itself is interpolated, so the FOV smooth follows a moving target
				*/
				Zoom(0f);
				// thisShootingManager.ResetDraw();
				thisShootingManager.StopDraw();
				// thisSho
			}
			public void ResetCameraPan(){
				thisInputScroller.SnapToCenter();
			}
			float maxZoom{
				get{return thisAdaptor.GetMaxZoom();}
			}
			public float GetMaxZoom(){
				return maxZoom;
			}
			public void Zoom(float normalizedZoom){
				float targetFOV = Mathf.Lerp(thisDefaultFOV, maxZoom, normalizedZoom);
				thisPlayerCamera.SetTargetFOV(targetFOV);
			}
		/* Shooting */
			IShootingManager thisShootingManager;
			public void SetShootingManager(IShootingManager shootingManager){
				thisShootingManager = shootingManager;
			}
			public void Nock(){
				thisShootingManager.NockArrow();
			}
			public void StartDraw(){
				thisShootingManager.StartDraw();
			}
			public void HoldDraw(){
				thisShootingManager.HoldDraw();
			}
			public bool IsHeld(){
				return thisShootingManager.IsHeld();
			}
			public void ReleaseHold(){
				thisShootingManager.ReleaseHold();
			}
			public void StopDraw(){
				thisShootingManager.StopDraw();
			}
			public void Release(){
				thisShootingManager.Release();
			}
			public void DeactivateArrow(){
				thisShootingManager.DeactivateArrow();
			}
			public void ClearAndDeactivateShotInBuffer(){
				thisShootingManager.ClearAndDeactivateShotInBuffer();
			}
			public void Aim(){
				thisShootingManager.Aim();
			}
			public void Unaim(){
				thisShootingManager.Unaim();
			}
		/*  */
			ISmoothFollower thisCameraPivotSmoothFollower;
			public void SetCameraPivotSmoothFollower(
				ISmoothFollower follower
			){
				thisCameraPivotSmoothFollower = follower;
			}
			public Vector3 GetLauncherVelocity(){
				return thisCameraPivotSmoothFollower.GetVelocity();
			}
	}





	public interface IPlayerInputManagerConstArg{
		float defaultFOV{get;}
		float drawDeltaThreshold{get;}
		ISlickBowShootingProcessFactory processFactory{get;}
		IPlayerInputManagerAdaptor adaptor{get;}
	}
	public struct PlayerInputManagerConstArg: IPlayerInputManagerConstArg{
		public PlayerInputManagerConstArg(
			float defaultFOV,
			float drawDeltaThreshold,
			ISlickBowShootingProcessFactory processFactory,
			IPlayerInputManagerAdaptor adaptor

		){
			thisDefaultFOV = defaultFOV;
			thisDrawDeltaThreshold = drawDeltaThreshold;
			thisProcessFactory = processFactory;
			thisAdaptor = adaptor;
		}
		readonly float thisDefaultFOV;
		public float defaultFOV{get{return thisDefaultFOV;}}
		readonly float thisDrawDeltaThreshold;
		public float drawDeltaThreshold{get{return thisDrawDeltaThreshold;}}
		readonly ISlickBowShootingProcessFactory thisProcessFactory;
		public ISlickBowShootingProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly IPlayerInputManagerAdaptor thisAdaptor;
		public IPlayerInputManagerAdaptor adaptor{get{return thisAdaptor;}}
	}
}
