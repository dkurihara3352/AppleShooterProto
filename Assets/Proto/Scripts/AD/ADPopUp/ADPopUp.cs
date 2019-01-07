using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using DKUtility;

namespace AppleShooterProto{
	public interface IADPopUp: IPopUp, IProcessHandler{
		void StartIndicateGetADReady();
		void StopIndicateGetADReady();
	}
	public class ADPopUp: PopUp, IADPopUp{
		public ADPopUp(IConstArg arg): base(arg){
			thisIndicateGetADReadyProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.None,
				0f
			);
		}
		IADPopUpAdaptor thisADPopUpAdaptor{
			get{
				return (IADPopUpAdaptor)thisUIAdaptor;
			}
		}
		IProcessSuite thisIndicateGetADReadyProcessSuite;
		public void StartIndicateGetADReady(){
			thisIndicateGetADReadyProcessSuite.Start();
		}
		public void StopIndicateGetADReady(){
			thisIndicateGetADReadyProcessSuite.Expire();
		}
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisIndicateGetADReadyProcessSuite){
				thisADPopUpAdaptor.SetIndicatorAlpha(1f);
				thisADPopUpAdaptor.SetText("AD Loading");
			}
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisIndicateGetADReadyProcessSuite){
				thisADPopUpAdaptor.RotateIndicatorImageOnZAxis(deltaTime);
			}
		}
		public void OnProcessExpire(IProcessSuite suite){
			if(suite == thisIndicateGetADReadyProcessSuite){
				thisADPopUpAdaptor.SetIndicatorAlpha(0f);
				thisADPopUpAdaptor.SetText("");
			}
		}
	}
}

