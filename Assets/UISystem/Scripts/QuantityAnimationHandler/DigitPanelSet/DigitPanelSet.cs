using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IDigitPanelSet: IUIElement{
		void SetDigitPanels(IDigitPanel[] panels);
		float GetDigitTargetValue();
		void PerformNumberTransition(int lesserInt, int greaterInt, float normalizedTransitionValue);
		void UpdateNumberOnPanel(int num);
		void Blank();
	}
	public class DigitPanelSet: UIElement, IDigitPanelSet{
		public DigitPanelSet(IConstArg arg): base(arg){
			/*  Create and set digit panels here
			*/
			// thisLesserPanel = CreateDigitPanel(arg.panelDim, arg.padding, isLesser: true);
			// thisGreaterPanel = CreateDigitPanel(arg.panelDim, arg.padding, isLesser: false);
			// CalcAndSetRectDimension(arg.panelDim, arg.padding);
			/*  Rect is calced and set in factory
				width => panelDim.x
				height => panelDim.y * 2 + padding.y
			*/
			thisDigitPlace = arg.digitPlace;
			thisPanelHeight = arg.panelDim.y;
			thisPaddingY = arg.padding.y;
		}
		IDigitPanel[] thisDigitPanels;
		IDigitPanel thisLesserPanel{
			get{
				return thisDigitPanels[0];
			}
		}
		IDigitPanel thisGreaterPanel{
			get{
				return thisDigitPanels[1];
			}
		}
		public void SetDigitPanels(IDigitPanel[] panels){
			thisDigitPanels = panels;
		}
		readonly int thisDigitPlace;
		readonly float thisPanelHeight;
		readonly float thisPaddingY;

		// IDigitPanel CreateDigitPanel(Vector2 panelDim, Vector2 padding, bool isLesser){
		// 	float lesserPanelLocalPosY = padding.y;
		// 	float greaterPanelLocalPosY = lesserPanelLocalPosY + panelDim.y + padding.y;
		// 	IUIElementFactory factory = thisUIElementFactory;
		// 	IDigitPanel digitPanel = factory.CreateDigitPanel(this, panelDim, isLesser?lesserPanelLocalPosY: greaterPanelLocalPosY);
		// 	return digitPanel;
		// }
		void CalcAndSetRectDimension(Vector2 panelDim, Vector2 padding){
			IQuantityRoller roller = (IQuantityRoller)this.GetParentUIElement();
			IUIAdaptor rollerAdaptor = roller.GetUIAdaptor();
			// Rect rollerRect = rollerUIA.GetRect();
			Vector2 rollerRectSize = rollerAdaptor.GetRectSize();
			float rollerWidth = rollerRectSize[0];

			float height = (panelDim.y * 2) + (padding.y * 3);
			float width = panelDim.x;
			float localX = rollerWidth - ((width + padding.x) * (thisDigitPlace +1));
			float localY = 0f;
			((IDigitPanelSetAdaptor)this.GetUIAdaptor()).SetRectDimension(height, width, localX, localY);
		}
		float thisDigitTargetValue;
		public float GetDigitTargetValue(){
			return thisDigitTargetValue;
		}
		public void PerformNumberTransition(int lesserInt, int greaterInt, float normalizedTransitionValue){
			thisDigitTargetValue = lesserInt + normalizedTransitionValue;
			thisLesserPanel.SetNumber(lesserInt);
			thisGreaterPanel.SetNumber(greaterInt);
			SetPositionInRoller(normalizedTransitionValue);
		}
		void SetPositionInRoller(float normalizedVerticalPos){
			Vector2 localPosition = GetUIAdaptor().GetLocalPosition();
			float localX = ((thisPanelHeight + thisPaddingY) * -1f) * normalizedVerticalPos;
			SetLocalPosition(new Vector2(localX, localPosition.y));
		}
		public void UpdateNumberOnPanel(int num){
			thisLesserPanel.SetNumber(num);
			SetPositionInRoller(0f);
		}
		public void Blank(){
			thisLesserPanel.SetNumber(-1);
			SetPositionInRoller(0f);
		}


		public new interface IConstArg: UIElement.IConstArg{
			int digitPlace{get;}
			Vector2 panelDim{get;}
			Vector2 padding{get;}
		}
		public new class ConstArg: UIElement.ConstArg, IConstArg{
			public ConstArg(
				IUIAdaptor adaptor, 

				int digitPlace, 
				Vector2 panelDim, 
				Vector2 padding
			): base(
				adaptor, 
				ActivationMode.None
			){
				thisDigitPlace = digitPlace;
			}
			readonly int thisDigitPlace;
			public int digitPlace{get{return thisDigitPlace;}}
			readonly Vector2 thisPanelDim;
			public Vector2 panelDim{get{return thisPanelDim;}}
			readonly Vector2 thisPadding;
			public Vector2 padding{get{return thisPadding;}}
		}
	}
}

