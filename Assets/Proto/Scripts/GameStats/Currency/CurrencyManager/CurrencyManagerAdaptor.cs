using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ICurrencyManagerAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		ICurrencyManager GetCurrencyManager();
	}
	public class CurrencyManagerAdaptor: SlickBowShootingMonoBehaviourAdaptor, ICurrencyManagerAdaptor{
		public override void SetUp(){
			thisManager = CreateCurrencyManager();
		}
		ICurrencyManager thisManager;
		public ICurrencyManager GetCurrencyManager(){
			return thisManager;
		}
		ICurrencyManager CreateCurrencyManager(){
			CurrencyManager.IConstArg arg = new CurrencyManager.ConstArg(this);
			return new CurrencyManager(arg);
		}
		public GainedCurrencyImageAdaptor gainedCurrencyImageAdaptor;

		public override void SetUpReference(){
			IGainedCurrencyImage gainedCurrencyImage = gainedCurrencyImageAdaptor.GetGainedCurrencyImage();
			thisManager.SetGainedCurrencyImage(gainedCurrencyImage);
		}
	}
}
