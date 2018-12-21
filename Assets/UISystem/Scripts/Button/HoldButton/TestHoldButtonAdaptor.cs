using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ITestHoldButtonAdaptor: IHoldButtonAdaptor{}
	public class TestHoldButtonAdaptor: HoldButtonAdaptor{
		protected override IUIElement CreateUIElement(){
			TestHoldButton.IConstArg arg = new TestHoldButton.ConstArg(
				this,
				activationMode
			);
			return new TestHoldButton(arg);
		}
	}
}
