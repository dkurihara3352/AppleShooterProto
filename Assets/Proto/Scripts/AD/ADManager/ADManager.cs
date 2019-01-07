using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using DKUtility;

namespace SlickBowShooting{
	public interface IADManager: ISlickBowShootingSceneObject, IProcessHandler{
		void SetADPopUp(IADPopUp popUp);
		void SetADStatusPopUp(IADStatusPopUp popUp);
		void SetEndGamePane(IEndGamePane pane);
		void SetInterstitialADManager(IInterstitialADManager manager);

		void StartRewardedADSequence();
		void StartNonRewardedADSequence();
		void EndADSequence();

		
		// void MarkADRemovalIsAlreadySuggested();

		void TestToggleADReady(bool ready);
	}
	public class ADManager: SlickBowShootingSceneObject, IADManager{
		public ADManager(IConstArg arg): base(arg){
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
		IADManagerAdaptor thisDoubleEarnedCrystalsADManagerAdaptor{
			get{
				return (IADManagerAdaptor)thisAdaptor;
			}
		}

		public void StartRewardedADSequence(){
			thisADIsRewarded = true;
			thisADSequenceProcessSuite.Start();
		}
		public void StartNonRewardedADSequence(){
			thisADIsRewarded = false;
			thisADSequenceProcessSuite.Start();
		}
		bool thisADIsRewarded = false;

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
		string thisInterstitialADPlacementID = "video";
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisADSequenceProcessSuite){
				thisADPopUp.Hide(false);
			}else if(suite == thisGetADReadyProcessSuite){
				thisADPopUp.StopIndicateGetADReady();
				if(thisADIsRewarded)
					ShowRewardedAD();
				else
					ShowNonRewardedAD();
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
		void ShowRewardedAD(){
			ShowOptions options = new ShowOptions();
			options.resultCallback = OnWatchRewardedADComplete;
			Advertisement.Show(thisDoubleEarnedCrystalsADPlacementID, options);

			thisADIsReadyTest = false;
		}
		void ShowNonRewardedAD(){
			ShowOptions options = new ShowOptions();
			options.resultCallback = OnWatchNonRewardedADComplete;
			Advertisement.Show(thisInterstitialADPlacementID, options);

			thisADIsReadyTest = false;
		}
		IADPopUp thisADPopUp;
		public void SetADPopUp(IADPopUp popUp){
			thisADPopUp = popUp;
		}
		void OnWatchRewardedADComplete(ShowResult result){
			// IndicateADCompletionFailure();
			// IndicateADSkip();
			// OnADCompleteSuccess();
			ResetInterstitialADManager();
			switch(result){
				case ShowResult.Failed:
					IndicateADCompletionFailure();
					break;
				case ShowResult.Finished:
					OnRewardedADCompleteSuccess();
					break;
				case ShowResult.Skipped:
					IndicateADSkip();
					break;
				default:
					break;
			}
		}
		void OnWatchNonRewardedADComplete(ShowResult result){
			EndADSequence();
			ResetInterstitialADManager();
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
		void OnRewardedADCompleteSuccess(){
			thisADSequenceProcessSuite.Expire();
			thisEndGamePane.OnWatchADComplete();
		}
		IEndGamePane thisEndGamePane;
		public void SetEndGamePane(IEndGamePane pane){
			thisEndGamePane = pane;
		}

		void ResetInterstitialADManager(){
			if(thisInterstitiaADManager.IsCountingDown())
				thisInterstitiaADManager.StopCounting();
			thisInterstitiaADManager.ResetTimerAndCounter();
		}
		IInterstitialADManager thisInterstitiaADManager;
		public void SetInterstitialADManager(IInterstitialADManager manager){
			thisInterstitiaADManager = manager;
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
