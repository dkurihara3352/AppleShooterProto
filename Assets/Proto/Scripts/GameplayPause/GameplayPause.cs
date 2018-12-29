using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
namespace AppleShooterProto{
	public interface IGameplayPause: IAppleShooterSceneObject{
		void SetInputScroller(ICoreGameplayInputScroller scroller);
		void SetGameplayPauseButton(IGameplayPauseButton button);
		void SetPauseMenuPopUp(IPopUp popUp);

		void Pause();
		void Unpause();
		void ActivatePauseButton();
		void DeactivatePauseButton();
		void SetTimeScale(float scale);
		float GetTimeScale();

		void ExpireUnpauseProcess();
	}
	public class GameplayPause : AppleShooterSceneObject, IGameplayPause {
		public GameplayPause(
			IConstArg arg
		): base(
			arg
		){
			thisUnpauseTime = arg.unpauseTime;
		}
		IGameplayPauseAdaptor thisTypedAdaptor{
			get{return (IGameplayPauseAdaptor)thisAdaptor;}
		}
		ICoreGameplayInputScroller thisInputScroller;
		public void SetInputScroller(ICoreGameplayInputScroller scroller){
			thisInputScroller = scroller;
		}
		IPopUp thisPauseMenuPopUp;
		public void SetPauseMenuPopUp(IPopUp popUp){
			thisPauseMenuPopUp = popUp;
		}
		public void Pause(){
			thisInputScroller.DisableInputRecursively();//just to be sure, may not be needed
			SetTimeScale(0f);
			thisPauseMenuPopUp.Show(true);
			thisPauseButton.DeactivateRecursively(true);
		}
		public void Unpause(){
			thisInputScroller.EnableInputRecursively();
			StartUnpauseProcess();
			thisPauseMenuPopUp.Hide(false);
		}
		IGameplayUnpauseProcess thisProcess;
		readonly float thisUnpauseTime; 
		void StartUnpauseProcess(){
			StopUnpauseProcess();
			thisProcess = thisAppleShooterProcessFactory.CreateGameplayUnpauseProcess(
				this,
				thisUnpauseTime
			);
			thisProcess.Run();
		}
		void StopUnpauseProcess(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		IGameplayPauseButton thisPauseButton;
		public void SetGameplayPauseButton(IGameplayPauseButton button){
			thisPauseButton = button;
		}
		public void ActivatePauseButton(){
			// thisPauseButton.ActivateRecursively(false);
			// thisPauseButton.EvaluateScrollerFocusRecursively();
			thisPauseButton.ActivateThruBackdoor();
		}
		public void DeactivatePauseButton(){
			thisPauseButton.DeactivateRecursively(true);
		}
		public void SetTimeScale(float scale){
			thisTypedAdaptor.SetTimeScale(scale);
		}
		public float GetTimeScale(){
			return thisTypedAdaptor.GetTimeScale();
		}
		public void ExpireUnpauseProcess(){
			if(thisProcess != null && thisProcess.IsRunning()){
				thisProcess.Expire();
			}
			thisProcess = null;
		}
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
			float unpauseTime{get;}
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGameplayPauseAdaptor adaptor,
				float unpauseTime
			): base(
				adaptor
			){
				thisUnpauseTime = unpauseTime;
			}
			readonly float thisUnpauseTime;
			public float unpauseTime{get{return thisUnpauseTime;}}
		}
	}
}
