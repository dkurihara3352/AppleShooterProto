using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IStaticTargetSpawnPoint: IShootingTargetSpawnPoint{
		
	}
	public class StaticTargetSpawnPoint: AbsShootingTargetSpawnPoint, IStaticTargetSpawnPoint {
		public StaticTargetSpawnPoint(
			IConstArg arg
		): base(
			arg
		){
		}
	}
}
