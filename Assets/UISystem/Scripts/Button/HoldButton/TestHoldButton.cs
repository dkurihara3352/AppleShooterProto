using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ITestHoldButton: IHoldButton{}
	public class TestHoldButton: HoldButton, ITestHoldButton{
		public TestHoldButton(IConstArg arg): base(arg){}
		protected override void OnHoldButtonExecute(){
			Debug.Log("Hold button fired");
		}
	}
}

