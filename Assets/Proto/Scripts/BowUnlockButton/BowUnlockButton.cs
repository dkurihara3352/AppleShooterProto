using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowUnlockButton: IHoldButton{
		void SetBowConfigWidget(IBowConfigWidget widget);
		int GetPanelIndex();
		void ValidateForUnlock();
		void InvalidateForShortMoney();
		void SetCostText(int cost);
	}
	public class BowUnlockButton: HoldButton, IBowUnlockButton{
		public BowUnlockButton(IConstArg arg): base(arg){}
		IBowUnlockButtonAdaptor thisBowUnlockButtonAdaptor{
			get{
				return (IBowUnlockButtonAdaptor)thisUIAdaptor;
			}
		}
		IBowConfigWidget thisWidget;
		public void SetBowConfigWidget(IBowConfigWidget widget){
			thisWidget = widget;
		}
		int thisIndex{
			get{
				return thisBowUnlockButtonAdaptor.GetPanelIndex();
			}
		}
		public int GetPanelIndex(){
			return thisIndex;
		}
		protected override void OnHoldButtonExecute(){
			thisWidget.UnlockPanel(thisIndex);
		}
		public void ValidateForUnlock(){
			Validate();
			thisBowUnlockButtonAdaptor.SetLabelText("Unlock");
		}
		public void InvalidateForShortMoney(){
			Invalidate();
			thisBowUnlockButtonAdaptor.SetLabelText("Not enough");
		}
		public void SetCostText(int cost){
			thisBowUnlockButtonAdaptor.SetCostText(cost.ToString());
		}
	}
}

