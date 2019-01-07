using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IArrowStateHandler{
		void Nock();
		void Release();
		void Deactivate();
	}
	public interface IArrowStateImplementor{
		void NockImple();
		void ReleaseImple();
		void DeactivateImple();
	}
	public interface IArrowStateEngine: ISwitchableStateEngine<ArrowStateEngine.IState>, IArrowStateHandler{
		void SwitchToNockedState();
		void SwitchToFlightState();
		void SwitchToDeactivatedState();
		bool IsInFlight();
		bool IsActivated();
		bool IsInReserve();
		ArrowStateEngine.IState GetCurrentState();
	}
	public class ArrowStateEngine : AbsSwitchableStateEngine<ArrowStateEngine.IState>, IArrowStateEngine {

		public ArrowStateEngine(
			IConstArg arg
		){
			thisArrow = arg.arrow;

			AbsState.IConstArg stateConstArg = new AbsState.ConstArg(
				thisArrow,
				this
			);
			thisNockedState = new NockedState(stateConstArg);
			thisFlightState = new FlightState(stateConstArg);
			thisDeactivatedState = new DeactivatedState(stateConstArg);
		}
		readonly IArrow thisArrow;
		public IState GetCurrentState(){
			return thisCurState;
		}
		public bool IsInFlight(){
			return thisCurState == thisFlightState;
		}
		public bool IsActivated(){
			return thisIsInitialized && thisCurState != thisDeactivatedState;
		}
		public bool IsInReserve(){
			return thisCurState == thisDeactivatedState;
		}
		/* states */
			INockedState thisNockedState;
			IFlightState thisFlightState;
			IDeactivatedState thisDeactivatedState;
		/* state handler */
			public void Nock(){
				thisCurState.Nock();
			}
			public void Release(){
				thisCurState.Release();
			}
			bool thisIsInitialized = false;
			public void Deactivate(){
				if(!thisIsInitialized){
					SwitchToDeactivatedState();
					thisIsInitialized = true;
				}else
					thisCurState.Deactivate();
			}
		/* switch */
			public void SwitchToNockedState(){
				TrySwitchState(thisNockedState);
			}
			public void SwitchToFlightState(){
				TrySwitchState(thisFlightState);
			}
			public void SwitchToDeactivatedState(){
				TrySwitchState(thisDeactivatedState);
			}
		/* ConstArg */
			public interface IConstArg{
				IArrow arrow{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IArrow arrow
				){
					thisArrow = arrow;
				}
				readonly IArrow thisArrow;
				public IArrow arrow{get{return thisArrow;}}
			}
		/* States */
			public interface IState: ISwitchableState, IArrowStateHandler{
				string GetName();
			}
			public abstract class AbsState: IState{
				public AbsState(
					IConstArg arg
				){
					thisArrow = arg.arrow;
					thisEngine = arg.engine;
				}
				protected IArrow thisArrow;
				protected IArrowStateEngine thisEngine;
				public abstract void OnEnter();
				public virtual void OnExit(){}
				public abstract void Nock();
				public abstract void Release();
				public abstract void Deactivate();
				public abstract string GetName();
				/* Const */
					public interface IConstArg{
						IArrow arrow{get;}
						IArrowStateEngine engine{get;}
					}
					public struct ConstArg: IConstArg{
						public ConstArg(
							IArrow arrow,
							IArrowStateEngine engine
						){
							thisArrow = arrow;
							thisEngine = engine;
						}
						readonly IArrow thisArrow;
						public IArrow arrow{get{return thisArrow;}}
						readonly IArrowStateEngine thisEngine;
						public IArrowStateEngine engine{get{return thisEngine;}}
					}

			}
			public interface INockedState: IState{}
			public class NockedState: AbsState, INockedState{
				public NockedState(
					AbsState.IConstArg arg
				): base(arg){}
				public override void OnEnter(){
					thisArrow.NockImple();
				}
				public override void Nock(){
					throw new System.InvalidOperationException(
						"state is already nocked"
					);
				}
				public override void Release(){
					thisEngine.SwitchToFlightState();
				}
				public override void Deactivate(){
					thisEngine.SwitchToDeactivatedState();
				}
				public override string GetName(){
					return "Nocked State";
				}
			}
			public interface IFlightState: IState{}
			public class FlightState: AbsState, IFlightState{
				public FlightState(
					IConstArg arg
				): base(arg){}
				public override void OnEnter(){
					thisArrow.ReleaseImple();
				}
				public override void Nock(){
					thisArrow.Deactivate();
					thisArrow.Nock();
				}
				public override void Release(){
					throw new System.InvalidOperationException(
						"state is alreadly in flight"
					);
				}
				public override void Deactivate(){
					thisEngine.SwitchToDeactivatedState();
				}
				public override string GetName(){
					return "Flight State";
				}
			}
			public interface IDeactivatedState: IState{}
			public class DeactivatedState: AbsState, IDeactivatedState{
				public DeactivatedState(
					IConstArg arg
				): base(arg){}
				public override void OnEnter(){
					thisArrow.DeactivateImple();
				}
				public override void Nock(){
					thisEngine.SwitchToNockedState();
				}
				public override void Release(){
					thisArrow.Nock();
					thisArrow.Release();
				}
				public override void Deactivate(){
					throw new System.InvalidOperationException(
						"curState is already deactivated"
					);
				}
				public override string GetName(){
					return "Deactivated State";
				}
			}
		/*  */
	}
}

