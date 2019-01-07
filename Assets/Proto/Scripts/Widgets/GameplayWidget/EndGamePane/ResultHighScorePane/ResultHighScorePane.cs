﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IResultHighScorePane: IAlphaVisibilityTogglableUIElement{
		void ResetHighScorePane();
		void SetInitialHighScore(int score);
		void SetTargetHighScore(int score);
		void UpdateHighScore(float normalizedTime);
	}
	public class ResultHighScorePane: AlphaVisibilityTogglableUIElement, IResultHighScorePane{
		public ResultHighScorePane(IConstArg arg): base(arg){}
		IResultHighScorePaneAdaptor thisResultHighScorePaneAdaptor{
			get{
				return (IResultHighScorePaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetHighScorePane(){
			// thisResultHighScorePaneAdaptor.SetAlpha(0f);
			Hide(true);
			thisTargetHighScore = 0;
			ClearFields();
		}
		int thisTargetHighScore;
		public void SetTargetHighScore(int score){
			thisTargetHighScore = score;
		}
		int thisInitialHighScore;
		public void SetInitialHighScore(int score){
			thisInitialHighScore = score;
			thisResultHighScorePaneAdaptor.SetScoreText(thisInitialHighScore.ToString());
		}
		
		public void UpdateHighScore(float normalizedTime){
			AnimationCurve highScoreUpdateProcessCurve = thisResultHighScorePaneAdaptor.GetUpdateHighScoreProcessCurve();
			float processValue = highScoreUpdateProcessCurve.Evaluate(normalizedTime);
			int newScore = Mathf.RoundToInt(Mathf.Lerp(
				thisInitialHighScore,
				thisTargetHighScore,
				processValue
			));
			thisResultHighScorePaneAdaptor.SetScoreText(newScore.ToString());
		}
	}
}
