using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IFadeImage: IAppleShooterSceneObject{
		void SetFadeness(float fadeness);//0 => total black, 1=> alpha 0
	}
	public class FadeImage: AppleShooterSceneObject, IFadeImage{
		public FadeImage(IConstArg arg): base(arg){
			SetFadeness(0f);
		}
		IFadeImageAdaptor thisFadeImageAdaptor{
			get{
				return (IFadeImageAdaptor)thisAdaptor;
			}
		}
		public void SetFadeness(float fadeness){
			thisFadeImageAdaptor.SetAlpha(1f - fadeness);
		}
	}
}

