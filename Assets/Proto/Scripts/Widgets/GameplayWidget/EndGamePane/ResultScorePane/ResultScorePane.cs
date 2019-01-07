using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IResultScorePane: IAlphaVisibilityTogglableUIElement{
		void ResetScorePane();
		void SetScore(int score);
		int GetScore();
	}
	public class ResultScorePane: AlphaVisibilityTogglableUIElement, IResultScorePane{
		public ResultScorePane(IConstArg arg): base(arg){}
		IResultScorePaneAdaptor thisResultScorePaneAdaptor{
			get{
				return (IResultScorePaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetScorePane(){
			// thisResultScorePaneAdaptor.SetAlpha(0f);
			Hide(true);
			SetScore(0);
			ClearFields();
		}
		public void SetScore(int score){
			thisScore = score;
			thisResultScorePaneAdaptor.SetScoreText(score.ToString());
		}
		int thisScore;
		public int GetScore(){
			return thisScore;
		}
	}
}
