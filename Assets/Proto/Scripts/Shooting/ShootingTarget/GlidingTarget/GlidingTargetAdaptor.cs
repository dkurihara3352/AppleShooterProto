using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetAdaptor: ITestShootingTargetAdaptor{}
	public class GlidingTargetAdaptor: TestShootingTargetAdaptor, IGlidingTargetAdaptor{
		void MakeSureParentHasWaypointsManagerAdaptor(){
			Transform parent = this.transform.parent;
			if(parent == null)
				throw new System.InvalidOperationException(
					"parent is missing on GlidingTargetAdaptor"
				);
			IWaypointsManagerAdaptor waypointsManagerAdaptor = (IWaypointsManagerAdaptor)parent.GetComponent(typeof(IWaypointsManagerAdaptor));
			if(waypointsManagerAdaptor == null)
				throw new System.InvalidOperationException(
					"GlidingTargetAdaptor must be child of its WaypointsManagerAdaptor"
				);
		}
		public override void SetUp(){
			base.SetUp();
			MakeSureParentHasWaypointsManagerAdaptor();
		}
		protected override void SetUpTarget(){
			TestShootingTarget.IConstArg arg = new TestShootingTarget.ConstArg(
				health,
				this,
				defaultColor,
				processFactory,
				fadeTime
			);
			thisShootingTarget = new GlidingTarget(arg);
		}
		public override void SetUpReference(){
			base.SetUpReference();
			SetGlidingTargetWithWaypointsFollower();
		}
		void SetGlidingTargetWithWaypointsFollower(){
			IWaypointsFollowerAdaptor adaptor = (IWaypointsFollowerAdaptor)this.transform.GetComponent(typeof(IWaypointsFollowerAdaptor));
			if(adaptor == null)
				throw new System.InvalidOperationException(
					"IWaypointsFollowerAdaptor is missing on GlidingTargetAdaptor"
				);
			IWaypointsFollower follower = adaptor.GetWaypointsFollower();
			((IGlidingTarget)thisShootingTarget).SetWaypointsFollower(follower);
		}
	}
}

