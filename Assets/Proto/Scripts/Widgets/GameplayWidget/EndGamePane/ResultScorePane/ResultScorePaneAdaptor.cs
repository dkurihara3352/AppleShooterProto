using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IResultScorePaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultScorePane GetResultScorePane();
		void SetScoreText(string text);
	}
	public class ResultScorePaneAdaptor: AlphaVisibilityTogglableUIAdaptor, IResultScorePaneAdaptor{
		protected override IUIElement CreateUIElement(){
			ResultScorePane.IConstArg arg = new ResultScorePane.ConstArg(
				this,
				activationMode
			);
			return new ResultScorePane(arg);
		}
		IResultScorePane thisScorePane{
			get{
				return (IResultScorePane)thisUIElement;
			}
		}
		public IResultScorePane GetResultScorePane(){
			return thisScorePane;
		}
		public UnityEngine.UI.Text scoreTextComp;
		public void SetScoreText(string text){
			scoreTextComp.text = text;
		}
	}
}
