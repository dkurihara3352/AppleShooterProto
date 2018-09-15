using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPointerDownInputState: IUIAdaptorInputState{}
	public abstract class AbsPointerDownInputState: AbsUIAdaptorInputState, IPointerDownInputState{
		public AbsPointerDownInputState(
			IPointerDownInputStateConstArg arg
		): base(
			arg
		){
			thisUIM = arg.uiManager;
			thisVelocityStackSize = arg.velocityStackSize;

			thisVelocityStack = new Vector2[thisVelocityStackSize];
			thisSwipeDistanceThreshold = arg.swipeDistanceThreshold;
		}
		int thisVelocityStackSize;
		public override void OnEnter(){
			thisVelocityStack = new Vector2[thisVelocityStackSize];
		}
		public override void OnPointerDown(ICustomEventData eventData){
			throw new System.InvalidOperationException("OnPointerDown should not be called while pointer is already held down");
		}
		readonly float thisSwipeVelocityThreshold;
		protected bool VelocityIsOverSwipeThreshold(Vector2 velocity){
			if(velocity.sqrMagnitude >= thisSwipeVelocityThreshold * thisSwipeVelocityThreshold)
				return true;
			else
				return false;
		}
		protected bool ShouldSwipe(ICustomEventData eventData){
			if(VelocityIsOverSwipeThreshold(eventData.velocity)){
				if(EnoughDistanceIsCoveredSinceTouch(eventData.position)){
					return true;
				}
			}
			return false;
		}
		readonly float thisSwipeDistanceThreshold;
		bool EnoughDistanceIsCoveredSinceTouch(Vector3 pointerPosition){
			Vector3 touchPosition = thisEngine.GetTouchPosition();
			Vector3 displacement = pointerPosition - touchPosition;
			if(displacement.sqrMagnitude >= thisSwipeDistanceThreshold * thisSwipeDistanceThreshold)
				return true;
			else
				return false;
		}
		readonly IUIManager thisUIM;
		void UpdateDragWorldPosition(Vector2 dragWorldPosition){
			thisUIM.SetDragWorldPosition(dragWorldPosition);
		}
		public override void OnBeginDrag(ICustomEventData eventData){
			thisEngine.BeginDragUIE(eventData);
			PushVelocityStack(eventData.velocity);
		}
		public override void OnDrag(ICustomEventData eventData){
			thisEngine.DragUIE(eventData);
			UpdateDragWorldPosition(eventData.position);
			PushVelocityStack(eventData.velocity);
		}
		protected void PushVelocityStack(Vector2 velocity){
			int stackSize = thisVelocityStack.Length;
			Vector2[] newStack = new Vector2[stackSize];
			for(int i = 0; i < stackSize; i ++){
				if(i < stackSize -1)
					newStack[i] = thisVelocityStack[i + 1];
				else
					newStack[i] = velocity;
			}
			thisVelocityStack = newStack;
		}
		protected Vector2[] thisVelocityStack;
		protected Vector2 GetAverageVelocity(){
			int stackSize = thisVelocityStack.Length;
			Vector2 sum = Vector2.zero;
			int nonZeroCount = 0;
			for(int i = 0; i < stackSize; i ++){
				if(thisVelocityStack[i] != Vector2.zero){
					nonZeroCount ++;
					sum += thisVelocityStack[i];
				}
			}
			return sum/ nonZeroCount;
		}
	}
	public interface IPointerDownInputStateConstArg: IUIAdaptorInputStateConstArg{
		IUIManager uiManager{get;}
		int velocityStackSize{get;}
		float swipeVelocityThreshold{get;}
		float swipeDistanceThreshold{get;}
	}
	public class PointerDownInputStateConstArg: UIAdaptorInputStateConstArg, IPointerDownInputStateConstArg{
		public PointerDownInputStateConstArg(
			IUIAdaptorInputStateEngine engine,
			IUIManager uiManager,
			int velocityStackSize,
			float swipeVelocityThreshold,
			float swipeDistanceThreshold
		): base(
			engine
		){
			thisUIManager = uiManager;
			thisVelocityStackSize = velocityStackSize;
			thisSwipeVelocityThreshold = swipeVelocityThreshold;
			thisSwipeDistanceThreshold = swipeDistanceThreshold;
		}
		readonly IUIManager thisUIManager;
		public IUIManager uiManager{get{return thisUIManager;}}
		readonly int thisVelocityStackSize;
		public int velocityStackSize{
			get{return thisVelocityStackSize;}
		}
		readonly float thisSwipeVelocityThreshold;
		public float swipeVelocityThreshold{get{return thisSwipeVelocityThreshold;}}
		readonly float thisSwipeDistanceThreshold;
		public float swipeDistanceThreshold{get{return thisSwipeDistanceThreshold;}}
	}
	public abstract class AbsPointerDownInputProcessState<T>: AbsPointerDownInputState where T: class, IUIAdaptorInputProcess{
		public AbsPointerDownInputProcessState(
			IPointerDownInputProcessStateConstArg arg
		): base(
			arg
			
		){
			thisProcessFactory = arg.processFactory;
		}
		readonly protected IUISystemProcessFactory thisProcessFactory;
		protected T thisProcess;
		public override void OnEnter(){
			base.OnEnter();
			thisProcess = CreateProcess();
			thisProcess.Run();
		}
		public override void OnExit(){
			if(thisProcess.IsRunning())
				thisProcess.Stop();
			thisProcess = null;
		}
		protected abstract T CreateProcess();
		public virtual void ExpireProcess(){
			if(thisProcess != null)
				if(thisProcess.IsRunning())
					thisProcess.Expire();
		}
	}
	public interface IPointerDownInputProcessStateConstArg: IPointerDownInputStateConstArg{
		IUISystemProcessFactory processFactory{get;}
	}
	public class PointerDownInputProcessStateConstArg: PointerDownInputStateConstArg, IPointerDownInputProcessStateConstArg{
		public PointerDownInputProcessStateConstArg(
			IUIAdaptorInputStateEngine engine,
			IUIManager uiManager,
			int velocityStackSize,
			float swipeVelocityThreshold,
			float swipeDistanceThreshold,

			IUISystemProcessFactory processFactory
		):base(
			engine,
			uiManager,
			velocityStackSize,
			swipeVelocityThreshold,
			swipeDistanceThreshold
		){
			thisProcessFactory = processFactory;
		}
		readonly IUISystemProcessFactory thisProcessFactory;
		public IUISystemProcessFactory processFactory{get{return thisProcessFactory;}}
	}
}

