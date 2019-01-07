using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using DKUtility;

namespace AppleShooterProto{
	public interface IDoubleEarnedCrystalsADManager: IAppleShooterSceneObject, IProcessHandler{
		void SetADPopUp(IADPopUp popUp);
		void SetADStatusPopUp(IADStatusPopUp popUp);
		void SetEndGamePane(IEndGamePane pane);
		void StartADSequence();
		void EndADSequence();
		void OnWatchADComplete(ShowResult result);
		// void MarkADRemovalIsAlreadySuggested();

		void TestToggleADReady(bool ready);
	}
	public class DoubleEarnedCrystalsADManager: AppleShooterSceneObject, IDoubleEarnedCrystalsADManager{
		public DoubleEarnedCrystalsADManager(IConstArg arg): base(arg){
			thisADSequenceProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.None,
				0f
			);
			thisGetADReadyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.None,
				0f
			);
		}
		IDoubleEarnedCrystalsADManagerAdaptor thisDoubleEarnedCrystalsADManagerAdaptor{
			get{
				return (IDoubleEarnedCrystalsADManagerAdaptor)thisAdaptor;
			}
		}

		public void StartADSequence(){
			thisADSequenceProcessSuite.Start();
			/*  Show popUp at start
				hide when expired
			*/

		}
		IProcessSuite thisADSequenceProcessSuite;
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisADSequenceProcessSuite){
				thisADPopUp.Show(instantly: false);
				StartGettingADReady();
			}else if(suite == thisGetADReadyProcessSuite){
				thisADPopUp.StartIndicateGetADReady();
			}
		}
		void StartGettingADReady(){
			thisGetADReadyProcessSuite.Start();
		}
		IProcessSuite thisGetADReadyProcessSuite;
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisGetADReadyProcessSuite){
				if(IsADReady())
					thisGetADReadyProcessSuite.Expire();
			}
		}
		string thisDoubleEarnedCrystalsADPlacementID = "rewardedVideo";
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisADSequenceProcessSuite){
				thisADPopUp.Hide(false);
			}else if(suite == thisGetADReadyProcessSuite){
				thisADPopUp.StopIndicateGetADReady();
				ShowAD();
			}
		}
		bool IsADReady(){
			return Advertisement.IsReady(thisDoubleEarnedCrystalsADPlacementID);
			// return thisADIsReadyTest;
		}
		bool thisADIsReadyTest = false;
		public void TestToggleADReady(bool ready){
			thisADIsReadyTest = ready;
		}
		void ShowAD(){
			ShowOptions options = new ShowOptions();
			options.resultCallback = OnWatchADComplete;
			Advertisement.Show(thisDoubleEarnedCrystalsADPlacementID, options);

			thisADIsReadyTest = false;
		}
		IADPopUp thisADPopUp;
		public void SetADPopUp(IADPopUp popUp){
			thisADPopUp = popUp;
		}
		public void OnWatchADComplete(ShowResult result){
			// OnADCompleteSuccess();
			switch(result){
				case ShowResult.Failed:
					IndicateADCompletionFailure();
					break;
				case ShowResult.Finished:
					OnADCompleteSuccess();
					break;
				case ShowResult.Skipped:
					IndicateADSkip();
					break;
				default:
					break;
			}
		}
		void IndicateADCompletionFailure(){
			ShowADCompletionFailurePopUp();
		}
		void ShowADCompletionFailurePopUp(){
			thisADStatusPopUp.Show(false);
			thisADStatusPopUp.SetText("AD didn't finish properly");

		}
		IADStatusPopUp thisADStatusPopUp;
		public void SetADStatusPopUp(IADStatusPopUp popUp){
			thisADStatusPopUp = popUp;
		}

		public void EndADSequence(){
			if(thisADSequenceProcessSuite.IsRunning())
				thisADSequenceProcessSuite.Expire();
		}
		void IndicateADSkip(){
			thisADStatusPopUp.Show(false);
			thisADStatusPopUp.SetText("AD skipped");
		}
		void OnADCompleteSuccess(){
			thisADSequenceProcessSuite.Expire();
			thisEndGamePane.OnWatchADComplete();
		}
		IEndGamePane thisEndGamePane;
		public void SetEndGamePane(IEndGamePane pane){
			thisEndGamePane = pane;
		}
		// bool ADRemovalHasBeenSuggested(){
		// 	return thisADRemovalIsSuggested;
		// }
		// bool thisADRemovalIsSuggested = false;
		// public void MarkADRemovalIsAlreadySuggested(){
		// 	thisADRemovalIsSuggested = true;
		// }
	}
}
