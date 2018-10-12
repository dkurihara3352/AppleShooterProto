using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowReserveAdaptor: IMonoBehaviourAdaptor{
		ILandedArrowReserve GetLandedArrowReserve();
	}
	public class LandedArrowReserveAdaptor: MonoBehaviourAdaptor, ILandedArrowReserveAdaptor{
		ILandedArrowReserve thisReserve;
		public ILandedArrowReserve GetLandedArrowReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			LandedArrowReserve.IConstArg arg = new LandedArrowReserve.ConstArg(
				this
			);
			thisReserve = new LandedArrowReserve(arg);
		}
		public override void SetUpReference(){
			ILandedArrow[] landedArrows = CreateLandedArrows();
			thisReserve.SetLandedArrows(landedArrows);
		}
		public int totalLandedArrowsCount;
		public GameObject landedArrowPrefab;
		ILandedArrow[] CreateLandedArrows(){
			List<ILandedArrow> resultList = new List<ILandedArrow>();
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
				arrowAdaptor.SetLandedArrowReserveAdaptor(this);
				arrowAdaptor.SetUp();
				arrowAdaptor.SetUpReference();

				ILandedArrow landedArrow = arrowAdaptor.GetLandedArrow();
				landedArrow.SetIndex(i);
				landedArrow.Reserve();

				resultList.Add(landedArrow);
			}
			return resultList.ToArray();
		}
	}
}


