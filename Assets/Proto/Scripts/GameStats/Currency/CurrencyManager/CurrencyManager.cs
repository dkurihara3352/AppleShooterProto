using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICurrencyManager: IAppleShooterSceneObject{
		void SetGainedCurrencyImage(IGainedCurrencyImage image);
		void AddGainedCurrency(int addedCurrency);
		int GetGainedCurrency();
		void ClearGainedCurrency();
	}
	public class CurrencyManager: AppleShooterSceneObject, ICurrencyManager{

		public CurrencyManager(IConstArg arg): base(arg){}

		IGainedCurrencyImage thisGainedCurrencyImage;
		public void SetGainedCurrencyImage(IGainedCurrencyImage image){
			thisGainedCurrencyImage = image;
		}
		int thisGainedCurrency = 0;
		public void AddGainedCurrency(int addedCurrency){
			thisGainedCurrency += addedCurrency;
			thisGainedCurrencyImage.UpdateCurrencyText(thisGainedCurrency);
		}
		public int GetGainedCurrency(){
			return thisGainedCurrency;
		}
		public void ClearGainedCurrency(){
			thisGainedCurrency = 0;
			thisGainedCurrencyImage.UpdateCurrencyText(thisGainedCurrency);
		}
	}
}
