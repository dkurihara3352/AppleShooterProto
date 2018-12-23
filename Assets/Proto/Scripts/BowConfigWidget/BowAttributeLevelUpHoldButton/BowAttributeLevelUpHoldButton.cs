using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowAttributeLevelUpHoldButton: IHoldButton{
		void SetBowConfigWidget(IBowConfigWidget widget);
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
	}
}

