using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlickBowShooting{
	public interface IShootingTargetSpawnPointGroup: ISlickBowShootingSceneObject{
		void SetSpawnPoints(IShootingTargetSpawnPoint[] spawnPoints);
		IShootingTargetSpawnPoint Draw();
		void Log();
		void ClearLog();
	}	
	public abstract class AbsShootingTargetSpawnPointGroup : SlickBowShootingSceneObject, IShootingTargetSpawnPointGroup {
		public AbsShootingTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		IShootingTargetSpawnPointGroupAdaptor thisTypedAdaptor{
			get{
				return (IShootingTargetSpawnPointGroupAdaptor)thisAdaptor;
			}
		}
		// IShootingTargetSpawnPoint[] _spawnPoints;
		IShootingTargetSpawnPoint[] thisSpawnPoints;
		public void SetSpawnPoints(IShootingTargetSpawnPoint[] spawnPoints){
			thisSpawnPoints = spawnPoints;
			float[] relativeProb = CreateRelativeProb(spawnPoints.Length);
			thisIndexPool = new UnityBase.Pool(relativeProb);
		}
		float[] CreateRelativeProb(int count){
			float[] result = new float[count];
			for(int i = 0; i < count; i ++){
				result[i] = 1f;
			}
			return result;
		}
		UnityBase.IPool thisIndexPool;
		public IShootingTargetSpawnPoint Draw(){
			int index = thisIndexPool.Draw();
			return thisSpawnPoints[index];
		}
		public void ClearLog(){
			thisIndexPool.ClearLog();
		}
		public void Log(){
			thisIndexPool.Log();
		}
		/*  */
		public new interface IConstArg: SlickBowShootingSceneObject.IConstArg{

		}
		public new class ConstArg: SlickBowShootingSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingTargetSpawnPointGroupAdaptor adaptor
			): base(
				adaptor
			){

			}
		}
		
	}
}
