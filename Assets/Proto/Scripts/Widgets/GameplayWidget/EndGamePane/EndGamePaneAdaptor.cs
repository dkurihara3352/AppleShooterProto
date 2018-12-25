using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IEndGamePaneAdaptor: IUIAdaptor{
		IEndGamePane GetEndGamePane();
		float GetShowResultLabelMasterProcessTime();
		float GetShowLabelProcessTime();
		float GetScoreMasterProcessTime();
		float GetShowHighScoreProcessStartTime();
	}
	public class EndGamePaneAdaptor: UIAdaptor, IEndGamePaneAdaptor{
		protected override IUIElement CreateUIElement(){
			EndGamePane.IConstArg arg = new EndGamePane.ConstArg(
				this,
				activationMode
			);
			return new EndGamePane(arg);
		}
		IEndGamePane thisEndGamePane{
			get{
				return (IEndGamePane)thisUIElement;
			}
		}
		public IEndGamePane GetEndGamePane(){
			return thisEndGamePane;
		}
		public float showResultLabelMasterProcessTime = 1f;
		public float GetShowResultLabelMasterProcessTime(){
			return showResultLabelMasterProcessTime;
		}
		public float showLabelProcessTime = .5f;
		public float GetShowLabelProcessTime(){
			return showLabelProcessTime;
		}
		public float scoreMasterProcessTime = 2f;
		public float GetScoreMasterProcessTime(){
			return scoreMasterProcessTime;
		}
		public float showHighScoreProcessStartTime = 1f;
		public float GetShowHighScoreProcessStartTime(){
			return showHighScoreProcessStartTime;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IResultLabelPane resultLabelPane = resultLabelPaneAdaptor.GetResultLabelPane();
			thisEndGamePane.SetResultLabelPane(resultLabelPane);

			IResultScorePane scorePane = resultScorePaneAdaptor.GetResultScorePane();
			thisEndGamePane.SetResultScorePane(scorePane);

			IResultHighScorePane highScorePane = resultHighScorePaneAdaptor.GetResultHighScorePane();
			thisEndGamePane.SetResultHighScorePane(highScorePane);
		}
		public ResultLabelPaneAdaptor resultLabelPaneAdaptor;
		public ResultScorePaneAdaptor resultScorePaneAdaptor;
		public ResultHighScorePaneAdaptor resultHighScorePaneAdaptor;
	}
}

