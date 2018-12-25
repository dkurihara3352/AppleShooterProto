using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultHighScorePaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultHighScorePane GetResultHighScorePane();
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
	}
}


