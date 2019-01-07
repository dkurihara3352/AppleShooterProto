using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IHeadUpDisplayAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		void SetAlpha(float alpha);
		float GetAlpha();
		float GetActivationTime();
		float GetDeactivationTime();

		IHeadUpDisplay GetHeadUpDisplay();
	}
	[RequireComponent(typeof(CanvasGroup))]
	public class HeadUpDisplayAdaptor : SlickBowShootingMonoBehaviourAdaptor, IHeadUpDisplayAdaptor {

		public override void SetUp(){
			thisHeadUpDisplay = CreateHeadUpDisplay();
			thisCanvasGroup = GetComponent<CanvasGroup>();
			SetAlpha(0f);
		}
		public override void FinalizeSetUp(){
			thisHeadUpDisplay.Deactivate();
		}
		IHeadUpDisplay thisHeadUpDisplay;
		IHeadUpDisplay CreateHeadUpDisplay(){
			HeadUpDisplay.IConstArg arg = new HeadUpDisplay.ConstArg(
				this
			);
			return new HeadUpDisplay(arg);
		}
		public IHeadUpDisplay GetHeadUpDisplay(){
			return thisHeadUpDisplay;
		}
		public float activationTime = .2f;
		public float GetActivationTime(){
			return activationTime;
		}
		public float deactivationTime = .2f;
		public float GetDeactivationTime(){
			return deactivationTime;
		}
		public void SetAlpha(float alpha){
			thisCanvasGroup.alpha = alpha;
			if(alpha != 1f)
				DisableHeatDelaImage();
			else
				EnableHeatDeltaImage();
		}
		public float GetAlpha(){
			return thisCanvasGroup.alpha;
		}
		CanvasGroup thisCanvasGroup;
		public UnityEngine.UI.Image heatDeltaImage;
		void DisableHeatDelaImage(){
			heatDeltaImage.enabled = false;
		}
		void EnableHeatDeltaImage(){
			heatDeltaImage.enabled = true;
		}
		

	}
}
