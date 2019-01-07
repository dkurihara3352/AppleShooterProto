using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface ICriticalFlash: ISlickBowShootingSceneObject{
		void Flash();

		void SetFlashValue(float normalizedFlashValue);
	}
	public class CriticalFlash : SlickBowShootingSceneObject, ICriticalFlash {
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
			thisProcess = thisSlickBowShootingProcessFactory.CreateCriticalFlashProcess(
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


		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				ICriticalFlashAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
