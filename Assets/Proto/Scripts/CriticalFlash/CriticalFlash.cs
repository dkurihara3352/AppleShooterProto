using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ICriticalFlash: IAppleShooterSceneObject{
		void Flash();

		void SetFlashValue(float normalizedFlashValue);
	}
	public class CriticalFlash : AppleShooterSceneObject, ICriticalFlash {
		public CriticalFlash(IConstArg arg): base(arg){}
		ICriticalFlashAdaptor thisTypedAdaptor{
			get{
				return (ICriticalFlashAdaptor)thisAdaptor;
			}
		}
		public void Flash(){
			StopFlashProcess();
			StartFlashProcess();
		}
		void StopFlashProcess(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		void StartFlashProcess(){
			thisProcess = thisAppleShooterProcessFactory.CreateCriticalFlashProcess(
				this,
				thisTypedAdaptor.GetFlashCurve(),
				thisTypedAdaptor.GetFlashTime()
			);
			thisProcess.Run();
		}
		ICriticalFlashProcess thisProcess;
		public void SetFlashValue(float flashValue){
			thisTypedAdaptor.SetFlashValue(flashValue);
		}


		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ICriticalFlashAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
