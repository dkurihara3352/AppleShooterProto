﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMarkerUI: ISceneUI{
		void StartMark();
		void StopMark();
	}
	public class MarkerUI : AbsSceneUI, IMarkerUI {

		public MarkerUI(
			IConstArg arg
		): base(arg){}
		IMarkerUIAdaptor thisTypedAdaptor{
			get{
				return (IMarkerUIAdaptor)thisAdaptor;
			}
		}
		public override void Deactivate(){
			base.Deactivate();
			// thisTypedAdaptor.ResetAtReserve();
			thisTypedAdaptor.TriggerDeactivationOnAnimator();
		}
		public override void Activate(){
			base.Activate();
			thisTypedAdaptor.BecomeChildToCanvas();
			StartMark();
			thisTypedAdaptor.TriggerActivationOnAnimator();
		}
		public void StartMark(){
			thisTypedAdaptor.StartMark();
		}
		public void StopMark(){
			thisTypedAdaptor.StopMark();
		}
	}
}
