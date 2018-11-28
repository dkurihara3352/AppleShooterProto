using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ISceneObjectElementGroupAdaptor: IUIElementGroupAdaptor{}
	public class SceneObjectElementGroupAdaptor: BaseGridGroupAdaptor, ISceneObjectElementGroupAdaptor{
		protected override IUIElement[] GetGroupElements(){
			return GetChildUIElements();
		}
		protected override IUIElement CreateUIElement(){
			GenericUIElementGroup.IConstArg arg = new GenericUIElementGroup.ConstArg(
				columnCountConstraint,
				rowCountConstraint,
				topToBottom,
				leftToRight,
				rowToColumn,
				this,
				activationMode
			);
			return new GenericUIElementGroup(arg);
		}
	}
}

