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
