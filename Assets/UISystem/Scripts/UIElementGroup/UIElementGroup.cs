﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UISystem{
	public interface IUIElementGroup: IUIElement{
		int GetSize();
		IUIElement[] GetGroupElements();
		int GetArraySize(int dimension);
		IUIElement GetGroupElement(int index);
		int GetGroupElementIndex(IUIElement groupElement);
		IUIElement GetGroupElement(int columnIndex, int rowIndex);
		int[] GetGroupElementArrayIndex(IUIElement groupElement);
		IUIElement[] GetGroupElementsWithinIndexRange(int minColumnIndex, int minRowIndex, int maxColumnIndex, int maxRowIndex);
		IUIElement GetGroupElementAtPositionInGroupSpace(Vector2 positionInElementGroupSpace);


		void SetUpElements(IUIElement[] elements);

		
		Vector2 GetGroupElementSize();
		Vector2 GetPadding();
		void SetUpRects(IRectCalculationData rectCalculationData);
		void PlaceElements();
	}
	public abstract class AbsUIElementGroup<T> : UIElement, IUIElementGroup where T: class, IUIElement{
		public AbsUIElementGroup(
			IConstArg arg
		) :base(arg){
			thisRowCountConstraint = arg.rowCountConstraint;
			thisColumnCountConstraint = arg.columnCountConstraint;
			MakeSureArrayConstraintIsProperlySet();
			CheckAndSetMaxElementsCount();
			thisTopToBottom = arg.topToBottom;
			thisLeftToRight = arg.leftToRight;
			thisRowToColumn = OverrideRowToColumnAccordingToConstraint(arg.rowToColumn);
			thisArrayIndexCalculator = new UIElementGroupArrayIndexCalculator(
				thisTopToBottom, 
				thisLeftToRight, 
				thisRowToColumn
			);
		}
		/* Construction */
			void MakeSureArrayConstraintIsProperlySet(){
				if(thisRowCountConstraint == 0 && thisColumnCountConstraint == 0)
					throw new System.InvalidOperationException("either rowCount or columnCount must be defined");
			}
			bool OverrideRowToColumnAccordingToConstraint(bool rowToColumn){
				bool result = rowToColumn;
				if(thisColumnCountConstraint == 0)
					result = false;
				else if(thisRowCountConstraint == 0)
					result = true;
				return result;
			}
			readonly int thisColumnCountConstraint = 0;
			readonly int thisRowCountConstraint = 0;
			bool thisIsConstrainedByColumnCount{get{return thisRowCountConstraint == 0 && thisColumnCountConstraint != 0;}}
			bool thisIsConstrainedByRowCount{get{return thisColumnCountConstraint == 0 && thisRowCountConstraint != 0;}}
			bool thisIsConstrainedByBothAxis{get{return thisColumnCountConstraint != 0 && thisRowCountConstraint != 0;}}
			readonly bool thisTopToBottom;
			readonly bool thisLeftToRight;
			readonly bool thisRowToColumn;
			int thisMaxElementCount = 0;/* used only when both axis are constrained */
			void CheckAndSetMaxElementsCount(){
				if(thisColumnCountConstraint != 0 && thisRowCountConstraint != 0)
					thisMaxElementCount = thisColumnCountConstraint * thisRowCountConstraint;
			}
		/* Accessing elements */
			protected T[] thisGroupElements;/* explicitly and externally set */
			public IUIElement GetGroupElement(int index){
				return thisGroupElements[index];
			}
			public IUIElement[] GetGroupElements(){
				List<IUIElement> result = new List<IUIElement>();
				foreach(T element in thisGroupElements){
					result.Add(element);
				}
				return result.ToArray();
			}
			public int GetGroupElementIndex(IUIElement groupElement){
				if(groupElement != null){
					int index = 0;
					foreach(T uie in thisGroupElements){
						if( uie == groupElement)
							return index;
						index ++;
					}
					throw new System.InvalidOperationException("groupElement is not found among thisGroupElements");
				}else
					throw new System.InvalidOperationException("groupElement should not be null");
			}
			public int GetSize(){return thisGroupElements.Length;}
			protected T[,] thisElementsArray;
			public IUIElement GetGroupElement(int columnIndex, int rowIndex){
				return thisElementsArray[columnIndex, rowIndex];
			}
			public int GetArraySize(int dimension){
				return thisElementsArray.GetLength(dimension);
			}
			public int[] GetGroupElementArrayIndex(IUIElement element){
				return thisGroupElementsArrayCalculator.GetGroupElementArrayIndex(element);
			}
		/* Setting up elements */
			public void SetUpElements(IUIElement[] elements){
				MakeSureElementsCountIsValid(elements.Length);
				thisGroupElements = CreateTypedElements(elements);
				ChildrenAllElements(elements);
				CalcAndSetGridCounts();
				SetUpElementsArray(elements);
				SetElementsDependentCalculators();
			}
			void MakeSureElementsCountIsValid(int count){
				if(thisIsConstrainedByBothAxis)
					if(count > thisMaxElementCount)
						throw new System.InvalidOperationException(
							"elements count exceeds maximum allowed count. \b" + 
							"try either decrease the elements count or release one of the array constraints"
						);
			}
			T[] CreateTypedElements(IUIElement[] source){
				List<T> resultList = new List<T>();
				foreach(IUIElement uie in source){
					resultList.Add((T)uie);
				}
				return resultList.ToArray();
			}
			void ChildrenAllElements(IUIElement[] elements){
				foreach(IUIElement uie in elements)
					uie.SetParent(this);
			}

			void CalcAndSetGridCounts(){
				thisNumOfColumns = CalcNumberOfColumnsToCreate();
				thisNumOfRows = CalcNumberOfRowsToCreate();
			}
			int thisNumOfColumns;
			int thisNumOfRows;
			protected int CalcNumberOfColumnsToCreate(){
				if(thisColumnCountConstraint != 0)
					return thisColumnCountConstraint;
				else{
					int quotient = thisGroupElements.Length / thisRowCountConstraint;
					int modulo = thisGroupElements.Length % thisRowCountConstraint;
					return modulo > 0? quotient + 1 : quotient;
				}
			}
			protected int CalcNumberOfRowsToCreate(){
				if(thisRowCountConstraint != 0)
					return thisRowCountConstraint;
				else{
					int quotient = thisGroupElements.Length / thisColumnCountConstraint;
					int modulo = thisGroupElements.Length % thisColumnCountConstraint;
					return modulo > 0? quotient + 1 : quotient;
				}
			}
			void SetUpElementsArray(IUIElement[] elements){
				thisElementsArray = CreateElements2DArray(
					thisNumOfColumns, 
					thisNumOfRows
				);
			}
			protected T[ , ] CreateElements2DArray(int numOfColumns, int numOfRows){
				T[ , ] array = new T[ numOfColumns, numOfRows];
				foreach(T element in thisGroupElements){
					int elementIndex = GetGroupElementIndex(element);
					int columnIndex = CalcColumnIndex(elementIndex, numOfColumns, numOfRows);
					int rowIndex = CalcRowIndex(elementIndex, numOfColumns, numOfRows);
					array[columnIndex, rowIndex] = element;
				}
				return array;
			}
			UIElementGroupArrayIndexCalculator thisArrayIndexCalculator;
			protected int CalcColumnIndex(int n, int numOfColumns, int numOfRows){
				return thisArrayIndexCalculator.CalcColumnIndex(n, numOfColumns, numOfRows);
			}
			protected int CalcRowIndex(int n, int numOfColumns, int numOfRows){
				return thisArrayIndexCalculator.CalcRowIndex(n, numOfColumns, numOfRows);
			}
		/* Setting up rects */
			/*  * Note *
				Three variable that affects the rects
					ElementGroupRectLength
					GroupElementLength
					PaddingLength
				two of these three must be somehow constrained to solve for each values
					Fixed GroupLength
					Fixed ElementLength
					Fixed PaddingLength
					Ratio of
						GroupToElement
						GropuToPadding
						ElementToPadding

					Fixed is either of
						constant value
						proportional to reference
			*/
			public void SetUpRects(IRectCalculationData rectCalculationData){
				thisRectCalculationData = rectCalculationData;
				thisRectCalculationData.SetColumnAndRowCount(
					thisNumOfColumns, 
					thisNumOfRows
				);
				CalculateAndSetRects(rectCalculationData);
				SetRectsDependentCalculators();
			}
			IRectCalculationData thisRectCalculationData;
			void CalculateAndSetRects(IRectCalculationData data){
				data.CalculateRects();
				// SetUpGroupSize(data.groupLength);
				thisUIAdaptor.SetRectSize(data.groupSize);
				// SetUpElementSize(data.elementSize);
				foreach(IUIElement ele in thisGroupElements)
					ele.SetRectSize(data.elementSize);
				thisPadding = data.padding;
			}
			// protected void SetUpGroupSize(Vector2 groupSize){
			// 	thisGroupSize = groupSize;
			// 	thisUIAdaptor.SetRectSize(groupSize);
			// }
			// protected void SetUpElementSize(Vector2 elementSize){
			// 	thisElementSize = elementSize;
			// 	foreach(IUIElement element in thisGroupElements){
			// 		IUIAdaptor elementUIA = element.GetUIAdaptor();
			// 		elementUIA.SetRectSize(elementSize);
			// 	}
			// }
			// protected void SetUpPadding(Vector2 padding){
			// 	thisPadding = padding;
			// }
			Vector2 thisGroupSize;
			Vector2 thisElementSize{
				get{
					return thisGroupElements[0].GetRectSize();
				}
			}
			public Vector2 GetGroupElementSize(){
				return thisElementSize;
			}
			Vector2 thisPadding;
			public Vector2 GetPadding(){
				return thisPadding;
			}
		/* calculators */
			void SetElementsDependentCalculators(){
				thisGroupElementsArrayCalculator = new GroupElementsArrayCalculator(
					thisElementsArray
				);
			}
			IGroupElementAtPositionInGroupSpaceCalculator thisGroupElementAtPositionInGroupSpaceCalculator;
			public IUIElement GetGroupElementAtPositionInGroupSpace(Vector2 positionInElementGroupSpace){
				return thisGroupElementAtPositionInGroupSpaceCalculator.Calculate(positionInElementGroupSpace);
			}
			IGroupElementsArrayCalculator thisGroupElementsArrayCalculator;
			public IUIElement[] GetGroupElementsWithinIndexRange(
				int minColumnIndex, 
				int minRowIndex, 
				int maxColumnIndex, 
				int maxRowIndex
			){
				return thisGroupElementsArrayCalculator.GetGroupElementsWithinIndexRange(minColumnIndex, minRowIndex, maxColumnIndex, maxRowIndex);
			}
			protected virtual void SetRectsDependentCalculators(){
				thisGroupElementAtPositionInGroupSpaceCalculator = new GroupElementAtPositionInGroupSpaceCalculator(
					thisElementsArray, 
					thisElementSize, 
					thisPadding, 
					thisUIAdaptor.GetRectSize(),
					GetName()
				);
			}
		/* Placing Elements */
			public void PlaceElements(){
				foreach(T element in thisGroupElements){
					int[] arrayIndex = GetGroupElementArrayIndex(element);
					float localPosX = (arrayIndex[0] * (thisElementSize.x + thisPadding.x)) + thisPadding.x;
					float localPosY = (arrayIndex[1] * (thisElementSize.y + thisPadding.y)) + thisPadding.y;
					Vector2 newLocalPos = new Vector2(localPosX, localPosY);
					element.SetLocalPosition(newLocalPos);
				}
			}
		/* Const */
			public new interface IConstArg: UIElement.IConstArg{
				int columnCountConstraint{get;}
				int rowCountConstraint{get;}
				bool topToBottom{get;}
				bool leftToRight{get;}
				bool rowToColumn{get;}
			}
			public new class ConstArg: UIElement.ConstArg ,IConstArg{
				public ConstArg(
					int columnCountConstraint, 
					int rowCountConstraint, 
					bool topToBottom, 
					bool leftToRight, 
					bool rowToColumn, 

					IUIElementGroupAdaptor adaptor, 
					ActivationMode activationMode

				): base(
					adaptor, 
					activationMode
				){
					thisColumnCountConstraint = columnCountConstraint;
					thisRowCountConstraint = rowCountConstraint;
					thisTopToBottom = topToBottom;
					thisLeftToRight = leftToRight;
					thisRowToColumn = rowToColumn;
				}
				readonly int thisColumnCountConstraint;
				public int columnCountConstraint{get{return thisColumnCountConstraint;}}
				readonly int thisRowCountConstraint;
				public int rowCountConstraint{get{return thisRowCountConstraint;}}
				readonly bool thisTopToBottom;
				public bool topToBottom{get{return thisTopToBottom;}}
				readonly bool thisLeftToRight;
				public bool leftToRight{get{return thisLeftToRight;}}
				readonly bool thisRowToColumn;
				public bool rowToColumn{get{return thisRowToColumn;}}
			}
	}



}
