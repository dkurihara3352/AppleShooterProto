using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IADStatusPopUp: IPopUp{
		void SetText(string text);
		void SetDoubleEarnedCrystalsADManager(IADManager adManager);
	}
	public class ADStatusPopUp: PopUp, IADStatusPopUp{
		public ADStatusPopUp(IConstArg arg): base(arg){

		}
		IADStatusPopUpAdaptor thisADStatusPopUpAdaptor{
			get{
				return (IADStatusPopUpAdaptor)thisUIAdaptor;
			}
		}
		public void SetText(string text){
			thisADStatusPopUpAdaptor.SetText(text);
		}
		protected override void OnTapImple(int tapCount){
			this.Hide(false);
		}
		public override void OnHideBegin(){
			thisDoubleEarnedCrystalsADManager.EndADSequence();
			Debug.Log("hidden");
		}
		IADManager thisDoubleEarnedCrystalsADManager;
		public void SetDoubleEarnedCrystalsADManager(IADManager adManager){
			thisDoubleEarnedCrystalsADManager = adManager;
		}
	}
}
