using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IPopUIReserve: ISceneObjectReserve<IPopUI>{
		void PopText(
			ISceneObject targetSceneObject,
			string text
		);
	}
	public class PopUIReserve: AbsSceneObjectReserve<IPopUI>, IPopUIReserve{
		public PopUIReserve(
			IConstArg arg
		): base(
			arg
		){
		}
		IPopUIReserveAdaptor thisTypedAdaptor{
			get{
				return (IPopUIReserveAdaptor)thisAdaptor;
			}
		}
		public override void SetSceneObjects(IPopUI[] sceneObjects){
			thisSceneObjects = sceneObjects;
			CacheReservedLocalPositions();
		}
		float reservedUISpace = 50f;
		Vector2 GetReservedLocalPosition(int index){
			return thisCachedReservedLocalPositions[index];
		}
		Vector2[] thisCachedReservedLocalPositions;
		void CacheReservedLocalPositions(){
			List<Vector2> resultList = new List<Vector2>();
			float sumOfWidth = 0f;
			foreach(IPopUI popUI in thisSceneObjects){
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
		public override void Reserve(IPopUI popUI){
			popUI.SetParent(this);
			popUI.ResetLocalTransform();
			Vector3 reservedLocalPosition = GetReservedLocalPosition(popUI.GetIndex());
			popUI.SetLocalPosition(reservedLocalPosition);
		}
		public void PopText(
			ISceneObject targetSceneObject,
			string text
		){
			IPopUI popUI = GetNext();

			popUI.SetTargetSceneObject(targetSceneObject);
			popUI.SetText(text);

			popUI.Activate();
		}
		/* Const */
			public new interface IConstArg: AbsSceneObject.IConstArg{
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IPopUIReserveAdaptor adaptor
				): base(
					adaptor
				){
				}
			}
		/*  */
	}
}


