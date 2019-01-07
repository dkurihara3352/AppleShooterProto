using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGainedCurrencyImage: ISlickBowShootingSceneObject{
		void UpdateCurrencyText(int currency);
	}
	public class GainedCurrencyImage: SlickBowShootingSceneObject, IGainedCurrencyImage{
		public GainedCurrencyImage(IConstArg arg): base(arg){}
		IGainedCurrencyImageAdaptor thisGainedCurrencyImageAdaptor{
			get{
				return(IGainedCurrencyImageAdaptor)thisAdaptor;
			}
		}
		public void UpdateCurrencyText(int currency){
			thisGainedCurrencyImageAdaptor.SetText(currency.ToString());
		}
	}
}
