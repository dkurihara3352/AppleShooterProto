using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IInstantiatedElementGroupAdaptor: IUIElementGroupAdaptor{}
	public class InstantiatedElementGroupAdaptor : BaseGridGroupAdaptor, IInstantiatedElementGroupAdaptor {
		
		public int groupElementCount;
		public GameObject uiElementPrefab;
		IUIElement[] thisGroupElements;
		protected override IUIElement[] GetGroupElements(){
			if(thisGroupElements == null)
				thisGroupElements = CreateUIEs();
			return thisGroupElements;
		}
		IUIElement[] CreateUIEs(){
			List<IUIElement> resultList = new List<IUIElement>();
			for(int i = 0; i < groupElementCount; i++ ){
				GameObject go = GameObject.Instantiate(uiElementPrefab, this.GetTransform());
				IUIAdaptor adaptor = (IUIAdaptor)go.GetComponent(typeof(IUIAdaptor));
				adaptor.SetIndex(i);
				adaptor.SetUpRecursively();
				adaptor.SetUpReferenceRecursively();

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
