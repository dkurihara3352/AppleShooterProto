using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultScorePane: IAlphaVisibilityTogglableUIElement{
		void ResetScorePane();
		void SetScore(int score);
	}
	public class ResultScorePane: AlphaVisibilityTogglableUIElement, IResultScorePane{
		public ResultScorePane(IConstArg arg): base(arg){}
		IResultScorePaneAdaptor thisResultScorePaneAdaptor{
			get{
				return (IResultScorePaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetScorePane(){
			thisResultScorePaneAdaptor.SetAlpha(0f);
			SetScore(0);
		}
		public void SetScore(int score){
			thisResultScorePaneAdaptor.SetScoreText(score.ToString());
		}
	}
}
