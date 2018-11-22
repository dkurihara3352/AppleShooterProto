using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IArrowReserve: ISceneObjectReserve<IArrow>{
		IArrow[] GetArrows();
		IArrow GetNextArrow();

		int GetIndexInFlight(IArrow arrow);
		int GetIndexInReserve(IArrow arrow);
	}
	public class ArrowReserve: AbsSceneObjectReserve<IArrow>, IArrowReserve{
		public ArrowReserve(
			IConstArg arg
		): base(
			arg
		){}
		
		public override void Reserve(IArrow arrow){
			arrow.SetParent(this);
			arrow.ResetLocalTransform();
			Vector3 reservedPosition = GetReservedPosition(arrow.GetIndex());
			arrow.SetLocalPosition(reservedPosition);
		}
		float reservedSpace = 1f;
		Vector3 GetReservedPosition(int index){
			float posX = reservedSpace * index;
			return new Vector3(posX, 0f, 0f);
		}
		public IArrow[] GetArrows(){
			return thisSceneObjects;
		}
		public IArrow GetNextArrow(){
			return GetNext();
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{

		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IArrowReserveAdaptor adaptor
			): base(
				adaptor
			){}
		}
		public int GetIndexInFlight(IArrow arrow){
			List<IArrow> arrowsInFlight = new List<IArrow>();
			foreach(IArrow arrowInArray in thisSceneObjects){
				if(arrowInArray.IsInFlight())
					arrowsInFlight.Add(arrowInArray);
			}
			if(arrowsInFlight.Contains(arrow))
				return arrowsInFlight.IndexOf(arrow);
			return -1;
		}
		public int GetIndexInReserve(IArrow arrow){
			List<IArrow> arrowsInReserve = new List<IArrow>();
			foreach(IArrow arrowInArray in thisSceneObjects){
				if(arrowInArray.IsInReserve())
					arrowsInReserve.Add(arrowInArray);
			}
			if(arrowsInReserve.Contains(arrow))
				return arrowsInReserve.IndexOf(arrow);
			else
				return -1;
		}
	}
}

