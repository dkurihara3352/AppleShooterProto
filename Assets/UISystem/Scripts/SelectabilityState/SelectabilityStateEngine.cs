using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DKUtility;

namespace UISystem{
	public interface ISelectabilityStateHandler{
		void BecomeSelectable();
		void BecomeUnselectable();
		void BecomeSelected();
		bool IsSelectable();
		bool IsSelected();
	}
	public interface ISelectabilityStateImplementor{
		void BecomeSelectableImple();
		void BecomeUnselectableImple();
		void BecomeSelectedImple();
	}
	public interface ISelectabilityStateEngine: ISelectabilityStateHandler{

	}
	public class SelectabilityStateEngine: DKUtility.AbsSwitchableStateEngine<SelectabilityStateEngine.IState>, ISelectabilityStateEngine{
		public SelectabilityStateEngine(
			ISelectabilityStateImplementor implementor
		){
			State.IConstArg stateArg = new State.ConstArg(
				implementor
			);
			selectableState = new SelectableState(
				stateArg
			);
			unselectableState = new UnselectableState(
				stateArg
			);
			selectedState = new SelectedState(
				stateArg
			);
			MakeSureStatesAreSet();

			// this.SetToInitialState();
			thisCurState = selectableState;
		}
		protected readonly SelectableState selectableState;
		protected readonly UnselectableState unselectableState;
		protected readonly SelectedState selectedState;
		void MakeSureStatesAreSet(){
			if(selectableState != null && unselectableState != null && selectedState != null)
				return;
			else
				throw new System.InvalidOperationException("any of the states not correctly set");
		}
		// void SetToInitialState(){
		// 	BecomeSelectable();
		// }
		/* SelStateHandler */
			public void BecomeSelectable(){
				TrySwitchState(selectableState);
			}
			public void BecomeUnselectable(){
				TrySwitchState(unselectableState);
			}
			public void BecomeSelected(){
				if(this.IsSelectable() || this.IsSelected())
					TrySwitchState(selectedState);
				else
					throw new System.InvalidOperationException("This method should not be called while this is not selectable");
			}
			public bool IsSelectable(){
				return thisCurState is SelectableState;
			}
			public bool IsSelected(){
				return thisCurState is SelectedState;
			}
		/* States */
			public interface IState: ISwitchableState{
			}
			public abstract class State: IState{
				public State(
					IConstArg arg
				){
					thisImplementor = arg.implementor;
				}
				protected ISelectabilityStateImplementor thisImplementor;
				public abstract void OnEnter();
				public virtual void OnExit(){}
				public interface IConstArg{
					ISelectabilityStateImplementor implementor{get;}
				}
				public struct ConstArg: IConstArg{
					public ConstArg(
						ISelectabilityStateImplementor implementor
					){
						thisImplementor = implementor;
					}
					readonly ISelectabilityStateImplementor thisImplementor;
					public ISelectabilityStateImplementor implementor{get{return thisImplementor;}}
				}
			}
			public class SelectableState: State, IState{
				public SelectableState(
					State.IConstArg arg
				): base(
					arg
				){}
				public override void OnEnter(){
					thisImplementor.BecomeSelectableImple();
				}
			}
			public class UnselectableState: State, IState{
				public UnselectableState(
					State.IConstArg arg
				): base(
					arg
				){}
				public override void OnEnter(){
					thisImplementor.BecomeUnselectableImple();
				}
			}
			public class SelectedState: State, IState{
				public SelectedState(
					State.IConstArg arg
				): base(
					arg
				){}
				public override void OnEnter(){
					
				}
			}
		}
}
