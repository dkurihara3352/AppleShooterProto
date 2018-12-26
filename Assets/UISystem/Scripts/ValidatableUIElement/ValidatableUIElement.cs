using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IValidatableUIElement: IUIElement{
		void Validate();
		void Invalidate();
	}
	public abstract class ValidatableUIElement: UIElement, IValidatableUIElement{
		public ValidatableUIElement(IConstArg arg): base(arg){}

		protected bool thisIsValid = true;
		public override void ActivateRecursively(bool instantly){
			if(thisIsValid)
				ActivateSelf(instantly);
			ActivateAllChildren(instantly);
		}
		public void Validate(){
			// Debug.Log(DKUtility.DebugHelper.StringInColor("validated", Color.blue));
			thisIsValid = true;
			ActivateSelf(false);
			if(thisIsFocusedInScroller){
				if(this.IsSelectable())
					BecomeSelectableImple();
				else
					BecomeSelectable();
			}
		}
		public void Invalidate(){
			// Debug.Log(DKUtility.DebugHelper.StringInColor("invalidated", Color.red));
			thisIsValid = false;
			DeactivateSelf(false);
		}
		public override void BecomeSelectableImple(){
			if(thisUIManager.ShowsNormal())
				if(thisIsValid)
					thisImage.TurnToOriginalColor();
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			BecomeSelectable();
			MakeAllChildUIESelectable();
		}
		protected override void OnTouchImple(int touchCount){
			base.OnTouchImple(touchCount);
			BecomeUnselectable();
			MakeAllChildUIEUnselectable();
		}
		protected override void OnReleaseImple(){
			base.OnReleaseImple();
			BecomeSelectable();
			MakeAllChildUIESelectable();
		}
		protected override void OnSwipeImple(ICustomEventData eventData){
			base.OnSwipeImple(eventData);
			BecomeSelectable();
			MakeAllChildUIESelectable();
		}
		void MakeAllChildUIESelectable(){
			foreach(IUIElement childUIE in this.GetChildUIElements())
				childUIE.BecomeSelectable();
		}
		void MakeAllChildUIEUnselectable(){
			foreach(IUIElement childUIE in this.GetChildUIElements())
				childUIE.BecomeUnselectable();
		}
	}
}

