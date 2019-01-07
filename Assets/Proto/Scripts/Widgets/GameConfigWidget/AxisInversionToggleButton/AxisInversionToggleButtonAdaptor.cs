using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IAxisInversionToggleButtonAdaptor: IUIAdaptor{
		IAxisInversionToggleButton GetAxisInversionToggleButton();
		int GetAxis();
		void SetText(string text);
	}
	public class AxisInversionToggleButtonAdaptor: UIAdaptor, IAxisInversionToggleButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			AxisInversionToggleButton.IConstArg arg = new AxisInversionToggleButton.ConstArg(
				this,
				activationMode
			);
			return new AxisInversionToggleButton(arg);
		}
		IAxisInversionToggleButton thisToggleButton{
			get{
				return (IAxisInversionToggleButton)thisUIElement;
			}
		}
		public IAxisInversionToggleButton GetAxisInversionToggleButton(){
			return thisToggleButton;
		}
		public int axis;
		public int GetAxis(){
			return axis;
		}
		public void SetText(string text){
			textComp.text = text;
		}
		public UnityEngine.UI.Text textComp;

		public override void SetUpReference(){
			base.SetUpReference();
			IGameConfigWidget widget = gameConfigWidgetAdaptor.GetGameConfigWidget();
			thisToggleButton.SetGameConfigWidget(widget);
		}
		public GameConfigWidgetAdaptor gameConfigWidgetAdaptor;
	}
}

