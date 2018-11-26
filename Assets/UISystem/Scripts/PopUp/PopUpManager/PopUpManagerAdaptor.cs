using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPopUpManagerAdaptor: IUISystemMonoBehaviourAdaptor{
		IPopUpManager GetPopUpManager();
	}
	public class PopUpManagerAdaptor : UISystemMonoBehaviourAdaptor, IPopUpManagerAdaptor {
		public override void SetUp(){
			thisManager = CreatePopUpManager();
		}
		IPopUpManager thisManager;
		public IPopUpManager GetPopUpManager(){
			return thisManager;
		}
		IPopUpManager CreatePopUpManager(){
			PopUpManager.IConstArg arg = new PopUpManager.ConstArg(
				this
			);
			return new PopUpManager(arg);
		}
		public UIAdaptor rootUIAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			IUIElement rootUIElement = rootUIAdaptor.GetUIElement();
			thisManager.SetRootUIElement(rootUIElement);
		}
	}
}
