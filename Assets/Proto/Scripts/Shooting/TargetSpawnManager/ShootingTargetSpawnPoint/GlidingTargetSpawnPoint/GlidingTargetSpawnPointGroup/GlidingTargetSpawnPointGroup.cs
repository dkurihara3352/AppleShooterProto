﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetSpawnPointGroup: IShootingTargetSpawnPointGroup{
		IGlidingTargetSpawnPoint[] GetGlidingTargetSpawnPoints();
	}
	public class GlidingTargetSpawnPointGroup : AbsShootingTargetSpawnPointGroup, IGlidingTargetSpawnPointGroup {
		public GlidingTargetSpawnPointGroup(IConstArg arg): base(arg){}
		public IGlidingTargetSpawnPoint[] GetGlidingTargetSpawnPoints(){
			return GetTypedSpawnPoints<IGlidingTargetSpawnPoint>();
		}
		/*  */
		public new interface IConstArg: AbsShootingTargetSpawnPointGroup.IConstArg{}
		public new class ConstArg: AbsShootingTargetSpawnPointGroup.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetSpawnPointGroupAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
