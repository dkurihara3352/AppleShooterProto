using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlickBowShooting{
	public interface IGainedCurrencyImageAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IGainedCurrencyImage GetGainedCurrencyImage();
		void SetText(string text);
	}
	public class GainedCurrencyImageAdaptor: SlickBowShootingMonoBehaviourAdaptor, IGainedCurrencyImageAdaptor{
		public override void SetUp(){
			thisGainedCurrencyImage = CreateGainedCurrencyImage();
		}
		IGainedCurrencyImage thisGainedCurrencyImage;
		public IGainedCurrencyImage GetGainedCurrencyImage(){
			return thisGainedCurrencyImage;
		}
		IGainedCurrencyImage CreateGainedCurrencyImage(){
			GainedCurrencyImage.IConstArg arg = new GainedCurrencyImage.ConstArg(this);
			return new GainedCurrencyImage(arg);
		}
		public Text textComp;
		public void SetText(string text){
			textComp.text = text;
		}
		
	}
}

