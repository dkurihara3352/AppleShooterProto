using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace AppleShooterProto{
	public interface IShootingTargetActivationStateEngine: ISwitchableStateEngine<ShootingTargetActivationStateEngine.IState>{
		void Activate();
		void Deactivate();
		bool IsActivated();

		void SwitchToActivatedState();
		void SwitchToDeactivatedState();

		void ActivateImpleTarget();
		void DeactivateImpleTarget();
	}
	public class ShootingTargetActivationStateEngine : AbsSwitchableStateEngine<ShootingTargetActivationStateEngine.IState>, IShootingTargetActivationStateEngine {
		public ShootingTargetActivationStateEngine(
			IShootingTarget target
		){
			thisTarget = target;
			thisActivatedState = new ActivatedState(this);
			thisDeactivatedState = new DeactivatedState(this);
			thisCurState  = thisActivatedState;//in order to deactivate at FinalizeSetUp in the adaptor
		}
		IShootingTarget thisTarget;
		IActivatedState thisActivatedState;
		IDeactivatedState thisDeactivatedState;

		public void Activate(){
			thisCurState.Activate();
		}
		public void Deactivate(){
			thisCurState.Deactivate();
		}
		public bool IsActivated(){
			return thisCurState == thisActivatedState;
		}
		public void SwitchToActivatedState(){
			TrySwitchState(thisActivatedState);
		}
		public void SwitchToDeactivatedState(){
			TrySwitchState(thisDeactivatedState);
		}
		public void ActivateImpleTarget(){
			thisTarget.ActivateImple();
		}
		public void DeactivateImpleTarget(){
			thisTarget.DeactivateImple();
		}
		/* States */
			public interface IState: ISwitchableState{
				void Activate();
				void Deactivate();
			}
			public abstract class AbsState: IState{
				public AbsState(IShootingTargetActivationStateEngine engine){
					thisEngine = engine;
				}
				protected IShootingTargetActivationStateEngine thisEngine;
				public abstract void OnEnter();
				public virtual void OnExit(){}
				public abstract void Activate();
				public abstract void Deactivate();

			}
			public interface IActivatedState: IState{}
			public class ActivatedState: AbsState, IActivatedState{
				public ActivatedState(
					IShootingTargetActivationStateEngine engine
				): base(
					engine
				){
				}
				public override void OnEnter(){
					thisEngine.ActivateImpleTarget();
				}
				public override void Activate(){
					return;
				}
				public override void Deactivate(){
					thisEngine.SwitchToDeactivatedState();
				}
			}
			public interface IDeactivatedState: IState{}
			public class DeactivatedState: AbsState, IDeactivatedState{
				public DeactivatedState(
					IShootingTargetActivationStateEngine engine
				): base(engine){

				}
				public override void OnEnter(){
					thisEngine.DeactivateImpleTarget();
				}
				public override void Activate(){
					thisEngine.SwitchToActivatedState();
				}
				public override void Deactivate(){
					return;
				}
			}
		/*  */
	}
}
