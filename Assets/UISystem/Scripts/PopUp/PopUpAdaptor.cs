using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem{
	public interface IPopUpAdaptor: IUIAdaptor{
		IPopUp GetPopUp();

		float GetPopValue();
		void SetPopValue(float value);
	}
	public class PopUpAdaptor : UIAdaptor, IPopUpAdaptor{
		public bool hidesOnTappingOthers;
		public PopUpMode popUpMode;

		public override void SetUp(){
			SetUpCanvasGroupComponent();
			
			ToggleRaycastTarget(false);
			base.SetUp();

			SetUpBlurMatarial();
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
		public float popUpProcessTime = .1f;
		protected override IUIElement CreateUIElement(){
			PopUp.IConstArg arg = new PopUp.ConstArg(
				this,
				activationMode,

				hidesOnTappingOthers,
				popUpMode,
				popUpProcessTime
			);
			return new PopUp(arg);
		}
		public override void ToggleRaycastTarget(bool blocks){
			if(thisCanvasGroup == null)
				thisCanvasGroup = CollectCanvasGroup();
			thisCanvasGroup.blocksRaycasts = blocks;
		}
		CanvasGroup CollectCanvasGroup(){
			return GetComponent<CanvasGroup>();
		}
		public float GetPopValue(){
			switch(popUpMode){
				case PopUpMode.Alpha:
					return GetGroupAlpha();
				case PopUpMode.Blur:
					return GetBlur();
				default:
					return -1f;
			}
		}
		public void SetPopValue(float value){
			switch(popUpMode){
				case PopUpMode.Alpha:
					SetGroupAlpha(value);
					break;
				case PopUpMode.Blur:
					SetGroupAlpha(value);
					SetBlur(value);
					break;
				default: 
					break;
			}
		}
		Material thisBlurMaterial;
		int thisBlurRadiusID;
		float thisMaxRadius = 10f;
		int thisColorID;
		Color thisOrigColor;
		float thisOrigColorAlpha = .1f;
		void SetUpBlurMatarial(){
			Image image = CollectImage();
			thisBlurMaterial = image.material;
			thisBlurRadiusID = Shader.PropertyToID("_Radius");
			thisColorID = Shader.PropertyToID("_Color");
			thisOrigColor = thisBlurMaterial.GetColor(thisColorID);
		}
		Image CollectImage(){
			List<Image> resultList = new List<Image>();
			int childCount = transform.childCount;
			for(int i = 0; i < childCount; i ++){
				Transform child = transform.GetChild(i);
				Image[] images = child.GetComponents<Image>();
				resultList.AddRange(images);
			}
			if(resultList.Count != 1)
				throw new System.InvalidOperationException(
					"there's more than one Image Component among children"
				);
			return resultList[0];
		}
		float GetBlur(){
			float radiusValue = thisBlurMaterial.GetFloat(thisBlurRadiusID);
			if(radiusValue == 0f)
				return 0f;
			return radiusValue/ thisMaxRadius;
		}
		void SetBlur(float value){
			float scaledValue = value * thisMaxRadius;
			thisBlurMaterial.SetFloat(thisBlurRadiusID, scaledValue);

			float newColorAlpha = Mathf.Lerp(
				0f,
				thisOrigColorAlpha,
				value
			);
			Color newColor = thisOrigColor;
			newColor.a = newColorAlpha;
			thisBlurMaterial.SetColor(thisColorID, newColor);

		}
	}
}
