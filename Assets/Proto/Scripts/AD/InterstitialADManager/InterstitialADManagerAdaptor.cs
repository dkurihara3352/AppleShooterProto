using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IInterstitialADManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IInterstitialADManager GetInterstitialADManager();
	}
	public class InterstitialADManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IInterstitialADManagerAdaptor{
		public override void SetUp(){
			thisInterstitialADManager = CreateInterstitialADManager();
		}
		IInterstitialADManager thisInterstitialADManager;
		IInterstitialADManager CreateInterstitialADManager(){
			InterstitialADManager.IConstArg arg = new InterstitialADManager.ConstArg(
				this
			);
			return new InterstitialADManager(arg);
		}
		public IInterstitialADManager GetInterstitialADManager(){
			return thisInterstitialADManager;
		}

		public override void SetUpReference(){
			IADManager adManager = adManagerAdaptor.GetADManager();
			thisInterstitialADManager.SetADManager(adManager);
		}
		public ADManagerAdaptor adManagerAdaptor;
		
	}
}
