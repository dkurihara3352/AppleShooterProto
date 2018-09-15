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
		public float defaultFOV = 60f;
		public Camera mainCamera;
		public override void SetUp(){
			IPlayerCameraConstArg arg = new PlayerCameraConstArg(
				rotationCoefficient,
				lookAtPivot,
				mainCamera,
				defaultFOV
			);
			thisPlayerCamera = new PlayerCamera(
				arg
			);
		}
		public CoreGameplayInputScrollerAdaptor thisScrollerAdaptor;
		public override void SetUpReference(){
			ICoreGameplayInputScroller scroller = thisScrollerAdaptor.GetInputScroller();
			thisPlayerCamera.SetInputScroller(scroller);
			thisPlayerCamera.InitializeFOV();
		}
		IPlayerCamera thisPlayerCamera;
		public IPlayerCamera GetPlayerCamera(){
			return thisPlayerCamera;
		}
	}
}
