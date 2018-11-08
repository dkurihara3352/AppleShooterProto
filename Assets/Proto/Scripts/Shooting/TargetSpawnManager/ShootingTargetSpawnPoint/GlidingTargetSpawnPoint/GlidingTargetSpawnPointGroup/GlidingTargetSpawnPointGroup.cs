using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IGlidingTargetSpawnPointGroup: ISceneObject{
		void SetSpawnPoints(IGlidingTargetSpawnPoint[] spawnPoints);
		IGlidingTargetSpawnPoint[] GetSpawnPoints();
		IGlidingTargetSpawnPoint Draw();
		void Log();
		void ClearLog();
	}
	public class GlidingTargetSpawnPointGroup : AbsSceneObject, IGlidingTargetSpawnPointGroup {
		public GlidingTargetSpawnPointGroup(IConstArg arg): base(arg){}
		IGlidingTargetSpawnPoint[] thisSpawnPoints;
		public void SetSpawnPoints(IGlidingTargetSpawnPoint[] spawnPoints){
			thisSpawnPoints = spawnPoints;
			thisIndexPool = CreatePool();
		}
		public IGlidingTargetSpawnPoint[] GetSpawnPoints(){
			return thisSpawnPoints;
		}
		IPool thisIndexPool;
		public IGlidingTargetSpawnPoint Draw(){
			int index = thisIndexPool.Draw();
			return thisSpawnPoints[index];
		}
		IPool CreatePool(){
			float[] relativeProb = new float[thisSpawnPoints.Length];
			int index = 0;
			foreach(float prob in relativeProb)
				relativeProb[index ++] = 1f;
			
			return new Pool(relativeProb);
		}
		public void Log(){
			thisIndexPool.Log();
		}
		public void ClearLog(){
			thisIndexPool.ClearLog();
		}
		
		public new interface IConstArg: AbsSceneObject.IConstArg{}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IGlidingTargetSpawnPointGroupAdaptor adaptor
			): base(adaptor){

			}
		}
	}
}
