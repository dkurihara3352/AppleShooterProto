using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IHoldButtonAdaptor: IUIAdaptor{
		IHoldButton GetHoldButton();
		float GetHoldTime();
		float GetIndicatorActivationDelayTime();
	}
	public abstract class HoldButtonAdaptor: UIAdaptor, IHoldButtonAdaptor{
		public IHoldButton GetHoldButton(){
			return thisHoldButton;
		}
		IHoldButton thisHoldButton{
			get{
				return (IHoldButton)thisUIElement;
			}
		}
		public HoldIndicatorImageAdaptor holdIndicatorImageAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			IHoldIndicatorImage holdIndicatorImage = holdIndicatorImageAdaptor.GetHoldIndicatorImage();
			thisHoldButton.SetHoldIndicatorImage(holdIndicatorImage);
		}
		public float holdTime = 2f;
		public float GetHoldTime(){
			return holdTime;
		}
		public float indicatorActivatioinDelayTime = .1f;
		public float GetIndicatorActivationDelayTime(){
			return indicatorActivatioinDelayTime;
		}
	}
}
