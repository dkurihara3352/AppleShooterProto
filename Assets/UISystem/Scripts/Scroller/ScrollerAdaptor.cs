﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IScrollerAdaptor: IUIAdaptor{
		void SetCursorRect(Rect rect);
		bool InvertsAxis(int axis);
		void SetAxisInversion(int axis, bool toggled);
	}
	public abstract class AbsScrollerAdaptor<T>: UIAdaptor, IScrollerAdaptor where T: class, IScroller{
		public ScrollerAxis scrollerAxis;
		public Vector2 relativeCursorPosition = new Vector2(.5f, .5f);
		public Vector2 rubberBandLimitMultiplier = new Vector2(.1f ,.1f);
		public bool isEnabledInertia = true;
		public float inertiaDecay = 300f;
		public float locksInputAboveThisVelocity = 200f;
		Rect thisCursorRect;
		public void SetCursorRect(Rect rect){
			thisCursorRect = rect;
		}
		bool cursorRectIsReady = false;
		protected IScroller thisScroller{
			get{
				return (IScroller)GetUIElement();
			}
		}
		public override void SetUp(){
			base.SetUp();
			thisScroller.UpdateRect();
		}
		public override void SetUpReference(){
			base.SetUpReference();
			SetUpScrollerReference();
		}
		protected virtual void SetUpScrollerReference(){
			SetUpScrollerElementAndCursor();
		}
		protected void SetUpScrollerElementAndCursor(){
			IUIElement element = GetScrollerElement();
			thisScroller.SetUpScrollerElementAndCursor(element);
			cursorRectIsReady = true;
		}
		protected IUIElement GetScrollerElement(){
			IUIElement[] childUIElements = GetChildUIElements();
			if(childUIElements.Length != 1)
				throw new System.InvalidOperationException(
					"childUIElement's count must be exactly one"
				);
			return childUIElements[0];
		}
		public override void RecalculateRect(){
			base.RecalculateRect();
			thisScroller.UpdateRect();
			SetUpScrollerElementAndCursor();
		}
		public void OnDrawGizmosSelected(){
			if(cursorRectIsReady){
				Gizmos.color = Color.red;
				float planeZ = -1f;
				Vector2 pivotOffset = GetPivotOffset();
				Vector2 pivotOffsetInPixel = GetScaledSize(pivotOffset);
				
				Vector2 cursorLocalPosInPixel = GetScaledPosition(thisCursorRect.position);

				Vector2 cursorSizeInPixel =  GetScaledSize(thisCursorRect.size);

				Vector3 bottomLeft = new Vector3(
					cursorLocalPosInPixel.x + GetPosition().x - pivotOffsetInPixel.x,
					cursorLocalPosInPixel.y + GetPosition().y - pivotOffsetInPixel.y,
					planeZ
				);
				Vector3 bottomRight = bottomLeft + (Vector3.right * cursorSizeInPixel.x);
				Vector3 topLeft = bottomLeft + Vector3.up * cursorSizeInPixel.y;
				Vector3 topRight = topLeft + Vector3.right * cursorSizeInPixel.x;
				Gizmos.DrawLine(topLeft, topRight);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(topRight, bottomRight);
				Gizmos.DrawLine(topLeft, bottomLeft);
				Gizmos.DrawLine(bottomLeft, bottomRight);

			}
		}
		public bool invertsHorizontally = false;
		public bool invertsVertically = false;
		public bool InvertsAxis(int axis){
			if(axis == 0)
				return invertsHorizontally;
			else
				return invertsVertically;
		}
		public void SetAxisInversion(int axis, bool inverted){
			if(axis == 0)
				invertsHorizontally = inverted;
			else
				invertsVertically = inverted;
		}
	}
}
