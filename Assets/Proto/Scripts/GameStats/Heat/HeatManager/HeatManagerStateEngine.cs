using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;
namespace SlickBowShooting{
	public interface IHeatManagerStateHandler{
		void StartCountingDown();
		void StopCountingDown();
	}
	public interface IHeatManagerStateEngine: ISwitchableStateEngine<HeatManagerStateEngine.IState>, IHeatManagerStateHandler{
		void SwitchToWaitingForCountDownStartState();
		void SwitchToCountingDownState();
		void StartCountDownProcess();
		void StopCountDownProcess();

		void TickAwayHeat(float delta);
		float GetMaxHeat();

		bool IsCountingDown();
	}
	public class HeatManagerStateEngine: AbsSwitchableStateEngine<HeatManagerStateEngine.IState>, IHeatManagerStateEngine{
		public HeatManagerStateEngine(
			IConstArg arg
		){
			thisHeatManager = arg.heatManager;
			thisProcessFactory = arg.processFactory;
			thisHeatDecayRate = arg.heatDecayRate;
			thisWaitingForCountDownState = new WaitingForCountDownState(this);
			thisCountingDownState = new CountingDownState(this);

			thisCurState = thisCountingDownState;
		}
		IWaitingForCountDownState thisWaitingForCountDownState;
		ICountingDownState thisCountingDownState;
		public void SwitchToWaitingForCountDownStartState(){
			TrySwitchState(thisWaitingForCountDownState);
		}
		public void SwitchToCountingDownState(){
			TrySwitchState(thisCountingDownState);
		}
		ISlickBowShootingProcessFactory thisProcessFactory;
		IHeatCountDownProcess thisCountDownProcess;
		float thisHeatDecayRate;
		public void StartCountDownProcess(){
			StopCountDownProcess();
			thisCountDownProcess = thisProcessFactory.CreateHeatCountDownProcess(
				thisHeatDecayRate,
				this
			);
			thisCountDownProcess.Run();
		}
		public void StopCountDownProcess(){
			if(thisCountDownProcess != null)
				thisCountDownProcess.Stop();
			thisCountDownProcess = null;
		}
		IHeatManager thisHeatManager;
		public void TickAwayHeat(float delta){
			thisHeatManager.TickAwayHeat(delta);
		}
		public void StartCountingDown(){
			thisCurState.StartCountingDown();
		}
		public void StopCountingDown(){
			 thisCurState.StopCountingDown();
		}
		public float GetMaxHeat(){
			return thisHeatManager.GetMaxHeat();
		}
		public bool IsCountingDown(){
			return thisCurState == thisCountingDownState;
		}
		/* States */
			public interface IState: ISwitchableState, IHeatManagerStateHandler{}
			public abstract class AbsState: IState{
				protected IHeatManagerStateEngine thisEngine;
				public AbsState(
					IHeatManagerStateEngine engine
				){
					thisEngine = engine;
				}
				public abstract void OnEnter();
				public virtual void OnExit(){}
				public abstract void StartCountingDown();
				public abstract void StopCountingDown();
			}
			public interface IWaitingForCountDownState: IState{
			}
			public class WaitingForCountDownState: AbsState, IWaitingForCountDownState{
				public WaitingForCountDownState(
					IHeatManagerStateEngine engine
				): base(engine){}
				float thisHoldTime;
				public override void OnEnter(){
					thisEngine.StopCountDownProcess();
				}
				public override void StartCountingDown(){
					thisEngine.SwitchToCountingDownState();
				}
				public override void StopCountingDown(){
					return;
				}
			}
			public interface ICountingDownState: IState{}
			public class CountingDownState: AbsState, ICountingDownState{
				public CountingDownState(
					IHeatManagerStateEngine engine
				): base(engine){}
				public override void OnEnter(){
					thisEngine.StartCountDownProcess();
				}
				public override void StartCountingDown(){
					thisEngine.StartCountDownProcess();
				}
				public override void StopCountingDown(){
					thisEngine.SwitchToWaitingForCountDownStartState();
				}
			}
		/* ConstArg */
			public interface IConstArg{
				IHeatManager heatManager{get;}
				ISlickBowShootingProcessFactory processFactory{get;}
				float heatDecayRate{get;}
			}
			public class ConstArg: IConstArg{
				public ConstArg(
					IHeatManager heatManager,
					float heatDecayRate,
					ISlickBowShootingProcessFactory processFactory
				){
					thisHeatManager = heatManager;
					thisHeatDecayRate = heatDecayRate;
					thisProcessFactory = processFactory;
				}
				readonly ISlickBowShootingProcessFactory thisProcessFactory;
				public ISlickBowShootingProcessFactory processFactory{
					get{return thisProcessFactory;}
				}
				readonly IHeatManager thisHeatManager;
				public IHeatManager heatManager{
					get{return thisHeatManager;}
				}
				readonly float thisHeatDecayRate;
				public float heatDecayRate{
					get{return thisHeatDecayRate;}
				}
			}
		
	}
}

