using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IAxisInversionToggleButton: IValidatableUIElement{
		void SetGameConfigWidget(IGameConfigWidget widget);
		void SetStatus(bool toggled);
	}
	public class AxisInversionToggleButton: ValidatableUIElement, IAxisInversionToggleButton{
		public AxisInversionToggleButton(IConstArg arg): base(arg){}
		protected override void OnTapImple(int tapCount){
			base.OnTapImple(tapCount);
			thisGameConfigWidget.ToggleAxisInversion(thisAxis);
		}
		IGameConfigWidget thisGameConfigWidget;
		public void SetGameConfigWidget(IGameConfigWidget widget){
			thisGameConfigWidget = widget;
		}
		int thisAxis{
			get{
				return thisAxisInversionToggleButtonAdaptor.GetAxis();
			}
		}
		IAxisInversionToggleButtonAdaptor thisAxisInversionToggleButtonAdaptor{
			get{
				return (IAxisInversionToggleButtonAdaptor)thisUIAdaptor;
			}
		}
		public void SetStatus(bool toggled){
			if(toggled)
				thisAxisInversionToggleButtonAdaptor.SetText("ON");
			else
				thisAxisInversionToggleButtonAdaptor.SetText("OFF");
		}
	}
}

