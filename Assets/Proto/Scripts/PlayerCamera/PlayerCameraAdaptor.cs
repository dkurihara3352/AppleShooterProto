using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCameraAdaptor{
		IPlayerCamera GetPlayerCamera();
	}
	public class PlayerCameraAdaptor : MonoBehaviourAdaptor, IPlayerCameraAdaptor {
		public Vector2 rotationCoefficient;
		public MonoBehaviourAdaptor lookAtPivot;
		public override void SetUp(){
			IPlayerCameraConstArg arg = new PlayerCameraConstArg(
				rotationCoefficient,
				lookAtPivot
			);
			thisPlayerCamera = new PlayerCamera(
				arg
			);
		}
		IPlayerCamera thisPlayerCamera;
		public SmoothFollowerAdaptor thisLookAtTargetAdaptor;
		public override void SetUpReference(){
			ISmoothFollower lookAtTarget = thisLookAtTargetAdaptor.GetSmoothFollower();
			thisPlayerCamera.SetLookAtTarget(lookAtTarget);
		}
		public IPlayerCamera GetPlayerCamera(){
			return thisPlayerCamera;
		}
	}
}
