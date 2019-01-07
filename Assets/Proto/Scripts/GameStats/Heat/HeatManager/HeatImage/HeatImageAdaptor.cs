using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlickBowShooting{
	public interface IHeatImageAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IHeatImage GetHeatImage();

		void UpdateMainHeatImageFill(float heat);
		void UpdateDeltaHeatImageFill(float heat);
	}
	public class HeatImageAdaptor : SlickBowShootingMonoBehaviourAdaptor, IHeatImageAdaptor {
		public Image thisMainImage;
		public Image thisDeltaImage;
		IHeatImage thisHeatImage;
		public override void SetUp(){
			thisHeatImage = CreateHeatImage();
			thisMainImage.type = Image.Type.Filled;
			thisDeltaImage.type = Image.Type.Filled;
		}
		public IHeatImage GetHeatImage(){
			return thisHeatImage;
		}
		IHeatImage CreateHeatImage(){
			HeatImage.IConstArg arg = new HeatImage.ConstArg(
				this
			);
			return new HeatImage(arg);
		}
		public void UpdateMainHeatImageFill(float fill){
			thisMainImage.fillAmount = fill;
		}
		public void UpdateDeltaHeatImageFill(float fill){
			thisDeltaImage.fillAmount = fill;
		}
	}
}
