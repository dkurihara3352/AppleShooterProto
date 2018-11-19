using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowTrail: ISceneObject, IActivationStateHandler, IActivationStateImplementor{
		void SetArrowTrailReserve(IArrowTrailReserve reserve);
		void ActivateAt(IArrow arrow);
		void Detach();

		void SetAlpha(float alpha);
	}
	public class ArrowTrail : AbsSceneObject, IArrowTrail {

		public ArrowTrail(
			IConstArg arg
		): base(arg){
			
			thisFadeTime = arg.fadeTime;
			thisStateEngine = new ActivationStateEngine(this);
		}
		IActivationStateEngine thisStateEngine;
		public void Activate(){
			thisStateEngine.Activate();
		}
		public void Deactivate(){
			thisStateEngine.Deactivate();
		}
		public bool IsActivated(){
			return thisStateEngine.IsActivated();
		}
		IArrowTrailReserve thisReserve;
		public void SetArrowTrailReserve(IArrowTrailReserve reserve){
			thisReserve = reserve;
		}
		public void ActivateImple(){
			thisTypedAdaptor.EnableTrailRenderer();
			thisTypedAdaptor.ResetAlpha();
			thisTypedAdaptor.ClearRenderer();
			StartFade();
		}
		IArrowTrailAdaptor thisTypedAdaptor{
			get{
				return (IArrowTrailAdaptor)thisAdaptor;
			}
		}
		public void ActivateAt(IArrow arrow){
			Deactivate();
			SetParent(arrow);
			thisTargetArrow = arrow;
			ResetLocalTransform();
			arrow.SetArrowTrail(this);
			Activate();
		}
		public void DeactivateImple(){
			Detach();
			StopFade();
			thisTypedAdaptor.DisableTrailRenderer();
			thisReserve.Reserve(this);
		}
		IArrow thisTargetArrow;
		public void Detach(){
			SetParent(null);
			if(thisTargetArrow != null)
				thisTargetArrow.CheckAndClearArrowTrail(this);
			thisTargetArrow = null;
		}

		IArrowTrailFadeProcess thisProcess;
		readonly float thisFadeTime;
		void StartFade(){
			StopFade();
			thisProcess = thisProcessFactory.CreateArrowTrailFadeProcess(
				thisFadeTime,
				this,
				thisTypedAdaptor.GetAlpha()
			);
			thisProcess.Run();
		}
		void StopFade(){
			if(thisProcess != null && thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		public void SetAlpha(float alpha){
			thisTypedAdaptor.SetAlpha(alpha);
		}
	

		public new interface IConstArg: AbsSceneObject.IConstArg{
			float fadeTime{get;}
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IArrowTrailAdaptor adaptor,
				float fadeTime
			): base(adaptor){
				thisFadeTime = fadeTime;
			}
			readonly float thisFadeTime;
			public float fadeTime{get{return thisFadeTime;}}
		}
	}
}
