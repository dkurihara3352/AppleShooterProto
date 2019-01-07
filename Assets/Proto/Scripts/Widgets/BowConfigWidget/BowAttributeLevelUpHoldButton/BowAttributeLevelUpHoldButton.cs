using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IBowAttributeLevelUpHoldButton: IHoldButton{
		void SetBowConfigWidget(IBowConfigWidget widget);
		void MaxOut();
		void ValidateForLevelUp();
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

		public void MaxOut(){
			Invalidate();
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Level Max");
		}
		public void ValidateForLevelUp(){
			Validate();
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Level Up");
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
			Invalidate();
			thisBowAttributeLevelUpHoldButtonAdaptor.SetLabelText("Not Enough");
		}
	}
}

