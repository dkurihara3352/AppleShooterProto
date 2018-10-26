using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPopUI: ISceneUI{}
	public class PopUI: AbsSceneUI, IPopUI{
		public PopUI(
			AbsSceneUI.IConstArg arg
		): base(arg){
		}
		IPopUIAdaptor thisTypedAdaptor{
			get{
				return (IPopUIAdaptor)thisAdaptor;
			}
		}
		protected override void ActivateImple(){
			thisTypedAdaptor.Pop();
		}
		protected override void DeactivateImple(){
			thisTypedAdaptor.StopMark();
			thisTypedAdaptor.StopGlide();
			thisTypedAdaptor.ResetAtReserve();

		}
	}
}

