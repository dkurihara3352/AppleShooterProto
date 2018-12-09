using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IUIManagerAdaptor: IUISystemMonoBehaviourAdaptor{
		IUIManager GetUIManager();
		Canvas GetCanvas();
	}
	public class UIManagerAdaptor: UISystemMonoBehaviourAdaptor, IUIManagerAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisUIManager = CreateUIManager();
		}
		IUIManager thisUIManager;
		public IUIManager GetUIManager(){
			return thisUIManager;
		}

		public bool showsInputability;
		public float swipeVelocityThreshold = 400f;
		public float swipeDistanceThreshold = 20f;
		public float maxSwipeVelovity = 1000f;
		public Canvas canvas;
		public Canvas GetCanvas(){
			return canvas;
		}
		

		IUIManager CreateUIManager(){
			UIManager.IConstArg arg = new UIManager.ConstArg(
				this,
				showsInputability,
				swipeVelocityThreshold,
				swipeDistanceThreshold,
				maxSwipeVelovity
			);
			return new UIManager(arg);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IPopUpManager popUpManager = CollectPopUpManager();
			thisUIManager.SetPopUpManager(popUpManager);
		}
		public PopUpManagerAdaptor popUpManagerAdaptor;
		IPopUpManager CollectPopUpManager(){
			
			return popUpManagerAdaptor.GetPopUpManager();
		}
	}	
}
