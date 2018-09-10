using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCamera{
		void StartPan();
		void EndPan();
		void Pan(
			float normalizedCameraPosition,
			int axis
		);
		void SetLookAtTarget(ISmoothFollower lookAtTarget);
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
		}
		ISmoothFollower thisCameraLookAtTarget;
		public void SetLookAtTarget(ISmoothFollower lookAtTarget){
			thisCameraLookAtTarget = lookAtTarget;
		}
		public void StartPan(){
			thisCameraLookAtTarget.StopFollow();
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
		public void EndPan(){
			ResetLookAtPivotRotation();
			thisCameraLookAtTarget.StartFollow();
		}
		void ResetLookAtPivotRotation(){
			thisLookAtPivot.Rotate(Vector3.zero);/* start process! */
		}
	}


	public interface IPlayerCameraConstArg{
		Vector2 rotationCoefficient{get;}
		IMonoBehaviourAdaptor lookAtPivot{get;}
	}
	public struct PlayerCameraConstArg: IPlayerCameraConstArg{
		public PlayerCameraConstArg(
			Vector2 rotationCoefficient,
			IMonoBehaviourAdaptor lookAtPivot

		){
			thisRotationCoefficient = rotationCoefficient;
			thisLookAtPivot = lookAtPivot;
		}
		readonly Vector2 thisRotationCoefficient;
		public Vector2 rotationCoefficient{get{return thisRotationCoefficient;}}
		readonly IMonoBehaviourAdaptor thisLookAtPivot;
		public IMonoBehaviourAdaptor lookAtPivot{get{return thisLookAtPivot;}}
	}
}
