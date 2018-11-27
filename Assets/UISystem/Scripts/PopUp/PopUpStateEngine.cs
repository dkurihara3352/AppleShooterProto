using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IPopUpStateHandler{
		void Hide(bool instantly);
		void Show(bool instantly);
	}
	public interface IPopUpStateImplementor{
		void OnHideBegin();
		void OnHideComplete();
		void OnShowBegin();
		void OnShowComplete();

		float GetAlpha();
		void SetAlpha(float alpha);
	}
	public interface IPopUpStateEngine: IPopUpStateHandler{
		void SwitchToHiddenState();
		void SwitchToHidingState();
		void SwitchToShownState();
		void SwitchToShowingState();
		/*  */
		void ExpireCurrentProcess();
		void StartNewHideProcess();
		void StartNewShowProcess();

		/*  */
		bool IsHidden();
		bool IsShown();

		void SetAlphaOnImplementor(float alpha);
		float GetAphaOnImplementor();
	}
	public class PopUpStateEngine :AbsSwitchableStateEngine<PopUpStateEngine.IState>, IPopUpStateEngine, ISwitchableStateEngine<PopUpStateEngine.IState> {

		public PopUpStateEngine(
			IConstArg arg
		){

			thisProcessFactory = arg.processFactory;
			thisImplementor = arg.implementor;
			thisProcessTime = arg.processTime;
			State.IConstArg stateArg = new State.ConstArg(
				this,
				arg.implementor
			);
			thisHiddenState = new HiddenState(stateArg);
			thisHidingState = new HidingState(stateArg);
			thisShownState = new ShownState(stateArg);
			thisShowingState = new ShowingState(stateArg);
			
			thisCurState = thisHiddenState;
		}
		readonly IUISystemProcessFactory thisProcessFactory;
		readonly PopUpMode thisPopUpMode;
		readonly IPopUpStateImplementor thisImplementor;
		readonly float thisProcessTime;
		/* states */
		readonly IHiddenState thisHiddenState;
		readonly IHidingState thisHidingState;
		readonly IShownState thisShownState;
		readonly IShowingState thisShowingState;
		/* switch */
		public void SwitchToHiddenState(){
			TrySwitchState(thisHiddenState);
		}
		public void SwitchToHidingState(){
			TrySwitchState(thisHidingState);
		}
		public void SwitchToShownState(){
			TrySwitchState(thisShownState);
		}
		public void SwitchToShowingState(){
			TrySwitchState(thisShowingState);
		}
		/*  */

		/* Process */
		IPopUpProcess thisRunningProcess;
		public void ExpireCurrentProcess(){
			if(thisRunningProcess != null)
				if(thisRunningProcess.IsRunning())
					thisRunningProcess.Expire();
		}
		void SetRunningProcess(IPopUpProcess process){
			if(thisRunningProcess != null)
				if(thisRunningProcess.IsRunning())
					thisRunningProcess.Stop();
			thisRunningProcess = process;
		}
		IPopUpProcess CreatePopUpProcess(bool hides){
			IPopUpProcess newProcess;
			switch(thisPopUpMode){
				case PopUpMode.Alpha:
					newProcess = thisProcessFactory.CreateAlphaPopUpProcess(
						this, 
						thisProcessTime,
						hides
					);
					break;
				default: 
					newProcess = null;
					break;
			}
			return newProcess;
		}
		public void StartNewHideProcess(){
			IPopUpProcess newPorcess = CreatePopUpProcess(true);
			newPorcess.Run();
			SetRunningProcess(newPorcess);
		}
		public void StartNewShowProcess(){
			IPopUpProcess newProcess = CreatePopUpProcess(false);
			newProcess.Run();
			SetRunningProcess(newProcess);
		}
		/*  */
		public void Hide(bool instantly){
			thisCurState.Hide(instantly);
		}
		public void Show(bool instantly){
			thisCurState.Show(instantly);
		}
		public bool IsHidden(){
			return thisCurState == thisHiddenState ||
				thisCurState == thisHidingState;
		}
		public bool IsShown(){
			return thisCurState == thisShownState ||
				thisCurState == thisShowingState;
		}
		/*  */
		public void SetAlphaOnImplementor(float alpha){
			thisImplementor.SetAlpha(alpha);
		}
		public float GetAphaOnImplementor(){
			return thisImplementor.GetAlpha();
		}

		/* states */
			public interface IState: ISwitchableState, IPopUpStateHandler{}
			public abstract class State : IState {
				public State(IConstArg arg){
					thisEngine = arg.engine;
					thisImplementor = arg.implementor;
				}
				protected readonly IPopUpStateEngine thisEngine;
				protected readonly IPopUpStateImplementor thisImplementor;
				public abstract void OnEnter();
				public virtual void OnExit(){
					return;
				}
				public abstract void Hide(bool instantly);
				public abstract void Show(bool instantly);

				public interface IConstArg{
					IPopUpStateEngine engine{get;}
					IPopUpStateImplementor implementor{get;}
				}
				public class ConstArg: IConstArg{
					public ConstArg(
						IPopUpStateEngine engine,
						IPopUpStateImplementor implementor
					){
						thisEngine = engine;
						thisImplementor = implementor;
					}
					readonly IPopUpStateEngine thisEngine;
					public IPopUpStateEngine engine{get{return thisEngine;}}
					readonly IPopUpStateImplementor thisImplementor;
					public IPopUpStateImplementor implementor{get{return thisImplementor;}}

				}

			}
			/* HiddenState */
				public interface IHiddenState: IState{}
				public class HiddenState: State, IHiddenState{
					public HiddenState(State.IConstArg arg): base(arg){}
					public override void OnEnter(){
						thisImplementor.OnHideComplete();
					}
					public override void Hide(bool instantly){
						return;
					}
					public override void Show(bool instantly){
						thisEngine.SwitchToShowingState();
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
				}
			/* Hiding State */
				public interface IHidingState: IState{}
				public class HidingState: State, IHidingState{
					public HidingState(State.IConstArg arg): base(arg){}
					public override void OnEnter(){
						thisImplementor.OnHideBegin();
						thisEngine.StartNewHideProcess();
					}
					public override void Hide(bool instantly){
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
					public override void Show(bool instantly){
						thisEngine.SwitchToShowingState();
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
				}
			/* ShownState */
				public interface IShownState: IState{}
				public class ShownState: State, IShownState{
					public ShownState(State.IConstArg arg): base(arg){}
					public override void OnEnter(){
						thisImplementor.OnShowComplete();
					}
					public override void Hide(bool instantly){
						thisEngine.SwitchToHidingState();
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
					public override void Show(bool instantly){
						return;
					}
				}
			/* Showing State */
				public interface IShowingState: IState{}
				public class ShowingState: State, IShowingState{
					public ShowingState(State.IConstArg arg): base(arg){}
					public override void OnEnter(){
						thisEngine.StartNewShowProcess();
						thisImplementor.OnShowBegin();
					}
					public override void Hide(bool instantly){
						thisEngine.SwitchToHidingState();
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
					public override void Show(bool instantly){
						if(instantly)
							thisEngine.ExpireCurrentProcess();
					}
				}
		/* ConstArg */
			public interface IConstArg{
				IUISystemProcessFactory processFactory{get;}
				IPopUpStateImplementor implementor{get;}
				float processTime{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IUISystemProcessFactory processFactory,
					IPopUpStateImplementor implementor,
					float processTime
				){
					thisProcessFactory = processFactory;
					thisImplementor = implementor;
					thisProcessTime = processTime;
				}
				readonly IUISystemProcessFactory thisProcessFactory;
				public IUISystemProcessFactory processFactory{get{return thisProcessFactory;}}
				readonly IPopUpStateImplementor thisImplementor;
				public IPopUpStateImplementor implementor{get{return thisImplementor;}}
				readonly float thisProcessTime;
				public float processTime{get{return thisProcessTime;}}
			}
	}
}
