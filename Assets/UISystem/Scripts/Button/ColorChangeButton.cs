using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
using UnityEngine.UI;
namespace UISystem{
	public interface IColorChangeButton: IUIElement{
		void SetTargetUIElement(IUIElement targetUIElement);
	}
	public class ColorChangeButton : UIElement, IColorChangeButton {

		public ColorChangeButton(
			IConstArg arg
		): base(arg){
			thisTargetColor = arg.targetColor;
			thisTargetText = arg.targetText;
		}
		IUIElement thisTargetUIElement;
		public void SetTargetUIElement(IUIElement targetUIElement){
			thisTargetUIElement = targetUIElement;
		}
		readonly Text thisTargetText;
		
		Color thisTargetUIEDefaultColor{
			get{
				IUIImage uiImage = thisTargetUIElement.GetUIImage();
				return uiImage.GetDefaultColor();
			}
		}
		readonly Color thisTargetColor;
		bool thisIsChangingToTarget = true;
		protected override void OnTapImple(int tapCount){
			thisIsChangingToTarget = !thisIsChangingToTarget;
			this.Flash(thisTargetColor);
			if(thisIsChangingToTarget)
				thisTargetUIElement.TurnTo(thisTargetUIEDefaultColor);
			else
				thisTargetUIElement.TurnTo(thisTargetColor);
		}
		int count = 0;
		protected override void OnUIActivate(){
			UpdateText();
		}
		void UpdateText(){
			thisTargetText.text = count.ToString();
		}
		protected override void OnTouchImple(int touchCount){
			count ++;
			UpdateText();
		}
		protected override void OnDelayedReleaseImple(){
			count = 0;
			UpdateText();
		}
		public new interface IConstArg: UIElement.IConstArg{
			Color targetColor{get;}
			Text targetText{get;}
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IColorChangeButtonAdaptor adaptor,

				Color targetColor,
				Text targetText
			): base(
				adaptor,
				ActivationMode.None
			){
				thisTargetColor = targetColor;
				thisText = targetText;
			}
			readonly Color thisTargetColor;
			public Color targetColor{get{return thisTargetColor;}}
			readonly Text thisText;
			public Text targetText{get{return thisText;}}
		}
	}
}

