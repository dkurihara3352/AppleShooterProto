using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerInputManager{
		void SetPlayerCamera(IPlayerCamera playerCamera);
		void LookAround(
			float normalizedCameraPosition,
			int axis
		);
		void PanCamera(
			float normalizedCameraPosition,
			int axis
		);
	}
	public class PlayerInputManager : IPlayerInputManager {
		public PlayerInputManager(/* IPlayerInputManagerConstArg arg */){
		}
		// IPlayerInputManagerStateEngine thisEngine;
		/*  Player Input State
				Idle
					enter
						CameraLookAtTarget start to smooth follow default position
					OnBeginDrag
						to LookingAround
					OnTouch
						to LookingAroung?
					
				LookingAround/Aiming
					enter
						CameraLookAtTarget stops following default position
					when deltaPos is below threshold
						to Drawing
				Drawing
					enter
						Zoom
							Change FOV and scroller coeff
					when deltaPos is above threshold
						to Looking
		*/
		bool panHasStarted = false;
		public void LookAround(
			float normalizedCameraPosition,
			int axis
		){
			// thisEngine.LookAround(
			// 	normalizedCameraPosition,
			// 	axis
			// );
			if(!panHasStarted){
				panHasStarted = true;
				thisPlayerCamera.StartPan();
			}

			thisPlayerCamera.Pan(
				normalizedCameraPosition,
				axis
			);
		}
		IPlayerCamera thisPlayerCamera;
		public void SetPlayerCamera(IPlayerCamera playerCamera){
			thisPlayerCamera = playerCamera;
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
	}


/* 	public interface IPlayerInputManagerConstArg{
	}
	public struct PlayerInputManagerConstArg: IPlayerInputManagerConstArg{
		public PlayerInputManagerConstArg(
		){
		}
	} */
}
