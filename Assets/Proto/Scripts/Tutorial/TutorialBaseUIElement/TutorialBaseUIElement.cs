using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ITutorialBaseUIElement: IUIElement{
		void ActivateThruBackdoor(bool instantly);
		void SetTutorialTapListener(ITutorialBaseTapListener listener);
		void CheckAndClearTapListener(ITutorialBaseTapListener listener);
	}
	public class TutorialBaseUIElement: UIElement, ITutorialBaseUIElement{
		public TutorialBaseUIElement(IConstArg arg): base(arg){

		}
		public override void ActivateRecursively(bool instantly){
			return;
		}
		public void ActivateThruBackdoor(bool instantly){
			ActivateSelf(instantly);
			ActivateAllChildren(instantly);
			EvaluateScrollerFocusRecursively();
		}
		protected override void OnTapImple(int tapCount){
			if(thisActiveListener != null){
				thisActiveListener.OnTutorialBaseTap();
			}
		}
		public void SetTutorialTapListener(ITutorialBaseTapListener listener){
			thisActiveListener = listener;
		}
		ITutorialBaseTapListener thisActiveListener;
		public void CheckAndClearTapListener(ITutorialBaseTapListener listener){
			if(thisActiveListener != null)
				if(thisActiveListener == listener)
					thisActiveListener = null;
		}
	}
	public interface ITutorialBaseTapListener{
		void OnTutorialBaseTap();
	}
}


