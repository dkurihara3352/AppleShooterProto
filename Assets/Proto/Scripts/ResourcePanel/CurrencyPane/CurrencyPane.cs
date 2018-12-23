using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface ICurrencyPane: IUIElement, IProcessHandler{
		void StartCurrencyUpdateProcess(int currency);
		void UpdateCurrency(int currency);
	}
	public class CurrencyPane: UIElement, ICurrencyPane{
		public CurrencyPane(IConstArg arg): base(arg){
			thisProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.ExpireTime,
				thisCurrencyPaneAdaptor.GetProcessTime()
			);
		}
		IProcessSuite thisProcessSuite;
		ICurrencyPaneAdaptor thisCurrencyPaneAdaptor{
			get{
				return (ICurrencyPaneAdaptor)thisUIAdaptor;
			}
		}
		public void StartCurrencyUpdateProcess(int currency){
			thisInitialCurrency = thisCurrencyPaneAdaptor.GetCurrency();
			thisTargetCurrency = currency;
			thisProcessSuite.Start();
		}
		int thisInitialCurrency;
		int thisTargetCurrency;
		public void OnProcessRun(IProcessSuite suite){
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisProcessSuite){
				AnimationCurve curve = thisCurrencyPaneAdaptor.GetProcessCurve();
				float processValue = curve.Evaluate(normalizedTime);
				int newCurrency = Mathf.RoundToInt(Mathf.Lerp(
					thisInitialCurrency,
					thisTargetCurrency,
					processValue
				));
				thisCurrencyPaneAdaptor.SetCurrency(newCurrency);
			}
		}
		public void OnProcessExpire(
			IProcessSuite suite
		){
			thisCurrencyPaneAdaptor.SetCurrency(thisTargetCurrency);
		}
		public void UpdateCurrency(int currency){
			thisCurrencyPaneAdaptor.SetCurrency(currency);
		}
	}
}


