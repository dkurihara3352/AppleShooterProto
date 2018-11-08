using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IStaticTargetSpawnPointGroup: ISceneObject{
		IStaticTargetSpawnPoint[] GetShootingTargetSpawnPoints();
		void SetShootingTargetSpawnPoints(IStaticTargetSpawnPoint[] points);
		IStaticTargetSpawnPoint Draw();
		void Log();
		void ClearLog();
	}
	public class StaticTargetSpawnPointGroup : AbsSceneObject, IStaticTargetSpawnPointGroup {
		
		public StaticTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		IStaticTargetSpawnPoint[] thisSpawnPoints;
		public void SetShootingTargetSpawnPoints(
			IStaticTargetSpawnPoint[] spawnPoints
		){
			thisSpawnPoints = spawnPoints;
			float[] relativeProbability = CreateRelativeProbArray();
			thisIndexPool = new Pool(relativeProbability);
		}
		public IStaticTargetSpawnPoint[] GetShootingTargetSpawnPoints(){
			return thisSpawnPoints;
		}

		IPool thisIndexPool;
		float[] CreateRelativeProbArray(){
			int count = thisSpawnPoints.Length;
			float[] resultArray = new float[count];
			for(int i = 0; i < count; i ++){
				resultArray[i] = 1f;
			}
			return resultArray;
		}
		public IStaticTargetSpawnPoint Draw(){
			int index = thisIndexPool.Draw();
			return thisSpawnPoints[index];
		}
		public void Log(){
			thisIndexPool.Log();
		}
		public void ClearLog(){
			thisIndexPool.ClearLog();
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
