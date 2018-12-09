using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUIElementGroupAdaptor: IUIAdaptor{
		IUIElementGroup GetUIElementGroup();
		void Fuck();
	}
	public abstract class AbsUIElementGroupAdaptor: UIAdaptor, IUIElementGroupAdaptor{
		public int columnCountConstraint;
		public int rowCountConstraint;
		public bool topToBottom;
		public bool leftToRight;
		public bool rowToColumn;
	
		public override void SetUp(){
			base.SetUp();
			MakeSureConstraintIsProperlySet();
		}
		protected IUIElementGroup thisUIElementGroup{
			get{
				return (IUIElementGroup)thisUIElement;
			}
		}
		public IUIElementGroup GetUIElementGroup(){
			return thisUIElementGroup;
		}
		protected abstract override IUIElement CreateUIElement();
		protected abstract void MakeSureConstraintIsProperlySet();

		public override void SetUpReference(){
			base.SetUpReference();
			SetUpElements();
		}
		void SetUpElements(){
			IUIElement[] groupElements = GetGroupElements();
			IRectCalculationData rectCalculationData = CreateRectCalculationData(groupElements);

			IUIElementGroup uieGroup = thisUIElementGroup;
			
			uieGroup.SetUpElements(groupElements);
			uieGroup.SetUpRects(rectCalculationData);
			uieGroup.PlaceElements();
		}
		protected abstract IUIElement[] GetGroupElements();
		protected abstract IRectCalculationData CreateRectCalculationData(IUIElement[] groupElements);
		bool thisIsScrollerElement{
			get{
				IUIElement parent = GetParentUIElement();
				if(parent != null){
					if(parent is IScroller)
						return true;
				}
				return false;
			}
		}
		public override void RecalculateRect(){
			base.RecalculateRect();
			if(thisIsScrollerElement)
				return;
			else
				SetUpElements();
		}
		public void Fuck(){
			SetUpElements();
		}
	}
}

