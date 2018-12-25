using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultScorePane: IAlphaVisibilityTogglableUIElement{
		void ResetScorePane();
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
		}
	}
}
