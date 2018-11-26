using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpAdaptor: IUIAdaptor{
		IPopUp GetPopUp();
		void ToggleRaycastBlock(bool interactable);
	}
	public class PopUpAdaptor : UIAdaptor, IPopUpAdaptor{
		public bool hidesOnTappingOthers;
		public PopUpMode popUpMode;

		public override void SetUp(){
			base.SetUp();
			if(popUpMode == PopUpMode.Alpha)
				SetUpCanvasGroupComponent();
			ToggleRaycastBlock(false);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUpManager manager = CreatePopUpMnager();
			thisPopUp.SetPopUpManager(manager);

			IPopUp parentPopUp = FindProximateParentTypedUIElement<IPopUp>();
			thisPopUp.SetParentPopUp(parentPopUp);

			parentPopUp.AddChildPopUp(thisPopUp);
		}
		public IPopUpManagerAdaptor popUpManagerAdaptor;
		IPopUpManager CreatePopUpMnager(){
			return popUpManagerAdaptor.GetPopUpManager();
		}

		IPopUp thisPopUp;
		public IPopUp GetPopUp(){
			return thisPopUp;
		}
		protected override IUIElement CreateUIElement(){
			PopUp.IConstArg arg = new PopUp.ConstArg(
				this,
				activationMode,
				hidesOnTappingOthers,
				popUpMode
			);
			return new PopUp(arg);
		}
		public void ToggleRaycastBlock(bool blocks){
			CanvasGroup canvasGroup = this.transform.GetComponent<CanvasGroup>();
			canvasGroup.blocksRaycasts = blocks;
		}
	}
}
