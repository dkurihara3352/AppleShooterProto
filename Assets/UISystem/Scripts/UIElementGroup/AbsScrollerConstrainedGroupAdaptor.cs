using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IScrollerConstrainedGroupAdaptor: IUIElementGroupAdaptor{}
	public abstract class AbsScrollerConstrainedGroupAdaptor : AbsUIElementGroupAdaptor, IScrollerConstrainedGroupAdaptor{
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
		string GetUIAName(IUIAdaptor uia){
			if(uia == null)
				return "null";
			else
				return uia.GetName();
		}
	
	}
}
