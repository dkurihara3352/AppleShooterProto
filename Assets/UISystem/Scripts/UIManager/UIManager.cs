using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IUIManager: IUISystemSceneObject{

		void SetPopUpManager(IPopUpManager manager);


		void SetDragWorldPosition(Vector2 dragPos);
		Vector2 GetDragWorldPosition();
		float GetSwipeVelocityThreshold();
		float GetSwipeDistanceThreshold();

		int registeredID{get;}
		bool TouchIDIsRegistered();
		void UnregisterTouchID();
		void RegisterTouchID(int touchID);


		void SetInputHandlingScroller(IScroller scroller, UIManager.InputName inputName);
		IScroller GetInputHandlingScroller();

		bool ShowsInputability();
		bool ShowsNormal();
		string GetEventName();


		IPopUpManager GetPopUpManager();

	}
	public class UIManager: UISystemSceneObject, IUIManager {
		public UIManager(
			IConstArg arg
		): base(arg){

			thisShowsInputability = arg.showsInputability;

			thisSwipeVelocityThreshold = arg.swipeVelocityThreshold;
			thisSwipeDistanceThreshold = arg.swipeDistanceThreshold;
		}
		readonly float thisSwipeVelocityThreshold;
		public float GetSwipeVelocityThreshold(){
			return thisSwipeVelocityThreshold;
		}
		readonly float thisSwipeDistanceThreshold;
		public float GetSwipeDistanceThreshold(){
			return thisSwipeDistanceThreshold;
		}
		/* Drag pos */
			Vector2 thisDragWorldPosition;
			public void SetDragWorldPosition(Vector2 dragPos){
				thisDragWorldPosition = dragPos;
			}
			public Vector2 GetDragWorldPosition(){return thisDragWorldPosition;}
		/* PopUpManager */
			IPopUpManager thisPopUpManager;
			public IPopUpManager GetPopUpManager(){return thisPopUpManager;}
			public void SetPopUpManager(IPopUpManager manager){
				thisPopUpManager = manager;
			}

		/* Touch management */
			const int noFingerID = -10;
			int thisRegisteredID = -10;
			public int registeredID{get{return thisRegisteredID;}}
			public bool TouchIDIsRegistered(){
				return thisRegisteredID != noFingerID;
			}
			public void UnregisterTouchID(){
				thisRegisteredID = noFingerID;
			}
			public void RegisterTouchID(int touchID){
				thisRegisteredID = touchID;
			}

		/* Debug */
			readonly bool thisShowsInputability;
			public bool ShowsInputability(){
				return thisShowsInputability;
			}
			public bool ShowsNormal(){
				return !ShowsInputability();
			}
			public void SetInputHandlingScroller(IScroller scroller, InputName inputName){
				thisInputHandlingScroller = scroller;
				thisInputName = inputName;
			}
			InputName thisInputName = InputName.None;
			public string GetEventName(){
				return thisInputName.ToString();
			}
		/* Input handling scroller */
			IScroller thisInputHandlingScroller;
			public IScroller GetInputHandlingScroller(){
				return thisInputHandlingScroller;
			}
		/*  */
			public enum InputName{
				None,
				Release,
				Tap,
				Swipe,
				BeginDrag,
				Drag,
				Touch,
			}
		/* Const */
			public new interface IConstArg: UISystemSceneObject.IConstArg{
				bool showsInputability{get;}
				float swipeVelocityThreshold{get;}
				float swipeDistanceThreshold{get;}
			}
			public new class ConstArg: UISystemSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IUIManagerAdaptor adaptor,
					bool showsInputability,
					float swipeVelocityThreshold,
					float swipeDistanceThreshold
				): base(
					adaptor
				){
					thisShowsInputability = showsInputability;
					thisSwipeVelocityThreshold = swipeVelocityThreshold;
					thisSwipeDistanceThreshold = swipeDistanceThreshold;
				}
				readonly bool thisShowsInputability;
				public bool showsInputability{get{return thisShowsInputability;}}
				readonly float thisSwipeVelocityThreshold;
				public float swipeVelocityThreshold{get{return thisSwipeVelocityThreshold;}}
				readonly float thisSwipeDistanceThreshold;
				public float swipeDistanceThreshold{get{return thisSwipeDistanceThreshold;}}
			}
	}
}
