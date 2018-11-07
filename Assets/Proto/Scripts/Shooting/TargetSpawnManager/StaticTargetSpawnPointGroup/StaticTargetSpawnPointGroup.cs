using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticTargetSpawnPointGroup: ISceneObject{
		IStaticTargetSpawnPoint[] GetShootingTargetSpawnPoints();
		void SetShootingTargetSpawnPoints(IStaticTargetSpawnPoint[] points);
	}
	public class ShootingTargetSpawnPointGroup : AbsSceneObject, IStaticTargetSpawnPointGroup {
		
		public ShootingTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		IStaticTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(
			IStaticTargetSpawnPoint[] spawnPoints
		){
			thisSpawnPoints = spawnPoints;
		}
		public IStaticTargetSpawnPoint[] GetShootingTargetSpawnPoints(){
			return thisSpawnPoints;
		}

		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{

		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IStaticTargetSpawnPointGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}

}
