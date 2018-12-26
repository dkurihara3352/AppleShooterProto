using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IEndGamePane: IUIElement, IProcessHandler{
		void SetPlayerDataManager(IPlayerDataManager manager);
		void SetRootScroller(IUIElementGroupScroller scroller);
		void SetResultLabelPane(IResultLabelPane pane);
		void SetResultScorePane(IResultScorePane pane);
		void SetResultHighScorePane(IResultHighScorePane pane);
		void SetResultCurrencyPane(IResultCurrencyPane pane);
		void SetWatchADButton(IWatchADButton button);
		void SetMainMenuButtonCluster(IMainMenuButtonCluster cluster);

		void ActivateThruBackdoor(bool instantly);
		void FeedStats(
			int score,
			int highScore,
			int earnedCurrency,
			int scoreCurrencyBonus
		);
		void StartSequence();
		void ResetEndGamePane();
		void OnWatchADComplete();

	}
	public class EndGamePane: UIElement, IEndGamePane{
		public EndGamePane(IConstArg arg): base(arg){
			thisShowResultLabelMasterProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowResultLabelMasterProcessTime()
			);
			thisShowResultLabelProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowLabelProcessTime()
			);
			thisScoreMasterProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetScoreMasterProcessTime()
			);
			thisShowScoreProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowLabelProcessTime()
			);
			thisShowHighScoreProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowLabelProcessTime()
			);
			thisHighScoreUpdateMasterProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetUpdateHighScoreMasterProcessTime()
			);
			thisHighScoreUpdateProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetUpdateHighScoreProcessTime()
			);
			thisCurrencyMasterProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				/* thisEndGamePaneAdaptor.GetCurrencyMasterProcessTime() */
				CalculateCurrencyMasterProcessTime()
			);
			thisShowCurrencyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowLabelProcessTime()
			);
			thisUpdateCurrencyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetUpdateCurrencyProcessTime()
			);
			thisShowWatchADButtonProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowLabelProcessTime()
			);
			thisShowButtonClusterProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetShowButtonClusterProcessTime()
			);
			thisDoubleCurrencyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisEndGamePaneAdaptor.GetUpdateCurrencyProcessTime()
			);
		}
		IEndGamePaneAdaptor thisEndGamePaneAdaptor{
			get{
				return (IEndGamePaneAdaptor)thisUIAdaptor;
			}
		}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(bool instantly){
			ActivateSelf(instantly);
			ActivateAllChildren(instantly);
		}
		public void FeedStats(
			int score,
			int highScore,
			int earnedCurrency,
			int scoreCurrencyBonus
		){
			thisResultScorePane.SetScore(score);
			thisResultHighScorePane.SetInitialHighScore(highScore);

			if(score > highScore){
				thisRequiresHighScoreUpdate = true;
				thisResultHighScorePane.SetTargetHighScore(score);
			}
			thisCurrencyPane.SetInitialCurrency(earnedCurrency);
			
			int totalAddedCurrency = earnedCurrency + scoreCurrencyBonus;
			thisCurrencyPane.SetTargetCurrency(totalAddedCurrency);
			if(totalAddedCurrency == 0)
				thisRequiresCurrencyUpdate = false;
			else
				thisRequiresCurrencyUpdate = true;

			float recalcedProcessTime = CalculateCurrencyMasterProcessTime();
				thisCurrencyMasterProcessSuite.SetConstraintValue(recalcedProcessTime);
		}
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisShowResultLabelMasterProcessSuite){
				StartShowResultLabelProcess();
				}else if(suite == thisShowResultLabelProcessSuite){
					return;
			}else if(suite == thisScoreMasterProcessSuite){
				StartShowScoreProcess();
				}else if(suite == thisShowScoreProcessSuite){
					return;
				}else if(suite == thisShowHighScoreProcessSuite){
					return;
			}else if(suite == thisHighScoreUpdateMasterProcessSuite){
				StartHighScoreUpdateProcess();
				}else if(suite == thisHighScoreUpdateProcessSuite){
					return;
			}else if(suite == thisCurrencyMasterProcessSuite){
				StartShowCurrencyProcess();
				}else if(suite == thisShowCurrencyProcessSuite){
					return;
				}else if(suite == thisUpdateCurrencyProcessSuite){
					return;
				}else if(suite == thisShowWatchADButtonProcessSuite){
					return;
			}else if(suite == thisShowButtonClusterProcessSuite){
				return;
			}else if(suite == thisDoubleCurrencyProcessSuite){
				return;
			}
		}
		bool thisShowHighScoreProcessIsStarted = false;
		bool thisUpdateCurrencyProcessIsStarted = false;
		bool thisShowWatchADButtonProcessIsStarted = false;
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisShowResultLabelMasterProcessSuite){
				return;
				}else if(suite == thisShowResultLabelProcessSuite){
					UpdateResultLabelShowness(normalizedTime);
			}else if(suite == thisScoreMasterProcessSuite){

				float elapsedTime = normalizedTime * thisEndGamePaneAdaptor.GetScoreMasterProcessTime();

				if(!thisShowHighScoreProcessIsStarted){
					if(elapsedTime >= thisEndGamePaneAdaptor.GetShowHighScoreProcessStartTime()){
						StartShowHighscoreProcess();
						thisShowHighScoreProcessIsStarted = true;
					}
				}

				}else if(suite == thisShowScoreProcessSuite){
					UpdateScoreShowness(normalizedTime);
				}else if(suite == thisShowHighScoreProcessSuite){
					UpdateHighScoreShowness(normalizedTime);
			}else if(suite == thisHighScoreUpdateMasterProcessSuite){
				return;
				}else if(suite == thisHighScoreUpdateProcessSuite){
					UpdateHighScore(normalizedTime);
			}else if(suite == thisCurrencyMasterProcessSuite){

				if(thisRequiresCurrencyUpdate){

					float elapsedTime = normalizedTime * thisEndGamePaneAdaptor.GetCurrencyMasterProcessTime();
					if(!thisUpdateCurrencyProcessIsStarted){
						if(elapsedTime >= thisEndGamePaneAdaptor.GetUpdateCurrencyProcessStartTime()){
							StartUpdateCurrencyProcess();
							thisUpdateCurrencyProcessIsStarted = true;
						}
					}
					if(!thisShowWatchADButtonProcessIsStarted){
						if(elapsedTime >= thisEndGamePaneAdaptor.GetShowWatchADButtonProcessStartTime()){
							StartShowWatchADButtonProcess();
							thisShowWatchADButtonProcessIsStarted = true;
						}
					}
				}

				}else if(suite == thisShowCurrencyProcessSuite){
					UpdateCurrencyShowness(normalizedTime);
				}else if(suite == thisUpdateCurrencyProcessSuite){
					UpdateCurrency(normalizedTime);
				}else if(suite == thisShowWatchADButtonProcessSuite){
					UpdateWatchADButtonShowness(normalizedTime);
			}else if(suite == thisShowButtonClusterProcessSuite){
				UpdateButtonClusterShowness(normalizedTime);
			}else if(suite == thisDoubleCurrencyProcessSuite){
				UpdateCurrency(normalizedTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisShowResultLabelMasterProcessSuite){
				TerminateShowResultLabelProcessAndStartScoreMasterProcess();
				}else if(suite == thisShowResultLabelProcessSuite){
					UpdateResultLabelShowness(1f);		
			}else if(suite == thisScoreMasterProcessSuite){
				TerminateAllScoreProcessAndCheckHighScoreUpdate();
				}else if(suite == thisShowScoreProcessSuite){
					UpdateScoreShowness(1f);
				}else if(suite == thisShowHighScoreProcessSuite){
					UpdateHighScoreShowness(1f);
			}else if(suite == thisHighScoreUpdateMasterProcessSuite){
				TerminateHighScoreUpdateProcessAndStartCurrencyMasterProcess();
				}else if(suite == thisHighScoreUpdateProcessSuite){
					UpdateHighScore(1f);
			}else if(suite == thisCurrencyMasterProcessSuite){
				TerminateAllCurrencyProcessAndStartShowButtonClusterProcess();
				}else if(suite == thisShowCurrencyProcessSuite){
					UpdateCurrencyShowness(1f);
				}else if(suite == thisUpdateCurrencyProcessSuite){
					UpdateCurrency(1f);
				}else if(suite == thisShowWatchADButtonProcessSuite){
					UpdateWatchADButtonShowness(1f);
			}else if(suite == thisShowButtonClusterProcessSuite){
				UpdateButtonClusterShowness(1f);
				thisMainMenuButtonCluster.EnableInput();
				thisRootScroller.EnableInputSelf();
			}else if(suite == thisDoubleCurrencyProcessSuite){
				UpdateCurrency(1f);
			}
		}
		IUIElementGroupScroller thisRootScroller;
		public void SetRootScroller(IUIElementGroupScroller scroller){
			thisRootScroller = scroller;
		}
		protected override void OnTapImple(int tapCount){
			if(thisRunningSkippableProcessSuite != null){
				thisRunningSkippableProcessSuite.Expire();
			}
		}
		IProcessSuite thisRunningSkippableProcessSuite;
		public void StartSequence(){
			// ResetEndGamePane();
			StartShowResultLabelMasterProcess();
		}

		/* ResultLabel */
			void StartShowResultLabelMasterProcess(){
				thisShowResultLabelMasterProcessSuite.Start();
				thisRunningSkippableProcessSuite = thisShowResultLabelMasterProcessSuite;
			}
			IProcessSuite thisShowResultLabelMasterProcessSuite;
				void StartShowResultLabelProcess(){
					thisShowResultLabelProcessSuite.Start();
				}
				IProcessSuite thisShowResultLabelProcessSuite;
				void UpdateResultLabelShowness(float normalizedTime){
					thisResultLabelPane.UpdateShowness(true, normalizedTime);
				}
				IResultLabelPane thisResultLabelPane;
				public void SetResultLabelPane(IResultLabelPane pane){
					thisResultLabelPane = pane;
				}
			void TerminateShowResultLabelProcessAndStartScoreMasterProcess(){
				thisShowResultLabelProcessSuite.Expire();
				StartScoreMasterProcess();
			}
		/* Score */
			void StartScoreMasterProcess(){
				thisScoreMasterProcessSuite.Start();
				thisRunningSkippableProcessSuite = thisScoreMasterProcessSuite;
			}
			IProcessSuite thisScoreMasterProcessSuite;
				void StartShowScoreProcess(){
					thisShowScoreProcessSuite.Start();
				}
				IProcessSuite thisShowScoreProcessSuite;
				void UpdateScoreShowness(float normalizedTime){
					thisResultScorePane.UpdateShowness(true, normalizedTime);
				}
				IResultScorePane thisResultScorePane;
				public void SetResultScorePane(IResultScorePane pane){
					thisResultScorePane = pane;
				}

				void StartShowHighscoreProcess(){
					thisShowHighScoreProcessSuite.Start();
				}
				IProcessSuite thisShowHighScoreProcessSuite;
				void UpdateHighScoreShowness(float normalizedTime){
					thisResultHighScorePane.UpdateShowness(true, normalizedTime);
				}
				IResultHighScorePane thisResultHighScorePane;
				public void SetResultHighScorePane(IResultHighScorePane pane){
					thisResultHighScorePane = pane;
				}
			void TerminateAllScoreProcessAndCheckHighScoreUpdate(){
				thisShowScoreProcessSuite.Expire();
				if(!thisShowHighScoreProcessIsStarted)
					StartShowHighscoreProcess();
				thisShowHighScoreProcessSuite.Expire();
				CheckForHighScoreUpdate();
			}
			void CheckForHighScoreUpdate(){
				if(this.RequiresHighScoreUpdate())
					StartHighScoreUpdateMasterProcess();
				else
					StartCurrencyMasterProcess();

			}
			bool thisRequiresHighScoreUpdate = false;
			bool RequiresHighScoreUpdate(){
				return thisRequiresHighScoreUpdate;
			}
			void StartHighScoreUpdateMasterProcess(){
				thisRunningSkippableProcessSuite = thisHighScoreUpdateMasterProcessSuite;
				thisHighScoreUpdateMasterProcessSuite.Start();
			}
			IProcessSuite thisHighScoreUpdateMasterProcessSuite;
				void StartHighScoreUpdateProcess(){
					thisHighScoreUpdateProcessSuite.Start();
				}
				IProcessSuite thisHighScoreUpdateProcessSuite;
				void UpdateHighScore(float normalizedTime){
					thisResultHighScorePane.UpdateHighScore(normalizedTime);
				}
			void TerminateHighScoreUpdateProcessAndStartCurrencyMasterProcess(){
				thisHighScoreUpdateProcessSuite.Expire();
				StartCurrencyMasterProcess();
			}
		/* Currency */
			void StartCurrencyMasterProcess(){
				thisRunningSkippableProcessSuite = thisCurrencyMasterProcessSuite;
				thisCurrencyMasterProcessSuite.Start();
			}
			IProcessSuite thisCurrencyMasterProcessSuite;
			bool thisRequiresCurrencyUpdate = false;
			float CalculateCurrencyMasterProcessTime(){
				if(thisRequiresCurrencyUpdate)
					return thisEndGamePaneAdaptor.GetCurrencyMasterProcessTime();
				else{
					return thisEndGamePaneAdaptor.GetUpdateCurrencyProcessStartTime();
				}
			}
			void TerminateAllCurrencyProcessAndStartShowButtonClusterProcess(){

				thisShowCurrencyProcessSuite.Expire();

				if(thisRequiresCurrencyUpdate){
					if(!thisUpdateCurrencyProcessIsStarted)
						StartUpdateCurrencyProcess();
					thisUpdateCurrencyProcessSuite.Expire();
					
					if(!thisShowWatchADButtonProcessIsStarted)
						StartShowWatchADButtonProcess();
					thisShowWatchADButtonProcessSuite.Expire();
				}
				StartShowButtonClusterProcess();
			}
			void StartShowCurrencyProcess(){
				thisShowCurrencyProcessSuite.Start();
			}
			IProcessSuite thisShowCurrencyProcessSuite;
			void UpdateCurrencyShowness(float normalizedTime){
				thisCurrencyPane.UpdateShowness(true, normalizedTime);
			}
			IResultCurrencyPane thisCurrencyPane;
			public void SetResultCurrencyPane(IResultCurrencyPane pane){
				thisCurrencyPane = pane;
			}
			void StartUpdateCurrencyProcess(){
				thisUpdateCurrencyProcessSuite.Start();
			}

			IProcessSuite thisUpdateCurrencyProcessSuite;
			void UpdateCurrency(float normalizedTime){
				thisCurrencyPane.UpdateCurrency(normalizedTime);
			}
			void StartShowWatchADButtonProcess(){
				thisWatchADButton.EnableInput();
				thisShowWatchADButtonProcessSuite.Start();
			}
			IProcessSuite thisShowWatchADButtonProcessSuite;
			IWatchADButton thisWatchADButton;
			public void SetWatchADButton(IWatchADButton button){
				thisWatchADButton = button;
			}
			void UpdateWatchADButtonShowness(float normalizedTime){
				thisWatchADButton.UpdateShowness(normalizedTime);
			}
		/* Button Cluster */
			void StartShowButtonClusterProcess(){
				thisShowButtonClusterProcessSuite.Start();
			}
			IProcessSuite thisShowButtonClusterProcessSuite;
			void UpdateButtonClusterShowness(float normalizedTime){
				thisMainMenuButtonCluster.UpdateShowness(true, normalizedTime);
			}
			IMainMenuButtonCluster thisMainMenuButtonCluster;
			public void SetMainMenuButtonCluster(IMainMenuButtonCluster cluster){
				thisMainMenuButtonCluster = cluster;
			}

		/* Reset */
			public void ResetEndGamePane(){
				// thisCurrencyBonus = 0;
				thisRunningSkippableProcessSuite = null;
				thisRequiresHighScoreUpdate = false;
				thisRequiresCurrencyUpdate = false;

				thisShowHighScoreProcessIsStarted = false;
				thisUpdateCurrencyProcessIsStarted = false;
				thisShowWatchADButtonProcessIsStarted = false;

				thisResultLabelPane.ResetResultLabelPane();
				thisResultScorePane.ResetScorePane();
				thisResultHighScorePane.ResetHighScorePane();
				thisCurrencyPane.ResetCurrencyPane();
				thisWatchADButton.ResetWatchADButton();
				thisMainMenuButtonCluster.ResetButtonCluster();
			}
		/* OnWatchADComplete */
			public void OnWatchADComplete(){
				CompleteRemainingSequence();
				StartDoubleCurrencySequence();
				thisWatchADButton.InvalidateForADWatchDone();

			}
			void CompleteRemainingSequence(){
				if(thisCurrencyMasterProcessSuite.IsRunning())
					TerminateAllCurrencyProcessAndStartShowButtonClusterProcess();
				if(!thisShowButtonClusterProcessSuite.IsRunning())
					StartShowButtonClusterProcess();
				thisShowButtonClusterProcessSuite.Expire();
				thisRunningSkippableProcessSuite = null;
			}
			void StartDoubleCurrencySequence(){

				int initialCurrency = thisCurrencyPane.GetTargetCurrency();
				int doubledCurrency = initialCurrency * 2;

				AddCurrencyToPlayerData(initialCurrency);

				thisCurrencyPane.SetInitialCurrency(initialCurrency);
				thisCurrencyPane.SetTargetCurrency(doubledCurrency);

				thisRunningSkippableProcessSuite = thisDoubleCurrencyProcessSuite;
				thisDoubleCurrencyProcessSuite.Start();
			}

			void AddCurrencyToPlayerData(int addedCurrency){
				thisPlayerDataManager.SetFileIndex(0);
				if(!thisPlayerDataManager.PlayerDataIsLoaded())
					thisPlayerDataManager.Load();
				int currency = thisPlayerDataManager.GetCurrency();
				int newCurrency = currency + addedCurrency;
				thisPlayerDataManager.SetCurrency(newCurrency);

				thisPlayerDataManager.Save();
			}
			IPlayerDataManager thisPlayerDataManager;
			public void SetPlayerDataManager(IPlayerDataManager manager){
				thisPlayerDataManager = manager;
			}
			IProcessSuite thisDoubleCurrencyProcessSuite;


	}
}


