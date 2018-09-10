using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerInputManagerAdaptor{
		IPlayerInputManager GetInputManager();
	}
	public class PlayerInputManagerAdaptor : MonoBehaviourAdaptor, IPlayerInputManagerAdaptor {
		public override void SetUp(){
			/* IPlayerInputManagerConstArg arg = new PlayerInputManagerConstArg(
			); */
			thisInputManager = new PlayerInputManager(/* arg */);
		}
		public override void SetUpReference(){
			IPlayerCamera playerCamera = playerCameraAdaptor.GetPlayerCamera();
			thisInputManager.SetPlayerCamera(playerCamera);
		}
		public PlayerCameraAdaptor playerCameraAdaptor;
		IPlayerInputManager thisInputManager;
		public IPlayerInputManager GetInputManager(){
			return thisInputManager;
		}
	}
}
