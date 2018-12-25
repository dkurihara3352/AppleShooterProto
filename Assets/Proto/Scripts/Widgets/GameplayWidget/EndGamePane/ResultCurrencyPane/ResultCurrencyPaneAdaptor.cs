using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultCurrencyPaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultCurrencyPane GetResultCurrencyPane();
		AnimationCurve GetUpdateCurrencyProcessCurve();
		void SetCurrencyText(string text);
	}
	public class ResultCurrencyPaneAdaptor: AlphaVisibilityTogglableUIAdaptor, IResultCurrencyPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			ResultCurrencyPane.IConstArg arg = new ResultCurrencyPane.ConstArg(
				this,
				activationMode
			);
			return new ResultCurrencyPane(arg);
		}
		IResultCurrencyPane thisPane{
			get{
				return (IResultCurrencyPane)thisUIElement;
			}
		}
		public IResultCurrencyPane GetResultCurrencyPane(){
			return thisPane;
		}
		public AnimationCurve thisUpdateCurrencyProcessCurve;
		public AnimationCurve GetUpdateCurrencyProcessCurve(){
			return thisUpdateCurrencyProcessCurve;
		}
		public UnityEngine.UI.Text currencyTextComp;
		public void SetCurrencyText(string text){
			currencyTextComp.text = text;
		}
	}
}


