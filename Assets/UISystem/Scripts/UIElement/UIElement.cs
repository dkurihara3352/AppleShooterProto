using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUIElement: IUISystemSceneObject, IUIInputHandler, ISelectabilityStateHandler, ISelectabilityStateImplementor{

		void SetUIImage(IUIImage image);
		void SetProximateParentScroller(IScroller scroller);

		IUIElement GetParentUIElement();
		// void SetParentUIE(IUIElement uie, bool worldPositionStays);
		IUIElement[] GetChildUIElements();
		void SetLocalPosition(Vector2 localPos);
		IUIAdaptor GetUIAdaptor();
		IUIImage GetUIImage();

		/* Activation */
		void InitiateActivation(bool instantly);

		void ActivateSelf(bool instantly);
		void ActivateRecursively(bool instantly);
		void DeactivateRecursively(bool instantly);
		void ActivateImple();
		void DeactivateSelf(bool instantly);
		void DeactivateImple();
		void OnActivationComplete();
		void OnDeactivationComplete();
		bool IsActivated();
		/*  */
		void EnableInputSelf();
		void EnableInputRecursively();
		void DisableInputSelf();
		void DisableInputRecursively();
		void DisableScrollInputRecursively(IScroller disablingScroller);
		void EnableScrollInputRecursively();
		void EnableScrollInputSelf();
		void CheckForScrollInputEnable();
		IScroller GetTopmostScrollerInMotion();

		/* Scroller */
		void CheckAndPerformStaticBoundarySnapFrom(IUIElement uieToStartCheck);
		IScroller GetProximateParentScroller();
		void EvaluateScrollerFocusRecursively();
		// void EvaluateScrollerFocus();
		void OnScrollerFocus();
		void OnScrollerDefocus();
		void BecomeFocusedInScrollerRecursively();
		void BecomeDefocusedInScrollerRecursively();
		/* PopUp */
		void PopUpDisableRecursivelyDownTo(IPopUp disablingPopUp);
		void ReversePopUpDisableRecursively();
		/* Debug */
		void TurnTo(Color color);
		void Flash(Color color);

		Vector2 GetRectSize();
		void SetRectSize(Vector2 size);
		void ClearInput();
	}
	public class UIElement: UISystemSceneObject, IUIElement{
		public UIElement(
			IConstArg arg
		): base(
			arg
		){

			thisSelectabilityEngine = new SelectabilityStateEngine(
				this
			);

			thisUIEActivationStateEngine = new UIEActivationStateEngine(
				thisUISystemProcessFactory, 
				this,
				arg.activationMode
			);
		}
		protected IUIManager thisUIManager{
			get{
				return thisUISystemMonoBehaviourAdaptorManager.GetUIManager();
			}
		}
		IPopUpManager thisPopUpManager{
			get{
				return thisUIManager.GetPopUpManager();
			}
		}
		protected IUIAdaptor thisUIAdaptor{
			get{
				return (IUIAdaptor)thisAdaptor;
			}
		}
		public IUIAdaptor GetUIAdaptor(){
			return thisUIAdaptor;
		}
		protected IUIElement thisParentUIE{
			get{return thisUIAdaptor.GetParentUIElement();}
		}
		public IUIElement GetParentUIElement(){
			return thisParentUIE;
		}
		protected IUIElement[] thisChildUIEs{
			get{return thisUIAdaptor.GetChildUIElements();}
		}
		public IUIElement[] GetChildUIElements(){
			return thisChildUIEs;
		}
		public void SetRectSize(Vector2 size){
			thisUIAdaptor.SetRectSize(size);
		}
		public Vector2 GetRectSize(){
			return thisUIAdaptor.GetRectSize();
		}
		/* UIImage */
			public IUIImage GetUIImage(){
				return thisImage;
			}
			protected IUIImage thisImage;
			public void SetUIImage(IUIImage image){
				thisImage = image;
			}
		/*  */
		public void SetLocalPosition(Vector2 localPos){
			thisUIAdaptor.SetLocalPosition(localPos);
		}

		/* Activation */
			protected IUIEActivationStateEngine thisUIEActivationStateEngine;
			public void InitiateActivation(bool instantly){
				EvaluateScrollerFocusRecursively();
				ActivateRecursively(instantly);
			}
			public void ActivateSelf(bool instantly){
				thisUIEActivationStateEngine.Activate(instantly);
			}

			public virtual void ActivateRecursively(bool instantly){
				ActivateSelf(instantly);
				ActivateAllChildren(instantly);
			}
			protected void ActivateAllChildren(bool instantly){
				foreach(IUIElement childUIE in this.GetChildUIElements()){
					if(childUIE != null)
						childUIE.ActivateRecursively(instantly); 
				}
			}
			public void ActivateImple(){
				EnableRaycastOnActivation();
				InitializeSelectabilityState();
				OnUIActivate();
			}
			protected virtual void EnableRaycastOnActivation(){
				thisUIAdaptor.ToggleRaycastTarget(true);
			}
			protected virtual void OnUIActivate(){}
			public virtual void DeactivateRecursively(bool instantly){
				DeactivateSelf(instantly);
				DeactivateAllChildren(instantly);
			}
			bool IsActivationComplete(){
				return thisUIEActivationStateEngine.IsActivationComplete();
			}
			public bool IsActivated(){
				return thisUIEActivationStateEngine.IsActivated();
			}
			public void DeactivateSelf(bool instantly){
				thisUIEActivationStateEngine.Deactivate(instantly);
			}
			protected void DeactivateAllChildren(bool instantly){
				foreach(IUIElement childUIE in this.GetChildUIElements()){
					if(childUIE != null)
						childUIE.DeactivateRecursively(instantly);
				}
			}

			public void DeactivateImple(){
				thisUIAdaptor.ToggleRaycastTarget(false);
				this.OnScrollerDefocus();
				OnUIDeactivate();
			}
			protected virtual void OnUIDeactivate(){}
			public virtual void OnActivationComplete(){
			}
			public virtual void OnDeactivationComplete(){
			}
		/* SelectabilityState */
			protected virtual void InitializeSelectabilityState(){
				if(thisIsFocusedInScroller)
					BecomeSelectable();
				else
					BecomeUnselectable();
			}
			ISelectabilityStateEngine thisSelectabilityEngine;
			public void BecomeSelectable(){
				thisSelectabilityEngine.BecomeSelectable();
			}
			public void BecomeUnselectable(){
				thisSelectabilityEngine.BecomeUnselectable();
			}
			public void BecomeSelected(){
				thisSelectabilityEngine.BecomeSelected();
			}
			public bool IsSelectable(){
				return thisSelectabilityEngine.IsSelectable();
			}
			public bool IsSelected(){
				return thisSelectabilityEngine.IsSelected();
			}
			/* imple */
				public virtual void BecomeSelectableImple(){
					if(thisUIManager.ShowsNormal())
						thisImage.TurnToSelectableBrightness();
				}
				public virtual void BecomeUnselectableImple(){
					if(thisUIManager.ShowsNormal())
						thisImage.TurnToUnselectableBrightness();
				}
				public void BecomeSelectedImple(){
					return;
				}



		/* UIInput */
			protected bool thisIsEnabledInput = true;
			/* Touch */
				public void OnTouch(int touchCount){
					if(this.IsActivated()){
						if(thisIsEnabledInput){
							IScroller scrollerToStartPauseMotorProcess = GetTargetUIEOrItsProximateParentAsScroller(this);
							if(scrollerToStartPauseMotorProcess != null)
								scrollerToStartPauseMotorProcess.StopRunningMotorProcessRecursivelyUp();
							OnTouchImple(touchCount);
						}
					}else{
						PassOnTouchUpward(touchCount);
					}
					
				}
				protected virtual void OnTouchImple(int touchCount){
					PassOnTouchUpward(touchCount);
				}
				void PassOnTouchUpward(int touchCount){
					if(thisParentUIE != null){
						thisParentUIE.OnTouch(touchCount);
					}
				}
			/* delayed touch */
				public void OnDelayedTouch(){
					if(this.IsActivated() && thisIsEnabledInput)
						OnDelayedTouchImple();
					else
						PassOnDelayedTouchUpward();
				}
				protected virtual void OnDelayedTouchImple(){
					PassOnDelayedTouchUpward();
				}
				void PassOnDelayedTouchUpward(){
					if(thisParentUIE != null)
						thisParentUIE.OnDelayedTouch();
				}
			/* Release */
				public void OnRelease(){
					if(this.IsActivated() && thisIsEnabledInput){
						CheckAndPerformStaticBoundarySnapFrom(this);
						OnReleaseImple();
					}
					else
						PassOnReleaseUpward();
				}
				protected virtual void OnReleaseImple(){
					PassOnReleaseUpward();
				}
				void PassOnReleaseUpward(){
					if(thisParentUIE != null){
						thisParentUIE.OnRelease();
					}
				}
			/* tap */
				public void OnTap(int tapCount){
					if(this.IsActivated()){
						if(thisIsDisabledForPopUp){
							thisPopUpManager.CheckAndHideActivePopUp();
						}
						else{
							if(thisIsEnabledInput){
								CheckAndPerformStaticBoundarySnapFrom(this);
								OnTapImple(tapCount);
							}else{
								PassOnTapUpward(tapCount);
							}
						}
					}else{
						PassOnTapUpward(tapCount);
					}
				}
				protected virtual void OnTapImple(int tapCount){
					PassOnTapUpward(tapCount);
				}
				void PassOnTapUpward(int tapCount){
					if(thisParentUIE != null){
						thisParentUIE.OnTap(tapCount);
					}
				}
			/* Scroller Helper */
				public void CheckAndPerformStaticBoundarySnapFrom(IUIElement uieToStartCheck){
					ClearTopMostScroller();
					IScroller scrollerToStartCheck = GetTargetUIEOrItsProximateParentAsScroller(uieToStartCheck);
					IScroller scrollerToExamine = scrollerToStartCheck;
					while(true){
						if(scrollerToExamine == null)
							break;
						scrollerToExamine.ResetDrag();
						scrollerToExamine.CheckAndPerformStaticBoundarySnap();
						scrollerToExamine = scrollerToExamine.GetProximateParentScroller();
					}
				}
				IScroller GetTargetUIEOrItsProximateParentAsScroller(IUIElement targetUIElement){
					if(targetUIElement != null){
						if(targetUIElement is IScroller)
							return (IScroller)targetUIElement;
						else
							return targetUIElement.GetProximateParentScroller();
					}else
						return null;
				}
				protected IScroller thisProximateParentScroller;
				public IScroller GetProximateParentScroller(){
					return thisProximateParentScroller;
				}
				public void SetProximateParentScroller(IScroller scroller){
					thisProximateParentScroller = scroller;
				}

				void ClearTopMostScroller(){
					ClearAllParentScrollerVelocity();
					if(thisTopmostScrollerInMotion != null)
						thisTopmostScrollerInMotion.EnableScrollInputRecursively();
				}
				void ClearAllParentScrollerVelocity(){
					IScroller scrollerToExamine = GetTargetUIEOrItsProximateParentAsScroller(this);
					while(true){
						if(scrollerToExamine == null)
							break;
						for(int i = 0; i < 2; i ++){
							scrollerToExamine.UpdateVelocity(0f, i);
						}
						scrollerToExamine = scrollerToExamine.GetProximateParentScroller();
					}
				}
			/* Delayed Release */
				public void OnDelayedRelease(){
					if(this.IsActivated() && thisIsEnabledInput)
						OnDelayedReleaseImple();
					else
						PassOnDelayedReleaseUpward();
				}
				protected virtual void OnDelayedReleaseImple(){
					PassOnDelayedReleaseUpward();
				}
				void PassOnDelayedReleaseUpward(){
					if(thisParentUIE != null)
						thisParentUIE.OnDelayedRelease();
				}
			/* BeginDrag */
				public void OnBeginDrag(ICustomEventData eventData){
					if(this.IsActivated() && thisIsEnabledInput)
						OnBeginDragImple(eventData);
					else
						PassOnBeginDragUpward(eventData);
				}
				protected virtual void OnBeginDragImple(ICustomEventData eventData){
					PassOnBeginDragUpward(eventData);
				}
				void PassOnBeginDragUpward(ICustomEventData eventData){
					if(thisParentUIE != null)
						thisParentUIE.OnBeginDrag(eventData);
				}
			/* Drag */
				public void OnDrag( ICustomEventData eventData){
					if(this.IsActivated() && thisIsEnabledInput)
						OnDragImple(eventData);
					else
						PassOnDragUpward(eventData);
				}
				protected virtual void OnDragImple(ICustomEventData eventData){
					PassOnDragUpward(eventData);
				}
				void PassOnDragUpward(ICustomEventData eventData){
					if(thisParentUIE != null)
						thisParentUIE.OnDrag(eventData);
				}
			/* Hold */
				public void OnHold( float elapsedT){
					if(this.IsActivated() && thisIsEnabledInput)
						OnHoldImple(elapsedT);
					else
						PassOnHoldUpward(elapsedT);
				}
				protected virtual void OnHoldImple(float elapsedT){
					PassOnHoldUpward(elapsedT);
				}
				void PassOnHoldUpward(float elapsedT){
					if(thisParentUIE != null)
						thisParentUIE.OnHold(elapsedT);
				}
			/* Swipe */
				public void OnSwipe( ICustomEventData eventData){
					if(this.IsActivated() && thisIsEnabledInput){
						if(!(this is IScroller))
							CheckAndPerformStaticBoundarySnapFrom(this);
						OnSwipeImple(eventData);
					}
					else
						PassOnSwipeUpward(eventData);
				}
				protected virtual void OnSwipeImple(ICustomEventData eventData){
					PassOnSwipeUpward(eventData);
				}
				void PassOnSwipeUpward(ICustomEventData eventData){
					if(thisParentUIE != null)
						thisParentUIE.OnSwipe(eventData);
				}
			/*  */
		/*  */
		public void EnableInputSelf(){
			thisIsEnabledInput = true;
			if(thisUIManager.ShowsInputability())
				TurnTo(GetUIImage().GetDefaultColor());
		}
		public void EnableInputRecursively(){
			this.EnableInputSelf();
			foreach(IUIElement child in thisChildUIEs)
				child.EnableInputRecursively();
		}
		public void DisableInputSelf(){
			thisIsEnabledInput = false;
			if(thisUIManager.ShowsInputability())
				TurnTo(Color.red);
		}
		public void DisableInputRecursively(){
			this.DisableInputSelf();
			foreach(IUIElement child in thisChildUIEs)
				child.DisableInputRecursively();
		}
		protected IScroller thisTopmostScrollerInMotion;
		public IScroller GetTopmostScrollerInMotion(){
			return thisTopmostScrollerInMotion;
		}
		public virtual void DisableScrollInputRecursively(IScroller disablingScroller){
			this.DisableInputSelf();
			thisTopmostScrollerInMotion = disablingScroller;
			foreach(IUIElement child in thisChildUIEs){
				child.DisableScrollInputRecursively(disablingScroller);
			}
		}
		public virtual void EnableScrollInputRecursively(){
			EnableScrollInputSelf();
			foreach(IUIElement child in thisChildUIEs){
				child.EnableScrollInputRecursively();
			}
		}
		public virtual void EnableScrollInputSelf(){
			this.EnableInputSelf();
			thisTopmostScrollerInMotion = null;
		}
		public virtual void CheckForScrollInputEnable(){
			this.EnableScrollInputSelf();
			foreach(IUIElement child in thisChildUIEs)
				child.CheckForScrollInputEnable();
		}



		/* Scrolller */
			public virtual void EvaluateScrollerFocusRecursively(){
				this.OnScrollerFocus();
				foreach(IUIElement childUIE in GetChildUIElements())
					if(childUIE != null)
						childUIE.EvaluateScrollerFocusRecursively();
			}
			protected bool thisIsFocusedInScroller = false;
			public virtual void OnScrollerFocus(){
				thisIsFocusedInScroller = true;
				BecomeSelectable();
				// if(this.IsActivated()){
				// }
			}
			public virtual void OnScrollerDefocus(){
				thisIsFocusedInScroller = false;
				BecomeUnselectable();
				// if(this.IsActivated()){
				// }
			}
			public virtual void BecomeFocusedInScrollerRecursively(){
				OnScrollerFocus();
				foreach(IUIElement child in GetChildUIElements())
					child.BecomeFocusedInScrollerRecursively();
			}
			public virtual void BecomeDefocusedInScrollerRecursively(){
				OnScrollerDefocus();
				foreach(IUIElement child in GetChildUIElements())
					child.BecomeDefocusedInScrollerRecursively();
			}
		/* PopUp */
		protected bool thisIsDisabledForPopUp = false;
		bool thisWasSelectableAtPopUpDisable;
		bool thisInputWasEnabledAtPopUpDisable;
		void DisableForPopUp(){
			thisIsDisabledForPopUp = true;
			thisWasSelectableAtPopUpDisable = this.IsSelectable();
			thisInputWasEnabledAtPopUpDisable = thisIsEnabledInput;
			BecomeUnselectable();
			DisableInputSelf();
		}
		public void PopUpDisableRecursivelyDownTo(IPopUp disablingPopUp){
			if(this.IsActivated()){
				if(this == disablingPopUp)
					return;
				else{
					DisableForPopUp();
					foreach(IUIElement child in thisChildUIEs)
						if(child != null)
							child.PopUpDisableRecursivelyDownTo(disablingPopUp);
				}
			}
		}
		void ReverseDisableForPopUp(){
			thisIsDisabledForPopUp = false;
			if(thisWasSelectableAtPopUpDisable)
				BecomeSelectable();
			if(thisInputWasEnabledAtPopUpDisable)
				EnableInputSelf();
		}
		public void ReversePopUpDisableRecursively(){
			ReverseDisableForPopUp();
			if(this.IsActivated()){
				foreach(IUIElement child in thisChildUIEs)
					if(child != null)
						child.ReversePopUpDisableRecursively();
			}
		}
		/*  */
		public void TurnTo(Color color){
			GetUIImage().TurnTo(color);
		}
		public void Flash(Color color){
			GetUIImage().Flash(color);
		}
		public void ClearInput(){
			this.OnRelease();
		}
		public new interface IConstArg: UISystemSceneObject.IConstArg{
			ActivationMode activationMode{get;}
		}
		public new class ConstArg: UISystemSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IUIAdaptor adaptor,
				ActivationMode activationMode
			): base(
				adaptor
			){
				thisActivationMode = activationMode;
			}
			readonly ActivationMode thisActivationMode;
			public ActivationMode activationMode{
				get{return thisActivationMode;}
			}
		}
	}
}
