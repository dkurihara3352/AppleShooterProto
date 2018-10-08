using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ITestShootingTargetHitStateEngine: ISwitchableStateEngine<TestShootingTargetHitStateEngine.IState>{
		void SwitchToWaitingForHitState();
		void SwitchToBeingHitState();

		void Hit(float magnitude);

		void CheckAndStartNewHitAnimation(float magnitude);
		void SetMagnitude(float magnitude);
	}
	public class TestShootingTargetHitStateEngine: AbsSwitchableStateEngine<TestShootingTargetHitStateEngine.IState>, ITestShootingTargetHitStateEngine{
		public TestShootingTargetHitStateEngine(
			IConstArg arg
		){
			thisTarget = arg.target;
			AbsTestShootingTargetHitState.IConstArg stateArg = new AbsTestShootingTargetHitState.ConstArg(this);
			thisWaitingForHitState = new WaitingForHitState(stateArg);
			thisBeingHitState = new BeingHitState(stateArg);
			thisCurState = thisWaitingForHitState;
		}
		readonly ITestShootingTarget thisTarget;
		IWaitingForHitState thisWaitingForHitState;
		IBeingHitState thisBeingHitState;
		public void SwitchToWaitingForHitState(){
			TrySwitchState(thisWaitingForHitState);
		}
		public void SwitchToBeingHitState(){
			TrySwitchState(thisBeingHitState);
		}
		public void Hit(float magnitude){
			thisCurState.Hit(magnitude);
		}
		public void CheckAndStartNewHitAnimation(float magnitude){
			thisTarget.CheckAndStartNewHitAnimation(magnitude);
		}
		public void SetMagnitude(float magnitude){
			thisBeingHitState.SetMagnitude(magnitude);
		}
		/* Const */
			public interface IConstArg{
				ITestShootingTarget target{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					ITestShootingTarget target
				){
					thisTarget = target;
				}
				readonly ITestShootingTarget thisTarget;
				public ITestShootingTarget target{get{return thisTarget;}}
			}
		/* states */		
			public interface IState: ISwitchableState{
				void Hit(float magnitude);
			}
			public abstract class AbsTestShootingTargetHitState: IState{
				public AbsTestShootingTargetHitState(
					IConstArg arg
				){
					thisEngine = arg.engine;
				}
				protected ITestShootingTargetHitStateEngine thisEngine;
				public abstract void Hit(float magnitude);
				public virtual void OnEnter(){
					return;
				}
				public virtual void OnExit(){
					return;
				}
				public interface IConstArg{
					ITestShootingTargetHitStateEngine engine{get;}
				}
				public struct ConstArg: IConstArg{
					public ConstArg(ITestShootingTargetHitStateEngine engine){
						thisEngine = engine;
					}
					readonly ITestShootingTargetHitStateEngine thisEngine;
					public ITestShootingTargetHitStateEngine engine{get{return thisEngine;}}
				}
			}
			public interface IWaitingForHitState: IState{}
			public class WaitingForHitState: AbsTestShootingTargetHitState, IWaitingForHitState{
				public WaitingForHitState(
					AbsTestShootingTargetHitState.IConstArg arg
				): base(arg){}
				public override void Hit(float magnitude){
					thisEngine.SetMagnitude(magnitude);
					thisEngine.SwitchToBeingHitState();
				}
			}
			public interface IBeingHitState: IState{
				void SetMagnitude(float magnitude);
			}
			public class BeingHitState: AbsTestShootingTargetHitState, IBeingHitState{
				public BeingHitState(
					AbsTestShootingTargetHitState.IConstArg arg
				): base(arg){}
				public void SetMagnitude(float magnitude){
					thisMagnitude = magnitude;
				}
				float thisMagnitude;
				public override void OnEnter(){
					thisEngine.CheckAndStartNewHitAnimation(thisMagnitude);
				}
				public override void Hit(float magnitude){
					thisEngine.CheckAndStartNewHitAnimation(magnitude);
				}
			}
		/*  */
	}
}
