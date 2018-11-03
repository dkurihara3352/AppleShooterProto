using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMarkerUIReserve: ISceneObjectReserve<IMarkerUI>{
		void ActivateMarkerUIAt(
			ISceneObject sceneObj
		);
	}
	public class MarkerUIReserve: AbsSceneObjectReserve<IMarkerUI>, IMarkerUIReserve{
		public MarkerUIReserve(
			IConstArg arg
		): base(
			arg
		){}
		public override void Reserve(IMarkerUI markerUI){
			markerUI.SetParent(this);
			markerUI.ResetLocalTransform();
			Vector2 reservedPosition = GetReservedLocalPosition(markerUI.GetIndex());
			markerUI.SetLocalPosition(reservedPosition);
		}
		float thisSpace = 150;
		Vector2 GetReservedLocalPosition(int index){
			float posX = index * thisSpace;
			return new Vector2(posX, 0f);
		}
		public virtual void ActivateMarkerUIAt(
			ISceneObject sceneObj
		){
			IMarkerUI markerUI = GetNext();
			markerUI.Deactivate();
			markerUI.SetTargetSceneObject(sceneObj);
			markerUI.Activate();
		}
			
		/* ConstArg */
			public new interface IConstArg: AbsSceneObject.IConstArg{

			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IMarkerUIReserveAdaptor adaptor
				): base(
					adaptor
				){
				}
			}
		/*  */
	}
}

