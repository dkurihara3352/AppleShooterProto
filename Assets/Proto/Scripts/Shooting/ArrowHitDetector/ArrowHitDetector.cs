using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowHitDetector: IAppleShooterSceneObject{
		void Hit(IArrow arrow);
		void AddLandedArrow(ILandedArrow arrow);
		void RemoveLandedArrow(ILandedArrow arrow);
		void DeactivateAllLandedArrows();
		ILandedArrow[] GetLandedArrows();
		void ToggleCollider(bool toggle);
		bool ShouldSpawnLandedArrow();
	}
	public abstract class ArrowHitDetector : AppleShooterSceneObject, IArrowHitDetector {
		public ArrowHitDetector(IConstArg arg): base(arg){}
		IArrowHitDetectorAdaptor thisArrowHitDetectorAdaptor{
			get{
				return (IArrowHitDetectorAdaptor)thisAdaptor;
			}
		}
		public abstract void Hit(IArrow arrow);
		ILandedArrow[] thisLandedArrows = new ILandedArrow[]{};
		public ILandedArrow[] GetLandedArrows(){
			return thisLandedArrows;
		}
		public void AddLandedArrow(ILandedArrow arrow){
			List<ILandedArrow> resultList = new List<ILandedArrow>(thisLandedArrows);
			resultList.Add(arrow);
			thisLandedArrows = resultList.ToArray();
		}
		public void RemoveLandedArrow(ILandedArrow arrow){
			List<ILandedArrow> listToReduce = new List<ILandedArrow>(thisLandedArrows);
			if(listToReduce.Contains(arrow)){
				listToReduce.Remove(arrow);
				thisLandedArrows = listToReduce.ToArray();
			}
		}
		public void DeactivateAllLandedArrows(){
			List<ILandedArrow> temp = new List<ILandedArrow>(thisLandedArrows);
			foreach(ILandedArrow arrow in temp)
				if(arrow != null)
					arrow.Deactivate();
		}
		public void ToggleCollider(bool toggle){
			thisArrowHitDetectorAdaptor.ToggleCollider(toggle);
		}
		public virtual bool ShouldSpawnLandedArrow(){
			return true;
		}
		public new interface IConstArg: AppleShooterSceneObject.IConstArg{}
		public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IArrowHitDetectorAdaptor adaptor
			): base(adaptor){}
		}
	}
}
