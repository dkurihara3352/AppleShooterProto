using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowState: ISwitchableState{
		void TryNock();
		void TryFire();
		void TryResetArrow();
		string GetName();
	}
	public abstract class AbsArrowState : IArrowState{
		public AbsArrowState(
			IArrowStateConstArg arg
		){
			thisEngine = arg.engine;
		}
		protected readonly IArrowStateEngine thisEngine;
		public abstract void OnEnter();
		public virtual void OnExit(){}
		public abstract void TryNock();
		public abstract void TryFire();
		public abstract void TryResetArrow();
		public abstract string GetName();
	}
	public interface IArrowStateConstArg{
		IArrowStateEngine engine{get;}
	}
	public struct ArrowStateConstArg: IArrowStateConstArg{
		public ArrowStateConstArg(
			IArrowStateEngine engine
		){
			thisEngine = engine;
		}
		readonly IArrowStateEngine thisEngine;
		public IArrowStateEngine engine{get{return thisEngine;}}
	}


	public interface IArrowReservedState: IArrowState{}
	public class ArrowReservedState: AbsArrowState, IArrowReservedState{
		public ArrowReservedState(
			IArrowStateConstArg arg
		): base(
			arg
		){

		}
		public override void OnEnter(){
			// thisEngine.CheckAndAddArrowToReserve();
			// thisEngine.MoveArrowToReservePosition();
			thisEngine.ResetArrow();
		}
		public override void TryNock(){
			thisEngine.SwitchToNockedState();
		}
		public override void TryFire(){
			throw new System.InvalidOperationException(
				"should not be able to fire an unnocked arrow"
			);
		}
		public override void TryResetArrow(){
			throw new System.InvalidOperationException(
				"this arrow is already reset"
			);
		}
		public override string GetName(){
			return "Reserved state";
		}
	}
	public interface IArrowNockedState: IArrowState{}
	public class ArrowNockedState: AbsArrowState, IArrowNockedState{
		public ArrowNockedState(
			IArrowStateConstArg arg
		): base(
			arg
		){}
		public override void OnEnter(){
			thisEngine.Nock();
		}
		public override void TryNock(){
			throw new System.InvalidOperationException(
				"this arrow is already nocked"
			);
		}
		public override void TryFire(){
			thisEngine.TryRegisterShot();
		}
		public override void TryResetArrow(){
			thisEngine.SwitchToReservedState();
		}
		public override string GetName(){
			return "Nocked state";
		}
	}
	public interface IArrowShotState: IArrowState{}
	public class ArrowShotState: AbsArrowState, IArrowShotState{
		public ArrowShotState(
			IArrowStateConstArg arg
		): base(arg){

		}
		public override void OnEnter(){
			thisEngine.Fire();
		}
		public override void TryNock(){
			thisEngine.AbortArrowFlight();
			thisEngine.SwitchToNockedState();
		}
		public override void TryFire(){
			throw new System.InvalidOperationException(
				"this arrow is already shot"
			);
		}
		public override void TryResetArrow(){
			thisEngine.SwitchToReservedState();
		}
		public override string GetName(){
			return "Shot state";
		}
	}
	public interface IArrowLandedState: IArrowState{}
	public class ArrowLandedState: AbsArrowState, IArrowLandedState{
		public ArrowLandedState(
			IArrowStateConstArg arg
		): base(arg){

		}
		public override void OnEnter(){
			return;
		}
		public override void TryNock(){
			thisEngine.SwitchToNockedState();
		}
		public override void TryFire(){
			throw new System.InvalidOperationException(
				"landed shot should not be fired"
			);
		}
		public override void TryResetArrow(){
			thisEngine.SwitchToReservedState();
		}
		public override string GetName(){
			return "Landed state";
		}
	}
}
