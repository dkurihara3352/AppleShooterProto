using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AppleShooterProto{
	public interface IResultCurrencyPane: IAlphaVisibilityTogglableUIElement{
		void ResetCurrencyPane();
		void SetInitialCurrency(int currency);
		void SetTargetCurrency(int currency);
		void UpdateCurrency(float normalizedTime);
	}
	public class ResultCurrencyPane: AlphaVisibilityTogglableUIElement, IResultCurrencyPane{
		public ResultCurrencyPane(IConstArg arg): base(arg){

		}
		IResultCurrencyPaneAdaptor thisResultCurrencyPaneAdaptor{
			get{
				return (IResultCurrencyPaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetCurrencyPane(){
			thisResultCurrencyPaneAdaptor.SetAlpha(0f);
			thisTargetCurrency = 0;
		}
		int thisTargetCurrency;
		public void SetTargetCurrency(int currency){
			thisTargetCurrency = currency;
		}
		int thisInitialCurrency;
		public void SetInitialCurrency(int currency){
			thisInitialCurrency = currency;
		}
		public void UpdateCurrency(float normalizedTime){
			AnimationCurve updateCurrencyProcessCurve = thisResultCurrencyPaneAdaptor.GetUpdateCurrencyProcessCurve();
			float processValue = updateCurrencyProcessCurve.Evaluate(normalizedTime);
			int newCurrency = Mathf.RoundToInt(
				Mathf.Lerp(
					thisInitialCurrency,
					thisTargetCurrency,
					processValue
				)
			);
			thisResultCurrencyPaneAdaptor.SetCurrencyText(newCurrency.ToString());
		}
	}
}

