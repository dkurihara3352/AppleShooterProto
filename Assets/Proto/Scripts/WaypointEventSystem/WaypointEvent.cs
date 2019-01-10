using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IWaypointEvent{
		void Execute(IWaypointsFollower follower);
		float GetEventPoint();
		string GetName();
		bool IsExecuted();
		void Reset();
	}
	public abstract class AbsWaypointEvent: IWaypointEvent{
		public AbsWaypointEvent(
			IConstArg arg
		){
			thisEventPoint = arg.eventPoint;
			thisIsExecuted = false;
		}
		readonly float thisEventPoint;
		public float GetEventPoint(){
			return thisEventPoint;
		}
		bool thisIsExecuted;
		public bool IsExecuted(){
			return thisIsExecuted;
		}
		public void Execute(IWaypointsFollower follwer){
			if(this.IsExecutable()){
				thisIsExecuted = true;
				ExecuteImple(follwer);
			}
		}
		protected virtual bool IsExecutable(){return true;}
		protected abstract void ExecuteImple(IWaypointsFollower follower);
		public abstract string GetName();
		public virtual void Reset(){
			thisIsExecuted = false;
		}
		/*  */
		public interface IConstArg{
			float eventPoint{get;}
		}
		public class ConstArg: IConstArg{
			public ConstArg(
				float eventPoint
			){
				thisEventPoint = eventPoint;
			}
			readonly float thisEventPoint;
			public float eventPoint{get{return thisEventPoint;}}
		}
	}
}

