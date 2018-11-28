using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IBaseGridGroupAdaptor: IUIElementGroupAdaptor{}
	public abstract class BaseGridGroupAdaptor : AbsUIElementGroupAdaptor, IBaseGridGroupAdaptor{
		public Vector2 elementToPaddingRatio = new Vector2(10f, 10f);
		protected override void MakeSureConstraintIsProperlySet(){}
		protected override IRectCalculationData CreateRectCalculationData(
			IUIElement[] groupElements
		){
			IUIAdaptor parentUIAdaptor = thisParentUIAdaptor;
			return new ScrollerConstraintRectCalculationData(
				elementToPaddingRatio,
				parentUIAdaptor,
				GetRectSize()
			);
		}
	}
}
