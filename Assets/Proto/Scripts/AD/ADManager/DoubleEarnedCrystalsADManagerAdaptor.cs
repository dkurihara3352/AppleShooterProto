using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IDoubleEarnedCrystalsADManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IDoubleEarnedCrystalsADManager GetADManager();
	}
	public class DoubleEarnedCrystalsADManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IDoubleEarnedCrystalsADManagerAdaptor{
		public override void SetUp(){
			thisADManager = CreateADManager();
		}
		IDoubleEarnedCrystalsADManager thisADManager;
		IDoubleEarnedCrystalsADManager CreateADManager(){
			DoubleEarnedCrystalsADManager.IConstArg arg = new DoubleEarnedCrystalsADManager.ConstArg(
				this
			);
			return new DoubleEarnedCrystalsADManager(arg);
		}
		public IDoubleEarnedCrystalsADManager GetADManager(){
			return thisADManager;
		}

		public override void SetUpReference(){
			IADPopUp adPopUp = adPopUpAdaptor.GetADPopUp();
			thisADManager.SetADPopUp(adPopUp);

			IADStatusPopUp adStatusPopUp = adStatusPopUpAdaptor.GetADStatusPopUp();
			thisADManager.SetADStatusPopUp(adStatusPopUp);

			IEndGamePane pane = endGamePaneAdaptor.GetEndGamePane();
			thisADManager.SetEndGamePane(pane);
		}
		public ADPopUpAdaptor adPopUpAdaptor;
		public ADStatusPopUpAdaptor adStatusPopUpAdaptor;
		public EndGamePaneAdaptor endGamePaneAdaptor;
	}
}

