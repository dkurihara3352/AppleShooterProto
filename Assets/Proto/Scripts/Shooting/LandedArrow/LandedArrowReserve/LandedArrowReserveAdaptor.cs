using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface ILandedArrowReserveAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		ILandedArrowReserve GetLandedArrowReserve();
	}
	public class LandedArrowReserveAdaptor: SlickBowShootingMonoBehaviourAdaptor, ILandedArrowReserveAdaptor{
		ILandedArrowReserve thisReserve;
		public ILandedArrowReserve GetLandedArrowReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			thisReserve = CreateLandedArrowReserve();
			thisLandedArrowAdaptors = CreateLandedArrowAdaptors();
		}
		ILandedArrowReserve CreateLandedArrowReserve(){
			LandedArrowReserve.IConstArg arg = new LandedArrowReserve.ConstArg(
				this
			);
			return new LandedArrowReserve(arg);
		}
		public override void SetUpReference(){
			ILandedArrow[] landedArrows = CreateLandedArrows();
			thisReserve.SetSceneObjects(landedArrows);
		}
		public int totalLandedArrowsCount;
		public GameObject landedArrowPrefab;
		ILandedArrowAdaptor[] thisLandedArrowAdaptors;
		IArrowTwangAdaptor[] thisArrowTwangAdaptors;
		public ParticleSystem hitParticle;
		ILandedArrowAdaptor[] CreateLandedArrowAdaptors(){

			List<ILandedArrowAdaptor> resultList = new List<ILandedArrowAdaptor>();

			for(int i = 0; i < totalLandedArrowsCount; i ++){
				GameObject landedArrowGO = GameObject.Instantiate(
					landedArrowPrefab,
					Vector3.zero,
					Quaternion.identity
				);
				ILandedArrowAdaptor arrowAdaptor = (ILandedArrowAdaptor)landedArrowGO.GetComponent(typeof(ILandedArrowAdaptor));
				if(arrowAdaptor == null)
					throw new System.InvalidOperationException(
						"landedArrowAdaptor missing"
					);
				arrowAdaptor.SetIndex(i);
				arrowAdaptor.SetLandedArrowReserveAdaptor(this);

				arrowAdaptor.SetUp();
				resultList.Add(arrowAdaptor);
			}
			return resultList.ToArray();
		}
		ILandedArrow[] CreateLandedArrows(){
			List<ILandedArrow> resultList = new List<ILandedArrow>();
			foreach(ILandedArrowAdaptor adaptor in thisLandedArrowAdaptors)
				resultList.Add(adaptor.GetLandedArrow());
			return resultList.ToArray();
		}
	}
}


