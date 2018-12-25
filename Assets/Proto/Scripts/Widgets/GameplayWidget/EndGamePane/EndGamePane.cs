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
				}
		}
		bool thisShowHighScoreProcessIsStarted = false;
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
				UpdateScoreShowness(1f);
				UpdateHighScoreShowness(1f);
			}

		/* Reset */
			public void ResetEndGamePane(){
				thisRunningSkippableProcessSuite = null;
				thisShowHighScoreProcessIsStarted = false;
				thisResultLabelPane.ResetResultLabelPane();
				thisResultScorePane.ResetScorePane();
				thisResultHighScorePane.ResetHighScorePane();
			}


	}
}


