using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGainedCurrencyImage: IAppleShooterSceneObject{
		void UpdateCurrencyText(int currency);
	}
	public class GainedCurrencyImage: AppleShooterSceneObject, IGainedCurrencyImage{
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
