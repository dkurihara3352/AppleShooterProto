using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ISceneObjectPoolAdaptor<T>: IMonoBehaviourAdaptor where T: ISceneObject{
		ISceneObjectPool<T> GetSceneObjectPool();
	}
	public abstract class SceneObjectPoolAdaptor<T> : MonoBehaviourAdaptor, ISceneObjectPoolAdaptor<T> where T: ISceneObject {
		public override void SetUp(){
			thisPool = CreateSceneObjectPool();
		}
		protected ISceneObjectPool<T> thisPool;
		public ISceneObjectPool<T> GetSceneObjectPool(){
			return  thisPool;
		}
		ISceneObjectPool<T> CreateSceneObjectPool(){

			SceneObjectPool<T>.IConstArg<T> arg = new SceneObjectPool<T>.ConstArg<T>(
				this
			);
			return new SceneObjectPool<T>(arg);
		}
		public override void SetUpReference(){
			Dictionary<T, float> relativeProbabilityTable = CreateRelativeProbabilityTable(
				adaptorRelativeProbPairs
			);
			thisPool.SetUpProbabilityTable(relativeProbabilityTable);
		}
		protected abstract Dictionary<T, float> CreateRelativeProbabilityTable(
			List<AdaptorRelativeProbPair> pairs
		);
		public List<AdaptorRelativeProbPair> adaptorRelativeProbPairs;
	}
	[System.Serializable]
	public struct AdaptorRelativeProbPair{
		public MonoBehaviourAdaptor adaptor;
		public float relativeProb;
	}
}
