using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
using DKUtility;

namespace AppleShooterProto{
	public interface IHeadUpDisplay: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor, IProcessHandler{
	}
	public class HeadUpDisplay : AppleShooterSceneObject, IHeadUpDisplay {
		public HeadUpDisplay(
			IConstArg arg
		): base(
			arg
		){
			thisActivationStateEngine = new ActivationStateEngine(this);
			thisActivationProcessSuite = new ProcessSuite(
				thisProcessManager,
				this, 
				ProcessConstraint.ExpireTime,
				thisTypedAdaptor.GetActivationTime()
			);
			thisDeactivationProcessSuite = new ProcessSuite(
				thisProcessManager,
				this, 
				ProcessConstraint.ExpireTime,
				thisTypedAdaptor.GetDeactivationTime()
			);
		}
		IHeadUpDisplayAdaptor thisTypedAdaptor{
			get{
				return (IHeadUpDisplayAdaptor)thisAdaptor;
			}
		}
		IActivationStateEngine thisActivationStateEngine;
		public void Activate(){
			thisActivationStateEngine.Activate();
		}
		public void Deactivate(){
			thisActivationStateEngine.Deactivate();
		}
		public bool IsActivated(){
			return thisActivationStateEngine.IsActivated();
		}
		IProcessSuite thisActivationProcessSuite;
		IProcessSuite thisDeactivationProcessSuite;
		public void ActivateImple(){
			thisDeactivationProcessSuite.Stop();
			thisActivationProcessSuite.Start();
		}
		public void DeactivateImple(){
			thisActivationProcessSuite.Stop();
			thisDeactivationProcessSuite.Start();
		}
		public void OnProcessRun(IProcessSuite suite){
		}
		public void OnProcessExpire(IProcessSuite suite){
			OnProcessUpdate(.1f, 1f, suite);
		}
		public void OnProcessUpdate(float deltaTime, float normalizedTime, IProcessSuite suite){
			if(suite == thisActivationProcessSuite){
				OnActivationProcessUpdate(normalizedTime);
			}else if(suite == thisDeactivationProcessSuite){
				OnDeactivationProcessUpdate(normalizedTime);
			}
		}
		void OnActivationProcessUpdate(float normalizedTime){
			float curAlpha = thisTypedAdaptor.GetAlpha();
			float newAlpha = Mathf.Lerp(
				curAlpha,
				1f,
				normalizedTime
			);
			thisTypedAdaptor.SetAlpha(newAlpha);
		}
		void OnDeactivationProcessUpdate(float normalizedTime){
			float curAlpha = thisTypedAdaptor.GetAlpha();
			float newAlpha = Mathf.Lerp(
				curAlpha,
				0f,
				normalizedTime
			);
			thisTypedAdaptor.SetAlpha(newAlpha);
		}

		/*  */
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{
		}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IHeadUpDisplayAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}
}
