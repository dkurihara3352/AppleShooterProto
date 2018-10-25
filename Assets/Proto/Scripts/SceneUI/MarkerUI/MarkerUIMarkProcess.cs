using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IMarkerUIMarkProcess: IProcess{}
	public class MarkerUIMarkProcess: AbsProcess, IMarkerUIMarkProcess{
		public MarkerUIMarkProcess(
			IConstArg arg
		): base(arg){
			thisMarkerUI = arg.markerUI;
		}
		IMarkerUI thisMarkerUI;
		protected override void RunImple(){
		}
		protected override void UpdateProcessImple(float deltaT){
			thisMarkerUI.UpdateUI();
		}
		protected override void StopImple(){
		}
		/* constArg */
			public interface IConstArg: IProcessConstArg{
				IMarkerUI markerUI{get;}
			}
			public class ConstArg: ProcessConstArg, IConstArg{
				public ConstArg(
					IMarkerUI markerUI,
					IProcessManager processManager
				): base(
					processManager
				){
					thisMarkerUI  = markerUI ;
				}
				readonly IMarkerUI thisMarkerUI;
				public IMarkerUI markerUI{
					get{
						return thisMarkerUI;
					}
				}

			}
		/*  */
	}
}


