using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IGlidingTargetAdaptor: IShootingTargetAdaptor{
		void SetGlidingTargetReserve(IGlidingTargetReserve reserve);
		IGlidingTarget GetGlidingTarget();
	}
	public class GlidingTargetAdaptor: AbsShootingTargetAdaptor, IGlidingTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
			thisWaypointsFollowerAdaptor = CollectWaypointsFollowerAdaptor();
			thisWaypointsFollowerAdaptor.SetUp();
		}
		protected override IShootingTarget CreateShootingTarget(){
			GlidingTarget.IConstArg arg = new GlidingTarget.ConstArg(
				thisIndex,
				thisHealthBellCurve,
				this,
				targetData
			);
			return new GlidingTarget(arg);
		}
		IGlidingTarget thisGlidingTarget{
			get{
				return (IGlidingTarget)thisShootingTarget;
			}
		}
		public IGlidingTarget GetGlidingTarget(){
			return thisGlidingTarget;
		}
		IWaypointsFollowerAdaptor thisWaypointsFollowerAdaptor;
		IWaypointsFollowerAdaptor CollectWaypointsFollowerAdaptor(){
			IWaypointsFollowerAdaptor adaptor = (IWaypointsFollowerAdaptor)this.transform.GetComponent(typeof(IWaypointsFollowerAdaptor));
			if(adaptor == null)
				throw new System.InvalidOperationException(
					"IWaypointsFollowerAdaptor is missing on GlidingTargetAdaptor"
				);
			return adaptor;
		}
		IGlidingTargetReserve thisGlidingTargetReserve;
		public void SetGlidingTargetReserve(
			IGlidingTargetReserve reserve
		){
			thisGlidingTargetReserve = reserve;
		}
		public override void SetUpReference(){
			base.SetUpReference();

			IWaypointsFollower follower = thisWaypointsFollowerAdaptor.GetWaypointsFollower();
			thisGlidingTarget.SetWaypointsFollower(follower);
			
			thisGlidingTarget.SetGlidingTargetReserve(
				thisGlidingTargetReserve
			);
		}
	}
}

