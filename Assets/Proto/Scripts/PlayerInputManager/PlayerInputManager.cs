using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IPlayerInputManager: IPlayerInputHandler{
		void SetPlayerCamera(IPlayerCamera playerCamera);
		void SetInputScroller(ICoreGameplayInputScroller inputScroller);

		void ResetCameraZoom();
		void ResetCameraPan();
		void PanCamera(
			float normalizedCameraPosition,
			int axis
		);
		void StartDraw();
		void HoldDraw();
		void Fire();

		IPlayerInputState GetCurrentState();
		void SetMaxZoom(float fov);
		void Zoom(float normalizedZoom);
	}
	public class PlayerInputManager : IPlayerInputManager {
		public PlayerInputManager(IPlayerInputManagerConstArg arg){
			thisDefaultFOV = arg.defaultFOV;


			IPlayerInputStateEngineConstArg engineConstArg = new PlayerInputStateEngineConstArg(
				this,
				arg.drawDeltaThreshold,
				arg.processFactory
			);
			thisEngine = new PlayerInputStateEngine(engineConstArg);
			
			IShootingManagerConstArg shootingManagerConstArg = new ShootingManagerConstArg(
				this,
				arg.processFactory
			);
			thisShootingManager = new ShootingManager(shootingManagerConstArg);
		}
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
				thisShootingManager.ResetDraw();
			}
			public void ResetCameraPan(){
				thisInputScroller.SnapToCenter();
			}
			float thisMaxZoom;
			public void SetMaxZoom(float fov){
				thisMaxZoom = fov;
			}
			public void Zoom(float normalizedZoom){
				float targetFOV = Mathf.Lerp(thisDefaultFOV, thisMaxZoom, normalizedZoom);
				thisPlayerCamera.SetFOV(targetFOV);
			}
			readonly IShootingManager thisShootingManager;
		/* Shooting */
			public void StartDraw(){
				thisShootingManager.StartDraw();
			}
			public void HoldDraw(){
				thisShootingManager.HoldDraw();
			}
			public void Fire(){
				thisShootingManager.Fire();
			}
		/*  */
	}





	public interface IPlayerInputManagerConstArg{
		float defaultFOV{get;}
		float drawDeltaThreshold{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public struct PlayerInputManagerConstArg: IPlayerInputManagerConstArg{
		public PlayerInputManagerConstArg(
			float defaultFOV,
			float drawDeltaThreshold,
			IAppleShooterProcessFactory processFactory

		){
			thisDefaultFOV = defaultFOV;
			thisDrawDeltaThreshold = drawDeltaThreshold;
			thisProcessFactory = processFactory;
		}
		readonly float thisDefaultFOV;
		public float defaultFOV{get{return thisDefaultFOV;}}
		readonly float thisDrawDeltaThreshold;
		public float drawDeltaThreshold{get{return thisDrawDeltaThreshold;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}
