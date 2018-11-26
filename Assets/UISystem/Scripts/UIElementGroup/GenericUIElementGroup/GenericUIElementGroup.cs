using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IGenericUIElementGroup: IUIElementGroup{}
	public class GenericUIElementGroup: AbsUIElementGroup<IUIElement>, IGenericUIElementGroup{
		public GenericUIElementGroup(
			IConstArg arg
		): base(
			arg
		){}

		public new interface IConstArg: AbsUIElementGroup<IUIElement>.IConstArg{}
		public new class ConstArg: AbsUIElementGroup<IUIElement>.ConstArg, IConstArg{
			public ConstArg(
				int columnCountConstraint,
				int rowCountConstraint,
				bool topToBottom,
				bool leftToRight,
				bool rowToColumn,
				IUIElementGroupAdaptor adaptor,
				ActivationMode activationMode
			): base(
				columnCountConstraint,
				rowCountConstraint,
				topToBottom,
				leftToRight,
				rowToColumn,
				adaptor,
				activationMode
			){
			}
		}

	}
}
