// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace UISystem{
// 	public interface IGenericUIElementGroupAdaptor: IUIElementGroupAdaptor{}
// 	public class GenericUIElementGroupAdaptor: AbsUIElementGroupAdaptor, IGenericUIElementGroupAdaptor{
// 		protected override IUIElement CreateUIElement(){
// 			GenericUIElementGroup.IConstArg arg = new GenericUIElementGroup.ConstArg(
// 				columnCountConstraint,
// 				rowCountConstraint,
// 				topToBottom,
// 				leftToRight,
// 				rowToColumn,
// 				this,
// 				activationMode
// 			);
// 			return new GenericUIElementGroup(arg);
// 		}
// 		public Vector2 elementToPaddingRatio;
// 		public Vector2 groupLengthAsNonScrollerElement;
// 		protected override IRectCalculationData CreateRectCalculationData(IUIElement[] groupElements){
// 			return new ScrollerConstraintRectCalculationData(
// 				elementToPaddingRatio,
// 				thisParentUIAdaptor,
// 				groupLengthAsNonScrollerElement
// 			);
// 		}
// 		protected override void MakeSureConstraintIsProperlySet(){
// 		}
// 	}
// }
