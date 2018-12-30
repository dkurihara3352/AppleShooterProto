using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{
	public interface IArrowTrailReserve: ISceneObjectReserve<IArrowTrail>{
		void ActivateTrailAt(
			IArrow arrow,
			float normalizedDraw
		);
	}
	public class ArrowTrailReserve : AbsSceneObjectReserve<IArrowTrail>, IArrowTrailReserve {
		public ArrowTrailReserve(
			IConstArg arg
		): base(arg){

		}
		IArrowTrailReserveAdaptor thisArrowTrailReserveAdaptor{
			get{
				return (IArrowTrailReserveAdaptor)thisAdaptor;
			}
		}
		public void ActivateTrailAt(
			IArrow arrow,
			float normalizedDraw
		){
			IArrowTrail next = GetNext();
			next.ActivateAt(arrow);

			Color trailColor = Color.Lerp(
				thisMinDrawColor,
				thisMaxDrawColor,
				normalizedDraw
			);
			next.SetColor(trailColor);
		}
		Color thisMinDrawColor{
			get{
				return thisArrowTrailReserveAdaptor.GetMinDrawColor();
			}
		}
		Color thisMaxDrawColor{
			get{
				return thisArrowTrailReserveAdaptor.GetMaxDrawColor();
			}
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
