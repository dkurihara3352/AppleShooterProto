using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IResizableRectUIAdaptor: IUIAdaptor{
		void SetRectDimension(
			float height, 
			float width, 
			float localPosX, 
			float localPosY
		);
	}
	public abstract class AbsResizableRectUIAdaptor<T>: UIAdaptor, IResizableRectUIAdaptor where T: IUIElement{
		public void SetRectDimension(
			float height, 
			float width, 
			float localPosX, 
			float localPosY
		){
			Rect rect = GetRect();
			rect.height = height;
			rect.width = width;
			this.transform.localPosition = new Vector2(localPosX, localPosY);
		}
	}
	public interface IQuantityRollerAdaptor: IResizableRectUIAdaptor/* , IInstatiableUIAdaptor */{
		IQuantityRoller GetQuantityRoller();
	}
	public class QuantityRollerAdaptor: AbsResizableRectUIAdaptor<IQuantityRoller>, IQuantityRollerAdaptor{
		public int thisMaxQuantity;
		public Vector2 thisPanelDim;
		public Vector2 thisPadding;
		public Vector2 thisRollerNormalizedPos;
		protected override IUIElement CreateUIElement(){
			QuantityRoller.IConstArg arg = new QuantityRoller.ConstArg(
				this,
				thisPanelDim,
				thisPadding,
				thisRollerNormalizedPos
			);

			QuantityRoller quantityRoller = new QuantityRoller(arg);
			return quantityRoller;
		}
		IQuantityRoller thisQuantityRoller{
			get{
				return (IQuantityRoller)thisUIElement;
			}
		}
		public IQuantityRoller GetQuantityRoller(){
			return thisQuantityRoller;
		}
		public override void SetUpReference(){
			base.SetUpReference();
			IDigitPanelSet[] digitPanelSets = CreateDigitPanelSets();
			thisQuantityRoller.SetDigitPanelSets(digitPanelSets);
		}
		IDigitPanelSet[] CreateDigitPanelSets(){
			return null;
		}
	}
}
