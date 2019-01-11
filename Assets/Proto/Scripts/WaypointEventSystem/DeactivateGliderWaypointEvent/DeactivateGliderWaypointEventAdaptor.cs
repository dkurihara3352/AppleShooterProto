using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IDeactivateGliderWaypointEventAdaptor: IWaypointEventAdaptor{
		IDeactivateGliderWaypointEvent GetDeactivateGliderWaypointEvent();
		// void SetGlider(IGlidingTarget target);
		// IGlidingTarget GetGlider();
		IGlidingTargetWaypointCurve GetThisGlidingTargetWaypointCurve();
	}
	public class DeactivateGliderWaypointEventAdaptor: SlickBowShootingMonoBehaviourAdaptor, IDeactivateGliderWaypointEventAdaptor{
		public override void SetUp(){
			thisDeactivateGliderWaypointEvent = CreateDeactivateGliderWaypointEvent();
		}
		public override void SetUpReference(){
			thisGlidingTargetWaypointCurve = CollectWaypointCurve();
		}
		IDeactivateGliderWaypointEvent thisDeactivateGliderWaypointEvent;
		public IDeactivateGliderWaypointEvent GetDeactivateGliderWaypointEvent(){
			return thisDeactivateGliderWaypointEvent;
		}
		public IWaypointEvent GetWaypointEvent(){
			return thisDeactivateGliderWaypointEvent;
		}
		IDeactivateGliderWaypointEvent CreateDeactivateGliderWaypointEvent(){
			DeactivateGliderWaypointEvent.IConstArg arg = new DeactivateGliderWaypointEvent.ConstArg(
				eventPoint,
				this
			);
			return new DeactivateGliderWaypointEvent(arg);
		}
		public float eventPoint;
		IGlidingTargetWaypointCurve thisGlidingTargetWaypointCurve;
		IGlidingTargetWaypointCurve CollectWaypointCurve(){
			IGlidingTargetWaypointCurveAdaptor adaptor = (IGlidingTargetWaypointCurveAdaptor)this.transform.GetComponent(typeof(IGlidingTargetWaypointCurveAdaptor));
			return adaptor.GetGlidingTargetWaypointCurve();
		}
		public IGlidingTargetWaypointCurve GetThisGlidingTargetWaypointCurve(){
			return thisGlidingTargetWaypointCurve;
		}
		
		// IGlidingTarget thisGlider;
		// public void SetGlider(IGlidingTarget glider){
		// 	thisGlider = glider;
		// }
		// public IGlidingTarget GetGlider(){
		// 	return thisGlider;
		// }
	}
}


