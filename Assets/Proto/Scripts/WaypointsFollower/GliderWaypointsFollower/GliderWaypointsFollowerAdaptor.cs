using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGliderWaypointsFollowerAdaptor: IWaypointsFollowerAdaptor{
		IGliderWaypointsFollower GetGliderWaypointsFollower();
	}
	public class GliderWaypointsFollowerAdaptor: WaypointsFollowerAdaptor, IGliderWaypointsFollowerAdaptor{
		protected override IWaypointsFollower CreateFollower(){
			GliderWaypointsFollower.IConstArg arg = new GliderWaypointsFollower.ConstArg(
				this,
				followSpeed,
				processOrder
			);
			return new GliderWaypointsFollower(arg);
		}
		IGliderWaypointsFollower thisGliderWaypointsFollower{
			get{
				return (IGliderWaypointsFollower)thisFollower;
			}
		}
		public IGliderWaypointsFollower GetGliderWaypointsFollower(){
			return thisGliderWaypointsFollower;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IGlidingTarget glider =  gliderAdaptor.GetGlidingTarget();
			thisGliderWaypointsFollower.SetGlider(glider);
		}
		public GlidingTargetAdaptor gliderAdaptor;
	}
}

