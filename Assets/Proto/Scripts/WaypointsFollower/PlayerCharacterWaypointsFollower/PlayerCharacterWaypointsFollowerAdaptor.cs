using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IPlayerCharacterWaypointsFollowerAdaptor: IWaypointsFollowerAdaptor{
		IPlayerCharacterWaypointsFollower GetPlayerCharacterWaypointsFollower();
	}
	public class PlayerCharacterWaypointsFollowerAdaptor : WaypointsFollowerAdaptor, IPlayerCharacterWaypointsFollowerAdaptor {

		protected override IWaypointsFollower CreateFollower(){
			PlayerCharacterWaypointsFollower.IConstArg arg = new PlayerCharacterWaypointsFollower.ConstArg(
				this,
				followSpeed,
				processOrder
			);
			return new PlayerCharacterWaypointsFollower(arg);
		}
		public IPlayerCharacterWaypointsFollower GetPlayerCharacterWaypointsFollower(){
			return (IPlayerCharacterWaypointsFollower)thisFollower;
		}
		
	}
}
