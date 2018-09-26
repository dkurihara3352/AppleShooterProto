using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowStateEngine: ISwitchableStateEngine<IArrowState>{
		void SetLaunchPoint(ILaunchPoint launchPoint);
		void TryNock();
		void TryFire();
		void TryResetArrow();

		void SwitchToReservedState();
		void SwitchToNockedState();
		void SwitchToShotState();
		void SwitchToLandedState();


		void TryRegisterShot();
		void Nock();
		void Fire();
		void ResetArrow();
		void AbortArrowFlight();

		IArrowState GetCurrentState();
	}
	public class ArrowStateEngine : AbsSwitchableStateEngine<IArrowState>, IArrowStateEngine {

		public ArrowStateEngine(IArrowStateEngineConstArg arg){

			thisArrow = arg.arrow;

			IArrowStateConstArg stateConstArg = new ArrowStateConstArg(
				this
			);
			thisReservedState = new ArrowReservedState(stateConstArg);
			thisNockedState = new ArrowNockedState(stateConstArg);
			thisShotState = new ArrowShotState(stateConstArg);
			thisLandedState = new ArrowLandedState(stateConstArg);

			thisCurState = thisReservedState;
		}
		readonly IArrow thisArrow;
		ILaunchPoint thisLaunchPoint;
		public void SetLaunchPoint(ILaunchPoint launchPoint){
			thisLaunchPoint = launchPoint;
		}
		public IArrowState GetCurrentState(){
			return thisCurState;
		}
		/* states */
			readonly IArrowReservedState thisReservedState;
			readonly IArrowNockedState thisNockedState;
			readonly IArrowShotState thisShotState;
			readonly IArrowLandedState thisLandedState;
		/* state delegate */
			public void TryNock(){
				thisCurState.TryNock();
			}
			public void TryFire(){
				thisCurState.TryFire();
			}
			public void TryResetArrow(){
				thisCurState.TryResetArrow();
			}
		/* switch */
			public void SwitchToReservedState(){
				TrySwitchState(thisReservedState);
			}
			public void SwitchToNockedState(){
				TrySwitchState(thisNockedState);
			}
			public void SwitchToShotState(){
				TrySwitchState(thisShotState);
			}
			public void SwitchToLandedState(){
				TrySwitchState(thisLandedState);
			}
		/* action */
			public void ResetArrow(){
				thisArrow.ResetArrow();
			}
			public void TryRegisterShot(){
				thisArrow.TryRegisterShot();
			}
			public void Nock(){
				thisArrow.Nock();
			}
			public void Fire(){
				thisArrow.Fire();
			}
			public void AbortArrowFlight(){
				thisArrow.StopFlight();
			}
	}



	public interface IArrowStateEngineConstArg{
		IArrow arrow{get;}
		IAppleShooterProcessFactory processFactory{get;}
	}
	public struct ArrowStateEngineConstArg: IArrowStateEngineConstArg{
		public ArrowStateEngineConstArg(
			IArrow arrow,
			IAppleShooterProcessFactory processFactory
		){
			thisArrow = arrow;
			thisProcessFactory = processFactory;
		}
		readonly IArrow thisArrow;
		public IArrow arrow{get{return thisArrow;}}
		readonly IAppleShooterProcessFactory thisProcessFactory;
		public IAppleShooterProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}

