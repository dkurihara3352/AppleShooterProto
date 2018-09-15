using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCamera{
		void Pan(
			float normalizedCameraPosition,
			int axis
		);
		void SetFOV(float fov);
		void SetInputScroller(ICoreGameplayInputScroller scroller);
		void InitializeFOV();
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

			thisDefaultTangent = Mathf.Tan(thisDefaultFOV);
		}
		public void InitializeFOV(){
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
			return Mathf.Tan(fov) / thisDefaultTangent;
		}
	}




	public interface IPlayerCameraConstArg{
		Vector2 rotationCoefficient{get;}
		IMonoBehaviourAdaptor lookAtPivot{get;}
		Camera camera{get;}
		float defaultFOV{get;}
	}
	public struct PlayerCameraConstArg: IPlayerCameraConstArg{
		public PlayerCameraConstArg(
			Vector2 rotationCoefficient,
			IMonoBehaviourAdaptor lookAtPivot,
			Camera camera,
			float defaultFOV

		){
			thisRotationCoefficient = rotationCoefficient;
			thisLookAtPivot = lookAtPivot;
			thisCamera = camera;
			thisDefaultFOV = defaultFOV;
		}
		readonly Vector2 thisRotationCoefficient;
		public Vector2 rotationCoefficient{get{return thisRotationCoefficient;}}
		readonly IMonoBehaviourAdaptor thisLookAtPivot;
		public IMonoBehaviourAdaptor lookAtPivot{get{return thisLookAtPivot;}}
		readonly Camera thisCamera;
		public Camera camera{get{return thisCamera;}}
		readonly float thisDefaultFOV;
		public float defaultFOV{get{return thisDefaultFOV;}}
	}
}
