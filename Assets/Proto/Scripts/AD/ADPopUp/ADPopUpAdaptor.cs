using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IADPopUpAdaptor: IPopUpAdaptor{
		IADPopUp GetADPopUp();
		void SetIndicatorAlpha(float alpha);
		void SetText(string text);
		void RotateIndicatorImageOnZAxis(float deltaTime);
	}
	public class ADPopUpAdaptor: PopUpAdaptor, IADPopUpAdaptor{
		public IADPopUp GetADPopUp(){
			return thisADPopUp;
		}
		IADPopUp thisADPopUp{
			get{
				return (IADPopUp)thisUIElement;
			}
		}
		protected override IUIElement CreateUIElement(){
			return CreateADPopUp();
		}
		IADPopUp CreateADPopUp(){
			ADPopUp.IConstArg arg = new ADPopUp.ConstArg(
				this,
				activationMode,
				false,
				PopUpMode.Alpha,
				popUpProcessTime
			);
			return new ADPopUp(arg);
		}

		public void SetIndicatorAlpha(float alpha){
			// Color newColor = indicatorImage.color;
			// newColor.a = alpha;
			// indicatorImage.color = newColor;
			imageCanvasGroup.alpha = alpha;
		}
		public CanvasGroup imageCanvasGroup;
		public UnityEngine.UI.Image indicatorImage;
		public void SetText(string text){
			textComp.text = text;
		}
		public UnityEngine.UI.Text textComp;
		public float anglePerSec = 360f;
		public void RotateIndicatorImageOnZAxis(float deltaTime){
			float angle = deltaTime * anglePerSec;
			indicatorImage.transform.Rotate(Vector3.forward, angle, Space.Self);
		}

	}
}
