using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultScorePaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultScorePane GetResultScorePane();
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
	}
}
