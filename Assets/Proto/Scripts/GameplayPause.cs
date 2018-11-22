using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IGameplayPause: IAppleShooterSceneObject{
		void Pause();
		void Unpause();
		void EnablePauseButtonInput();
	}
	// public class GameplayPause : AbsSceneObject, IGameplayPause {
	// 	public GameplayPause(
	// 		IConstArg arg
	// 	): base(
	// 		arg
	// 	){
	// 	}
	// 	IGameplayPauseAdaptor thisTypedAdaptor{
	// 		get{return (IGamePlayPauseAdaptor)thisAdaptor;}
	// 	}
	// 	ICoreGameplayInputScroller thisInputScroller;
	// 	public void Pause(){
	// 		// thisInputScroller.DisableInputRecursively();
	// 		thisTypedAdaptor.SetTimeScale(0f);
	// 	}
	// 	public void Unpause(){
	// 		// thisInputScroller.EnableInputRecursively();
	// 		StartUnpauseProcess();
	// 	}
	// 	public void EnablePauseButtonInput(){}

	// 	public new interface IConstArg: AbsSceneObject.IConstArg{}
	// 	public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
	// 		public ConstArg(
	// 			IGamePlayPauseAdaptor adaptor
	// 		): base(
	// 			adaptor
	// 		){
	// 		}
	// 	}
	// }
}
