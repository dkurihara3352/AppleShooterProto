using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowTrailReserve: ISceneObjectReserve<IArrowTrail>{
		void ActivateTrailAt(IArrow arrow);
	}
	public class ArrowTrailReserve : AbsSceneObjectReserve<IArrowTrail>, IArrowTrailReserve {
		public ArrowTrailReserve(
			IConstArg arg
		): base(arg){

		}
		public void ActivateTrailAt(IArrow arrow){
			IArrowTrail next = GetNext();
			next.ActivateAt(arrow);
		}
		public override void Reserve(IArrowTrail trail){
			trail.SetParent(this);
			trail.ResetLocalTransform();
		}
		public new interface IConstArg: AbsSceneObject.IConstArg{
		}
		public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
			public ConstArg(
				IArrowTrailReserveAdaptor adaptor
			): base(adaptor){
			}
		}
	}
}
