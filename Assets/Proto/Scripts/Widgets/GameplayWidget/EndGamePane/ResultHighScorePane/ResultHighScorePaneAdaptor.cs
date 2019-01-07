using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IResultHighScorePaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultHighScorePane GetResultHighScorePane();
		AnimationCurve GetUpdateHighScoreProcessCurve();
		void SetScoreText(string text);
	}
	public class ResultHighScorePaneAdaptor: AlphaVisibilityTogglableUIAdaptor, IResultHighScorePaneAdaptor{
		protected override IUIElement CreateUIElement(){
			ResultHighScorePane.IConstArg arg = new ResultHighScorePane.ConstArg(
				this,
				activationMode
			);
			return new ResultHighScorePane(arg);
		}
		IResultHighScorePane thisPane{
			get{
				return (IResultHighScorePane)thisUIElement;
			}
		}
		public IResultHighScorePane GetResultHighScorePane(){
			return thisPane;
		}
		public AnimationCurve updateHighScoreProcessCurve;
		public AnimationCurve GetUpdateHighScoreProcessCurve(){
			return updateHighScoreProcessCurve;
		}
		public UnityEngine.UI.Text textComp;
		public void SetScoreText(string text){
			textComp.text = text;
		}
	}
}


