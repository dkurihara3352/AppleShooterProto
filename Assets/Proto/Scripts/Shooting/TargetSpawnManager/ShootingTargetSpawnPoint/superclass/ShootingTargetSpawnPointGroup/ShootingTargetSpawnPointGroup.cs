using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IShootingTargetSpawnPointGroup: ISceneObject{
		IShootingTargetSpawnPoint[] GetSpawnPoints();
		void SetSpawnPoints(IShootingTargetSpawnPoint[] spawnPoints);
		IShootingTargetSpawnPoint Draw();
		void Log();
		void ClearLog();
	}	
	public abstract class AbsShootingTargetSpawnPointGroup : AbsSceneObject, IShootingTargetSpawnPointGroup {
		public AbsShootingTargetSpawnPointGroup(
			IConstArg arg
		): base(
			arg
		){}
		protected IShootingTargetSpawnPoint[] thisSpawnPoints{
			get{
				if(_spawnPoints == null)
					_spawnPoints = thisTypedAdaptor.GetSpawnPoints();
				return _spawnPoints;
			}
			set{
				_spawnPoints = value;
			}
		}
		IShootingTargetSpawnPointGroupAdaptor thisTypedAdaptor{
			get{
				return (IShootingTargetSpawnPointGroupAdaptor)thisAdaptor;
			}
		}
		IShootingTargetSpawnPoint[] _spawnPoints;
		public void SetSpawnPoints(IShootingTargetSpawnPoint[] spawnPoints){
			thisSpawnPoints = spawnPoints;
			float[] relativeProb = CreateRelativeProb(spawnPoints.Length);
			thisIndexPool = new Pool(relativeProb);
		}
		public IShootingTargetSpawnPoint[] GetSpawnPoints(){
			return thisSpawnPoints;
		}
		float[] CreateRelativeProb(int count){
			float[] result = new float[count];
			for(int i = 0; i < count; i ++){
				result[i] = 1f;
			}
			return result;
		}
		IPool thisIndexPool;
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
		protected T[] GetTypedSpawnPoints<T>() where T: class, IShootingTargetSpawnPoint{
			List<T> resultList = new List<T>();
			foreach(IShootingTargetSpawnPoint point in thisSpawnPoints)
				resultList.Add((T)point);
			return resultList.ToArray();
		}
		/*  */
		public new interface IConstArg: AbsSceneObject.IConstArg{

		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IShootingTargetSpawnPointGroupAdaptor adaptor
			): base(
				adaptor
			){

			}
		}
		
	}
}
