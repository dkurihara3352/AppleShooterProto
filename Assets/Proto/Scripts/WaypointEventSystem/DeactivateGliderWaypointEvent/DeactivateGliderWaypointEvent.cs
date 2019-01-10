using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IDeactivateGliderWaypointEvent: IWaypointEvent{
	}
	public class DeactivateGliderWaypointEvent: AbsWaypointEvent, IDeactivateGliderWaypointEvent{
		public DeactivateGliderWaypointEvent(IConstArg arg): base(arg){
			thisDeactivateGliderWaypointEventAdaptor = arg.adaptor;
		}
		IDeactivateGliderWaypointEventAdaptor thisDeactivateGliderWaypointEventAdaptor;
		public override string GetName(){
			return "DeactivateGliderWaypointEvent";
		}
		protected override void ExecuteImple(IWaypointsFollower follower){
			IGliderWaypointsFollower gliderWaypointsFollower = (IGliderWaypointsFollower)follower;
			IGlidingTarget glider = gliderWaypointsFollower.GetGlider();
			glider.Deactivate();
		}

		/* Const */
		public new interface IConstArg: AbsWaypointEvent.IConstArg{
			IDeactivateGliderWaypointEventAdaptor adaptor{get;}
		}
		public new class ConstArg: AbsWaypointEvent.ConstArg, IConstArg{
			public ConstArg(
				float eventPoint,
				IDeactivateGliderWaypointEventAdaptor adaptor
			): base(
				eventPoint
			){
				thisAdaptor = adaptor;
			}
			IDeactivateGliderWaypointEventAdaptor thisAdaptor;
			public IDeactivateGliderWaypointEventAdaptor adaptor{
				get{
					return thisAdaptor;
				}
			}
		}
	}
}

