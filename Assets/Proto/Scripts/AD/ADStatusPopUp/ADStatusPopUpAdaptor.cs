using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IADStatusPopUpAdaptor: IPopUpAdaptor{
		IADStatusPopUp GetADStatusPopUp();
		void SetText(string text);
	}
	public class ADStatusPopUpAdaptor: PopUpAdaptor, IADStatusPopUpAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateADStatusPopUp();
		}
		IADStatusPopUp thisADStatusPopUp{
			get{
				return (IADStatusPopUp)thisUIElement;
			}
		}
		IADStatusPopUp CreateADStatusPopUp(){
			ADStatusPopUp.IConstArg arg = new ADStatusPopUp.ConstArg(
				this,
				activationMode,
				hidesOnTappingOthers,
				PopUpMode.Alpha,
				popUpProcessTime
			);
			return new ADStatusPopUp(arg);
		}
		public IADStatusPopUp GetADStatusPopUp(){
			return thisADStatusPopUp;
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IDoubleEarnedCrystalsADManager doubleEarnedCrystalsADManager = doubleEarnedCrystalsADManagerAdaptor.GetADManager();
			thisADStatusPopUp.SetDoubleEarnedCrystalsADManager(doubleEarnedCrystalsADManager);
		}
		public DoubleEarnedCrystalsADManagerAdaptor doubleEarnedCrystalsADManagerAdaptor;

		public UnityEngine.UI.Text textComp;
		public void SetText(string text){
			textComp.text = text;
		}
	}

}
