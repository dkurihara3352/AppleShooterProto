using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCamera{
		void Pan(
			float normalizedCameraPosition,
			int axis
		);
		void SetInputScroller(ICoreGameplayInputScroller scroller);
		void InitializeFOV();
		void StartSmoothZoom();
		void SetTargetFOV(float fov);
		float GetTargetFOV();
		float GetCurrentFOV();
		void SetFOV(float fov);
	}
	public class PlayerCamera : IPlayerCamera {
		/*  Make CameraParents the parent of both lookAtTarget and the Camera itself
				this makes sure cam and target moves the same
			Hiearchy
				CameraPivot
					CameraParent
						Camera
						LookAtTargetPivot
							LookAtTarget
			Make CameraParent the smoothFollower, not the camera
			Rotate the LookAtPivot so that the lookAtTarget changes its world pos, pannig the camera
			CameraPivot is located at lookAtTargetDefaultPos
				Rotating CameraPivot to change direction of everything as a whole 
		*/
		public PlayerCamera(IPlayerCameraConstArg arg){
			thisRotationCoefficient = arg.rotationCoefficient;
			thisLookAtPivot = arg.lookAtPivot;
			thisCamera = arg.camera;
			thisDefaultFOV = arg.defaultFOV;

			float defFOVInRadian = Mathf.Deg2Rad * thisDefaultFOV;
			thisDefaultTangent = Mathf.Tan(defFOVInRadian);

			thisProcessFactory = arg.processFactory;
			thisSmoothCoefficient = arg.smoothCoefficient;
		}
		public void InitializeFOV(){
			SetTargetFOV(thisDefaultFOV);
			SetFOV(thisDefaultFOV);
		}
		readonly Vector2 thisRotationCoefficient;
		IMonoBehaviourAdaptor thisLookAtPivot;
		public void Pan(
			float normalizedCameraPosition,
			int axis
		){
			float adjustedNormalizedCursorPos = normalizedCameraPosition - .5f;
			float angleOnAxis = thisRotationCoefficient[axis] * adjustedNormalizedCursorPos;
			if(axis == 0)
				axis = 1;
			else
				axis = 0;

			thisLookAtPivot.Rotate(angleOnAxis, axis);
		}
		void StartSmoothFollowTargetFOV(){
			/*  
				Start a process where camera's fov (and scroller's multiplier) constantly smooth follow target values and call SetFOV
			*/
		}
		readonly Camera thisCamera;
		readonly float thisDefaultFOV;
		readonly float thisDefaultTangent;
		ICoreGameplayInputScroller thisInputScroller;
		public void SetInputScroller(ICoreGameplayInputScroller scroller){
			thisInputScroller = scroller;
		}

		readonly IAppleShooterProcessFactory thisProcessFactory;
		readonly float thisSmoothCoefficient;
		public void StartSmoothZoom(){
			ISmoothZoomProcess process = thisProcessFactory.CreateSmoothZoomProcess(
				this,
				thisSmoothCoefficient
			);
			process.Run();
		}
		public float GetCurrentFOV(){
			return thisCamera.fieldOfView;
		}
		float thisTargetFOV;
		public void SetTargetFOV(float fov){
			thisTargetFOV = fov;
		}
		public float GetTargetFOV(){
			return thisTargetFOV;
		}
		public void SetFOV(float fov){
			/*  
				Discount the displacement of scroller element on drag and swipe
					put a multiplier on deltaPos/ velocity for modification

			*/
			thisCamera.fieldOfView = fov;
			float relativeScreenSize = CalculateRelativeScreenSize(fov);
			thisInputScroller.SetScrollMultiplier(relativeScreenSize);
		}
		float CalculateRelativeScreenSize(float fov){
			float targetFOVInRadian = Mathf.Deg2Rad * fov;
			float result = Mathf.Tan(targetFOVInRadian) / thisDefaultTangent;
			return result;
		}
	}




	public interface IPlayerCameraConstArg{
		Vector2 rotationCoefficient{get;}
		IMonoBehaviourAdaptor lookAtPivot{get;}
		Camera camera{get;}
		float defaultFOV{get;}
		IAppleShooterProcessFactory processFactory{get;}
		float smoothCoefficient{get;}
	}
	public struct PlayerCameraConstArg: IPlayerCameraConstArg{
		public PlayerCameraConstArg(
			Vector2 rotationCoefficient,
			IMonoBehaviourAdaptor lookAtPivot,
			Camera camera,
			float defaultFOV,
			IAppleShooterProcessFactory processFactory,
			float smoothCoefficient
		){
			thisRotationCoefficient = rotationCoefficient;
			thisLookAtPivot = lookAtPivot;
			thisCamera = camera;
			thisDefaultFOV = defaultFOV;
			thisProcessFactory = processFactory;
			thisSmoothCoefficient = smoothCoefficient;
		}
		readonly Vector2 thisRotationCoefficient;
		public Vector2 rotationCoefficient{get{return thisRotationCoefficient;}}
		readonly IMonoBehaviourAdaptor thisLookAtPivot;
		public IMonoBehaviourAdaptor lookAtPivot{get{return thisLookAtPivot;}}
		readonly Camera thisCamera;
		public Camera camera{get{return thisCamera;}}
		readonly float thisDefaultFOV;
		public float defaultFOV{get{return thisDefaultFOV;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
		readonly float thisSmoothCoefficient;
		public float smoothCoefficient{get{return thisSmoothCoefficient;}}
	}
}
