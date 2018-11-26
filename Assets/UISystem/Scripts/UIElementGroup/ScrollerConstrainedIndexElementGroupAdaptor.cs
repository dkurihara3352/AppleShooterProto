using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IScrollerConstrainedIndexElementGroupAdaptor: IUIElementGroupAdaptor{}
	public class ScrollerConstrainedIndexElementGroupAdaptor : AbsScrollerConstrainedGroupAdaptor, IScrollerConstrainedIndexElementGroupAdaptor {
		public int groupElementCount;
		public GameObject indexElementPrefab;
		protected override IUIElement[] GetGroupElements(){
			return CreateUIEs();
		}
		IUIElement[] CreateUIEs(){
			List<IUIElement> resultList = new List<IUIElement>();
			for(int i = 0; i < groupElementCount; i++ ){
				GameObject go = GameObject.Instantiate(indexElementPrefab, this.GetTransform());
				IIndexElementAdaptor adaptor = (IIndexElementAdaptor)go.GetComponent(typeof(IIndexElementAdaptor));
				adaptor.SetIndex(i);
				adaptor.SetUp();
				adaptor.SetUpReference();

				IUIElement element = adaptor.GetUIElement();
				resultList.Add(element);
			}
			return resultList.ToArray();
		}
		Vector2 GetIndexElementLength(){
			return new Vector2(100f, 100f);
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
