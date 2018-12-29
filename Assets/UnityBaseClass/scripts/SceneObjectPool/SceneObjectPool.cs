using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ISceneObjectPool<T>: ISceneObject where T: IMonoBehaviourAdaptor{
		void SetUpProbabilityTable(
			Dictionary<T, float> relativeProbabilityTable
		);
		T Draw();
		void Log();
		void ClearLog();
	}
	public class SceneObjectPool<T> : AbsSceneObject, ISceneObjectPool<T> where T: IMonoBehaviourAdaptor {
		public SceneObjectPool(
			IConstArg<T> arg
		): base(
			arg
		){
		}
		public void SetUpProbabilityTable(Dictionary<T, float> relativeProbabilityTable){

			thisSceneObjects = new T[relativeProbabilityTable.Count];
			float[] relativeProbabilities = new float[relativeProbabilityTable.Count];
			int index = 0;
			foreach(KeyValuePair<T, float> pair in relativeProbabilityTable){
				thisSceneObjects[index] = pair.Key;
				relativeProbabilities[index] = pair.Value;
				index ++;
			}
			thisPool = new Pool(relativeProbabilities);
			
		}
		IPool thisPool;
		T[] thisSceneObjects;
		public T Draw(){
			int index = thisPool.Draw();
			return thisSceneObjects[index];
		}
		public void Log(){
			thisPool.Log();
		}
		public void ClearLog(){
			thisPool.ClearLog();
		}
		/*  */
		public interface IConstArg<U>: AbsSceneObject.IConstArg where U: IMonoBehaviourAdaptor{
		}
		public class ConstArg<U>: AbsSceneObject.ConstArg, IConstArg<U> where U: IMonoBehaviourAdaptor{
			public ConstArg(
				ISceneObjectPoolAdaptor<U> adaptor
			): base(adaptor){
			}
		}
	}
}
