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
			thisSceneUI = arg.sceneUI;
		}
		ISceneUI thisSceneUI;
		protected override void RunImple(){
		}
		protected override void UpdateProcessImple(float deltaT){
			thisSceneUI.UpdateUI();
		}
		protected override void StopImple(){
		}
		/* constArg */
			public interface IConstArg: IProcessConstArg{
				ISceneUI sceneUI{get;}
			}
			public class ConstArg: ProcessConstArg, IConstArg{
				public ConstArg(
					ISceneUI sceneUI,
					IProcessManager processManager
				): base(
					processManager
				){
					thisSceneUI  = sceneUI ;
				}
				readonly ISceneUI thisSceneUI;
				public ISceneUI sceneUI{
					get{
						return thisSceneUI;
					}
				}
			}
		/*  */
	}
}


