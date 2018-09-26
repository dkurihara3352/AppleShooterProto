using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerInputManagerAdaptor{
		IPlayerInputManager GetInputManager();
		/* Debug */
		string GetStateName();
		float GetMaxZoom();
	}
	public class PlayerInputManagerAdaptor : MonoBehaviourAdaptor, IPlayerInputManagerAdaptor {
		public float defaultFOV = 60f;
		public float drawDeltaThreshold = 100f;
		public float maxZoom;
		public float GetMaxZoom(){
			return maxZoom;
		}
		public DKUtility.ProcessManager processManager;
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(processManager);
			IPlayerInputManagerConstArg arg = new PlayerInputManagerConstArg(
				defaultFOV,
				drawDeltaThreshold,
				processFactory,
				this
			);
			thisInputManager = new PlayerInputManager(arg);
		}
		public ShootingManagerAdaptor shootingManagerAdaptor;
		public SmoothFollowerAdaptor cameraPivotSmoothFollowerAdaptor;
		public override void SetUpReference(){
			IPlayerCamera playerCamera = playerCameraAdaptor.GetPlayerCamera();
			thisInputManager.SetPlayerCamera(playerCamera);
			ICoreGameplayInputScroller scroller = inputScrollerAdaptor.GetInputScroller();
			thisInputManager.SetInputScroller(scroller);
			IShootingManager shootingManager = shootingManagerAdaptor.GetShootingManager();
			thisInputManager.SetShootingManager(shootingManager);

			ISmoothFollower follower = cameraPivotSmoothFollowerAdaptor.GetSmoothFollower();
			thisInputManager.SetCameraPivotSmoothFollower(follower);
		}
		public PlayerCameraAdaptor playerCameraAdaptor;
		public CoreGameplayInputScrollerAdaptor inputScrollerAdaptor;
		IPlayerInputManager thisInputManager;
		public IPlayerInputManager GetInputManager(){
			return thisInputManager;
		}
		public string GetStateName(){
			IPlayerInputState currrentState = thisInputManager.GetCurrentState();
			return currrentState.GetName();
		}
	}
}
