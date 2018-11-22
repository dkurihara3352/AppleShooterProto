using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ISceneObjectReserve<T>: ISceneObject where T: ISceneObject{
		void SetSceneObjects(T[] sceneObjects);
		void Reserve(T sceneObj);
	}
	public abstract class AbsSceneObjectReserve<T>: AbsSceneObject, ISceneObjectReserve<T> where T: ISceneObject{
		public AbsSceneObjectReserve(
			IConstArg arg
		): base(
			arg
		){

		}
		public virtual void SetSceneObjects(T[] sceneObjects){
			thisSceneObjects = sceneObjects;
		}
		protected T[] thisSceneObjects;
		int nextIndex = 0;
		protected T thisCurrent;
		protected T GetNext(){
			T next = thisSceneObjects[nextIndex];
			nextIndex ++;
			if(nextIndex >= thisSceneObjects.Length)
				nextIndex = 0;
			thisCurrent = next;
			return next;
		}
		public abstract void Reserve(T sceneObject);
	}
}

