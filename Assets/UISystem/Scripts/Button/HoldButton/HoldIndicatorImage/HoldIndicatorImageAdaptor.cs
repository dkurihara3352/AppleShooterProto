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
		//make sure parent is HoldButtonAdaptor or activation block is not guaranteed
		public override void SetUp(){
			base.SetUp();
			MakeSureParentIsHoldButton();
		}
		void MakeSureParentIsHoldButton(){
			Transform parent = transform.parent;
			Component[] parComps = parent.GetComponents<Component>();
			bool found = false;
			foreach(Component comp in parComps){
				if(comp is IHoldButtonAdaptor)
					found = true;
			}
			if(!found)
				throw new System.InvalidOperationException(
					"parent does not have IHoldButtonAdaptor component"
				);
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

