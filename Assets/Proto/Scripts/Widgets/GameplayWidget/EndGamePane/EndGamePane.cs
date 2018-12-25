using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IEndGamePane: IUIElement, IProcessHandler{
		void SetResultLabelPane(IResultLabelPane pane);
		void SetResultScorePane(IResultScorePane pane);
		void SetResultHighScorePane(IResultHighScorePane pane);
		void SetResultCurrencyPane(IResultCurrencyPane pane);
		void SetWatchADButton(IWatchADButton button);

		void StartSequence();
		void ResetEndGamePane();
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
				thisEndGamePaneAdaptor.GetCurrencyMasterProcessTime()
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
		}
		IEndGamePaneAdaptor thisEndGamePaneAdaptor{
			get{
				return (IEndGamePaneAdaptor)thisUIAdaptor;
			}
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

				}else if(suite == thisShowCurrencyProcessSuite){
					UpdateCurrencyShowness(normalizedTime);
				}else if(suite == thisUpdateCurrencyProcessSuite){
					UpdateCurrency(normalizedTime);
				}else if(suite == thisShowWatchADButtonProcessSuite){
					UpdateWatchADButtonShowness(normalizedTime);
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
				}
		}
		protected override void OnTapImple(int tapCount){
			if(thisRunningSkippableProcessSuite != null){
				thisRunningSkippableProcessSuite.Expire();
			}
		}
		IProcessSuite thisRunningSkippableProcessSuite;
		public void StartSequence(){
			ResetEndGamePane();
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
					thisResultLabelPane.UpdateShowness(normalizedTime);
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
					thisResultScorePane.UpdateShowness(normalizedTime);
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
					thisResultHighScorePane.UpdateShowness(normalizedTime);
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
			}
			bool RequiresHighScoreUpdate(){
				return true;
			}
			void StartHighScoreUpdateMasterProcess(){
				thisRunningSkippableProcessSuite = thisHighScoreUpdateMasterProcessSuite;
				thisHighScoreUpdateMasterProcessSuite.Start();
			}
			IProcessSuite thisHighScoreUpdateMasterProcessSuite;
				void StartHighScoreUpdateProcess(){
					int targetHighScore = 100;
					thisResultHighScorePane.SetTargetHighScore(targetHighScore);
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
			void TerminateAllCurrencyProcessAndStartShowButtonClusterProcess(){

				thisShowCurrencyProcessSuite.Expire();

				if(!thisUpdateCurrencyProcessIsStarted)
					thisUpdateCurrencyProcessSuite.Start();
				thisUpdateCurrencyProcessSuite.Expire();
				
				if(!thisShowWatchADButtonProcessIsStarted)
					thisShowWatchADButtonProcessSuite.Start();
				thisShowWatchADButtonProcessSuite.Expire();
				// StartShowButtonClusterProcess();
			}
			void StartShowCurrencyProcess(){
				thisShowCurrencyProcessSuite.Start();
			}
			IProcessSuite thisShowCurrencyProcessSuite;
			void UpdateCurrencyShowness(float normalizedTime){
				thisCurrencyPane.UpdateShowness(normalizedTime);
			}
			IResultCurrencyPane thisCurrencyPane;
			public void SetResultCurrencyPane(IResultCurrencyPane pane){
				thisCurrencyPane = pane;
			}
			void StartUpdateCurrencyProcess(){
				int targetCurrency = 10;
				thisCurrencyPane.SetTargetCurrency(targetCurrency);
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
		/* Reset */
			public void ResetEndGamePane(){
				thisRunningSkippableProcessSuite = null;

				thisShowHighScoreProcessIsStarted = false;
				thisUpdateCurrencyProcessIsStarted = false;
				thisShowWatchADButtonProcessIsStarted = false;

				thisResultLabelPane.ResetResultLabelPane();
				thisResultScorePane.ResetScorePane();
				thisResultHighScorePane.ResetHighScorePane();
				thisCurrencyPane.ResetCurrencyPane();
				thisWatchADButton.ResetWatchADButton();
			}


	}
}


