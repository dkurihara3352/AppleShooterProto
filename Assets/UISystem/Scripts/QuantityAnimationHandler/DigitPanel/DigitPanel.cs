using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IDigitPanel: IUIElement{
		void SetNumber(int number);
		/*  if number = -1, substitute the panel image with Blank image
		*/
	}
	public class DigitPanel: UIElement, IDigitPanel{
		public DigitPanel(
			IConstArg arg
		): base(arg){
			CalcAndSetRectDimension(arg.panelDim, arg.localPosY);
		}
		public void SetNumber(int number){
			((IDigitPanelAdaptor)GetUIAdaptor()).SetImageNumber(number);
		}
		void CalcAndSetRectDimension(Vector2 panelDim, float localPosY){
			IResizableRectUIAdaptor thisAdaptor = (IResizableRectUIAdaptor)this.GetUIAdaptor();
			thisAdaptor.SetRectDimension(panelDim.y, panelDim.x, 0f, localPosY);
		}
		public new interface IConstArg: UIElement.IConstArg{
			Vector2 panelDim{get;}
			float localPosY{get;}
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IDigitPanelAdaptor adaptor, 

				Vector2 panelDim, 
				float localPosY
			): base(
				adaptor, 
				ActivationMode.None
			){
				thisPanelDim = panelDim;
				thisLocalPosY = localPosY;
			}
			Vector2 thisPanelDim;
			public Vector2 panelDim{get{return thisPanelDim;}}
			float thisLocalPosY;
			public float localPosY{get{return thisLocalPosY;}}
		}
	}
}
