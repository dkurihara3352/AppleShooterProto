using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowAttributeLevelUpHoldButton: IHoldButton{
		void SetBowConfigWidget(IBowConfigWidget widget);
		void MaxOut();
		void Validate();
		void SetNextCost(int cost);
		int GetCost();
		void InvalidateForShortMoney();
	}
	public class BowAttributeLevelUpHoldButton: HoldButton, IBowAttributeLevelUpHoldButton{
		public BowAttributeLevelUpHoldButton(IConstArg arg): base(arg){}
		IBowConfigWidget thisWidget;
		IBowAttributeLevelUpHoldButtonAdaptor thisBowAttributeLevelUpHoldButtonAdaptor{
			get{
				return (IBowAttributeLevelUpHoldButtonAdaptor)thisUIAdaptor;
			}
		}
		public void SetBowConfigWidget(IBowConfigWidget widget){
			thisWidget = widget;
		}
		int thisIndex{
			get{
				return thisBowAttributeLevelUpHoldButtonAdaptor.GetAttributeIndex();
			}
		}
		protected override void OnHoldButtonExecute(){
			thisWidget.IncreaseAttributeLevel(thisIndex);
		}

		bool thisIsValid = true;
		public override void ActivateRecursively(bool instantly){
			if(thisIsValid)
				ActivateSelf(instantly);
			ActivateAllChildren(instantly);
		}
		public void MaxOut(){
			thisIsValid = false;
			DeactivateSelf(false);
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Level Max");
		}
		public void Validate(){
			thisIsValid = true;
			ActivateSelf(false);
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Level Up");
		}
		public override void BecomeSelectableImple(){
			if(thisUIManager.ShowsNormal())
				if(thisIsValid)
					thisImage.TurnToOriginalColor();
		}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			BecomeSelectable();
		}
		protected override void OnTouchImple(int touchCount){
			base.OnTouchImple(touchCount);
			BecomeUnselectable();
		}
		protected override void OnReleaseImple(){
			base.OnReleaseImple();
			BecomeSelectable();
		}
		protected override void OnSwipeImple(ICustomEventData eventData){
			base.OnSwipeImple(eventData);
			BecomeSelectable();
		}
		int thisNextCost = 0;
		public int GetCost(){
			return thisNextCost;
		}
		public void SetNextCost(int cost){
			thisNextCost = cost;
			thisBowAttributeLevelUpHoldButtonAdaptor.SetCostText(cost.ToString());
		}
		public void InvalidateForShortMoney(){
			thisIsValid = false;
			DeactivateSelf(false);
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Not Enough");
		}
	}
}

