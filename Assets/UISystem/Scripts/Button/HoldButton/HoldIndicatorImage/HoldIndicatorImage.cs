using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IHoldIndicatorImage: IUIElement{
		void SetHoldValue(float value);
	}
	//make sure parent is Holdbutton
	public class HoldIndicatorImage: UIElement, IHoldIndicatorImage{
		public HoldIndicatorImage(IConstArg arg): base(arg){}
		IHoldIndicatorImageAdaptor thisHoldIndicatorImageAdaptor{
			get{
				return (IHoldIndicatorImageAdaptor)thisUIAdaptor;
			}
		}
		public void SetHoldValue(float value){
			thisHoldIndicatorImageAdaptor.SetFill(1f - value);
		}
	}
}

