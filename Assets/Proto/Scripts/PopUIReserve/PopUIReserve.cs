using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPopUIReserve{
		void SetPopUIs(IPopUI[] uis);
		RectTransform GetReserveRectTransform();
		IPopUI GetNextPopUI();
		Vector2 GetReservedLocalPosition(int index);
	}
	public class PopUIReserve: IPopUIReserve{
		public PopUIReserve(
			IConstArg arg
		){
			thisAdaptor = arg.adaptor;
		}
		IPopUIReserveAdaptor thisAdaptor;
		IPopUI[] thisPopUIs;

		public void SetPopUIs(IPopUI[] popUIs){
			thisPopUIs = popUIs;
			CacheReservedLocalPositions();
		}
		int nextIndex = 0;
		public IPopUI GetNextPopUI(){
			IPopUI uiAtIndex = thisPopUIs[nextIndex];
			nextIndex ++;
			if(nextIndex >= thisPopUIs.Length - 1)
				nextIndex = 0;
			return uiAtIndex;
		}
		float reservedUISpace = 50f;
		public Vector2 GetReservedLocalPosition(int index){
			return thisCachedReservedLocalPositions[index];
		}
		Vector2[] thisCachedReservedLocalPositions;
		void CacheReservedLocalPositions(){
			List<Vector2> resultList = new List<Vector2>();
			float sumOfWidth = 0f;
			foreach(IPopUI popUI in thisPopUIs){
				float rectWidth = popUI.GetRectSize()[0];
				float posX = sumOfWidth;

				Vector2 position = new Vector2(
					posX,
					0f
				);
				resultList.Add(position);
				sumOfWidth += rectWidth + reservedUISpace;
			}
			thisCachedReservedLocalPositions = resultList.ToArray();
		}
		public RectTransform GetReserveRectTransform(){
			return thisAdaptor.GetRectTransform();
		}
		/* Const */
			public interface IConstArg{
				IPopUIReserveAdaptor adaptor{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					IPopUIReserveAdaptor adaptor
				){
					thisAdaptor = adaptor;
				}
				readonly IPopUIReserveAdaptor thisAdaptor;
				public IPopUIReserveAdaptor adaptor{
					get{
						return thisAdaptor;
					}
				}
			}
		/*  */
	}
}


