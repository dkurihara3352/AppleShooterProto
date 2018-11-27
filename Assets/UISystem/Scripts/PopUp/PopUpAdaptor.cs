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
			if(popUpMode == PopUpMode.Alpha)
				SetUpCanvasGroupComponent();
			ToggleRaycastBlock(false);
			base.SetUp();
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUpManager manager = CreatePopUpMnager();
			thisPopUp.SetPopUpManager(manager);

			IPopUp parentPopUp = FindProximateParentTypedUIElement<IPopUp>();
			thisPopUp.SetParentPopUp(parentPopUp);
			if(parentPopUp != null)
				parentPopUp.AddChildPopUp(thisPopUp);
			
		}
		public override void FinalizeSetUp(){
			base.FinalizeSetUp();
			// thisPopUp.LogHierarchy();
		}
		public PopUpManagerAdaptor popUpManagerAdaptor;
		IPopUpManager CreatePopUpMnager(){
			return popUpManagerAdaptor.GetPopUpManager();
		}

		IPopUp thisPopUp{
			get{
				return (IPopUp)thisUIElement;
			}
		}
		public IPopUp GetPopUp(){
			return thisPopUp;
		}
		public float processTime = .1f;
		protected override IUIElement CreateUIElement(){
			PopUp.IConstArg arg = new PopUp.ConstArg(
				this,
				activationMode,

				hidesOnTappingOthers,
				popUpMode,
				processTime
			);
			return new PopUp(arg);
		}
		public void ToggleRaycastBlock(bool blocks){
			thisCanvasGroup.blocksRaycasts = blocks;
		}
	}
}
