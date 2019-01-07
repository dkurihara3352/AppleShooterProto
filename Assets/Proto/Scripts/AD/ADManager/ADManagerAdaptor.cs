using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IADManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IADManager GetADManager();
	}
	public class ADManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IADManagerAdaptor{
		public override void SetUp(){
			thisADManager = CreateADManager();
		}
		IADManager thisADManager;
		IADManager CreateADManager(){
			ADManager.IConstArg arg = new ADManager.ConstArg(
				this
			);
			return new ADManager(arg);
		}
		public IADManager GetADManager(){
			return thisADManager;
		}

		public override void SetUpReference(){
			IADPopUp adPopUp = adPopUpAdaptor.GetADPopUp();
			thisADManager.SetADPopUp(adPopUp);

			IADStatusPopUp adStatusPopUp = adStatusPopUpAdaptor.GetADStatusPopUp();
			thisADManager.SetADStatusPopUp(adStatusPopUp);

			IEndGamePane pane = endGamePaneAdaptor.GetEndGamePane();
			thisADManager.SetEndGamePane(pane);

			IInterstitialADManager intersitialADManager = interstitialADManagerAdaptor.GetInterstitialADManager();
			thisADManager.SetInterstitialADManager(intersitialADManager);
		}
		public ADPopUpAdaptor adPopUpAdaptor;
		public ADStatusPopUpAdaptor adStatusPopUpAdaptor;
		public EndGamePaneAdaptor endGamePaneAdaptor;
		public InterstitialADManagerAdaptor interstitialADManagerAdaptor;
	}
}

