﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlickBowShooting{
	public interface IStaticTargetSpawnPointAdaptor: IShootingTargetSpawnPointAdaptor{
		IStaticTargetSpawnPoint GetStaticTargetSpawnPoint();
	}
	public class StaticTargetSpawnPointAdaptor : AbsShootingTargetSpawnPointAdaptor, IStaticTargetSpawnPointAdaptor {

		public override void SetUp(){
			thisSpawnPoint = CreateSpawnPoint();
			// DisableAllMeshRenderer();
		}
		IStaticTargetSpawnPoint CreateSpawnPoint(){
			StaticTargetSpawnPoint.IConstArg arg = new StaticTargetSpawnPoint.ConstArg(
				eventPoint,
				this
			);
			return new StaticTargetSpawnPoint(arg);
		}
		public IStaticTargetSpawnPoint GetStaticTargetSpawnPoint(){
			return (IStaticTargetSpawnPoint)thisSpawnPoint;
		}
	}
}
