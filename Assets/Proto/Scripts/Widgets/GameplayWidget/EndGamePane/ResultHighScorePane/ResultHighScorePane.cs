using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IResultHighScorePane: IAlphaVisibilityTogglableUIElement{
		void ResetHighScorePane();
	}
	public class ResultHighScorePane: AlphaVisibilityTogglableUIElement, IResultHighScorePane{
		public ResultHighScorePane(IConstArg arg): base(arg){}
		IResultHighScorePaneAdaptor thisResultHighScorePaneAdaptor{
			get{
				return (IResultHighScorePaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetHighScorePane(){
			thisResultHighScorePaneAdaptor.SetAlpha(0f);
		}
	}
}
