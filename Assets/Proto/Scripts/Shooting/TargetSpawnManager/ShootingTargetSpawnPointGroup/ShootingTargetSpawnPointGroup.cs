using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointGroup: ISceneObject{
		IShootingTargetSpawnPoint[] GetShootingTargetSpawnPoints();
		void SetShootingTargetSpawnPoints(IShootingTargetSpawnPoint[] points);
	}
	public class ShootingTargetSpawnPointGroup : AbsSceneObject, IShootingTargetSpawnPointGroup {
		
		public ShootingTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		IShootingTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(
			IShootingTargetSpawnPoint[] spawnPoints
		){
			thisSpawnPoints = spawnPoints;
		}
		public IShootingTargetSpawnPoint[] GetShootingTargetSpawnPoints(){
			return thisSpawnPoints;
		}

		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{

		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingTargetSpawnPointGroupAdaptor adaptor
			): base(
				adaptor
			){}
		}
	}

}
