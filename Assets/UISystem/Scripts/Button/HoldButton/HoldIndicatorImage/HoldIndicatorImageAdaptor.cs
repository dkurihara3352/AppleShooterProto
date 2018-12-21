using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem{
	public interface IHoldIndicatorImageAdaptor: IUIAdaptor{
		IHoldIndicatorImage GetHoldIndicatorImage();
		void SetFill(float value);
	}
	public class HoldIndicatorImageAdaptor: UIAdaptor, IHoldIndicatorImageAdaptor{
		public override void SetUp(){
			base.SetUp();
		}
		protected override IUIElement CreateUIElement(){
			HoldIndicatorImage.IConstArg arg = new HoldIndicatorImage.ConstArg(
				this,
				activationMode
			);
			return new HoldIndicatorImage(arg);
		}
		IHoldIndicatorImage thisHoldIndicatorImage{
			get{
				return (IHoldIndicatorImage)thisUIElement;
			}
		}
		public IHoldIndicatorImage GetHoldIndicatorImage(){
			return thisHoldIndicatorImage;
		}
		public Image fillImage;
		public void SetFill(float value){
			fillImage.fillAmount = value;
		}
	}
}

