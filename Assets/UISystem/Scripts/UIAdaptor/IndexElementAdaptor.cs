using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem{
	public interface IIndexElementAdaptor: IUIAdaptor{
		void SetIndex(int index);
	}
	public class IndexElementAdaptor : UIAdaptor, IIndexElementAdaptor{
		protected override IUIElement CreateUIElement(){
			UIElement.IConstArg arg = new UIElement.ConstArg(
				this,
				activationMode
			);
			return new UIElement(arg);
		}
		public override void SetUp(){
			base.SetUp();
			thisText = CollectText();
			UpdateText(thisIndex.ToString());
		}
		int thisIndex;
		Text thisText;
		Text CollectText(){
			Transform childWithText = CollectChildWithText();
			return childWithText.GetComponent<Text>();
		}
		Transform CollectChildWithText(){
			int childCount = transform.childCount;
			for(int i = 0; i < childCount; i ++){
				Transform child = transform.GetChild(i);
				if(ChildHasTextComponent(child))
					return child;
			}
			return null;
		}
		bool ChildHasTextComponent(Transform child){
			Component[] comps = child.GetComponents<Component>();
			foreach(Component comp in comps)
				if(comp is Text)
					return true;
			return false;
		}
		public void SetIndex(int index){
			thisIndex = index;
		}
		void UpdateText(string text){
			thisText.text = text;
		}
	}
}
