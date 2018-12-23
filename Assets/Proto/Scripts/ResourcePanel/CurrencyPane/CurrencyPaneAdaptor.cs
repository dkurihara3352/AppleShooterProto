using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface ICurrencyPaneAdaptor: IUIAdaptor{
		ICurrencyPane GetCurrencyPane();
		int GetCurrency();
		void SetCurrency(int currency);
		float GetProcessTime();
		AnimationCurve GetProcessCurve();
	}
	public class CurrencyPaneAdaptor: UIAdaptor, ICurrencyPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			CurrencyPane.IConstArg arg = new CurrencyPane.ConstArg(
				this,
				activationMode
			);
			return new CurrencyPane(arg);
		}
		ICurrencyPane thisCurrencyPane{
			get{
				return (ICurrencyPane)thisUIElement;
			}
		}
		public ICurrencyPane GetCurrencyPane(){
			return thisCurrencyPane;
		}
		public Text textComp;
		int thisCurrency;
		public int GetCurrency(){
			return thisCurrency;
		}
		public void SetCurrency(int currency){
			thisCurrency = currency;
			this.textComp.text = thisCurrency.ToString();
		}
		public float processTime = 1f;
		public float GetProcessTime(){
			return processTime;
		}
		public AnimationCurve processCurve;
		public AnimationCurve GetProcessCurve(){
			return processCurve;
		}
	}
}


