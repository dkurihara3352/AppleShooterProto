using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPlayerCharacterWaypointsFollower: IWaypointsFollower{
		
		void StartExecutingSpawnEvents();
		void StopExecutingSpawnEvents();
		bool ShouldSpawnTargets();
	}
	public class PlayerCharacterWaypointsFollower : WaypointsFollower, IPlayerCharacterWaypointsFollower {
		public PlayerCharacterWaypointsFollower(IConstArg arg): base(arg){}
		public void StartExecutingSpawnEvents(){
			thisShouldSpawnTargets = true;
		}
		public void StopExecutingSpawnEvents(){
			thisShouldSpawnTargets = false;
		}
		bool thisShouldSpawnTargets = false;
		public bool ShouldSpawnTargets(){
			return thisShouldSpawnTargets;
		}

		public new interface IConstArg: WaypointsFollower.IConstArg{}
		public new class ConstArg: WaypointsFollower.ConstArg, IConstArg{
			public ConstArg(
				IPlayerCharacterWaypointsFollowerAdaptor adaptor,
				float followSpeed,
				int processOrder
			): base(
				adaptor,
				followSpeed,
				processOrder
			){}
		}
	}
}
