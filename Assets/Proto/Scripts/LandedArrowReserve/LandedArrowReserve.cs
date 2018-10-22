using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowReserve{
		void Reserve(ILandedArrow arrow);
		ILandedArrow Unreserve();
		void SetLandedArrows(ILandedArrow[] arrows);

		ILandedArrow[] GetLandedArrows();
	}
	public class LandedArrowReserve : ILandedArrowReserve {
		public LandedArrowReserve(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		ILandedArrowReserveAdaptor thisAdaptor;

		public void Reserve(ILandedArrow arrow){
			arrow.SetParent(thisAdaptor.GetTransform());
			PutArrowInArray(arrow);
			// arrow.ResetLocalTransform();
		}
		float arrowSpace = 1f;
		void PutArrowInArray(ILandedArrow arrow){
			arrow.ResetLocalTransform();
			int index = arrow.GetIndex();
			float posX = index * arrowSpace;
			Vector3 offset = new Vector3(posX, 0f, 0f);
			Vector3 newWorPos = arrow.GetPosition() + offset;
			arrow.SetPosition(newWorPos);
		}
		ILandedArrow[] thisLandedArrows;
		public ILandedArrow[] GetLandedArrows(){
			return thisLandedArrows;
		}
		int nextAvailableLandedArrowIndex = 0;
		public void SetLandedArrows(ILandedArrow[] arrows){
			thisLandedArrows = arrows;
		}
		public ILandedArrow Unreserve(){
			ILandedArrow result = thisLandedArrows[nextAvailableLandedArrowIndex ++];
			if(nextAvailableLandedArrowIndex >= thisLandedArrows.Length - 1)
				nextAvailableLandedArrowIndex = 0;
			return result;
		}
		/* Const */
			public interface IConstArg{
				ILandedArrowReserveAdaptor adaptor{get;}
			}	
			public struct ConstArg: IConstArg{
				public ConstArg(
					ILandedArrowReserveAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly ILandedArrowReserveAdaptor thisAdaptor;
				public ILandedArrowReserveAdaptor adaptor{get{return thisAdaptor;}}
			}
		/*  */
	}
}
