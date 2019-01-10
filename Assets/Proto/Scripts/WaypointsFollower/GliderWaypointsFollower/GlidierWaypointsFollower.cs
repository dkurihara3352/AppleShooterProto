using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGliderWaypointsFollower: IWaypointsFollower{
		void SetGlider(IGlidingTarget target);
		IGlidingTarget GetGlider();
	}
	public class GliderWaypointsFollower: WaypointsFollower, IGliderWaypointsFollower{
		public GliderWaypointsFollower(IConstArg arg): base(arg){}
		IGlidingTarget thisGlider;
		public void SetGlider(IGlidingTarget target){
			thisGlider = target;
		}
		public IGlidingTarget GetGlider(){
			return thisGlider;
		}
	}
}

