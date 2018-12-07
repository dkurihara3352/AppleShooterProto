using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RTDebugger : MonoBehaviour {

	void OnGUI(){
		CalcRects();
		DrawBottomRightRect();
	}
	Rect bottomRightRect;
	void CalcRects(
	){
		if(bottomRightRect == default(Rect))
			bottomRightRect = GetBottomRightRect();
	}
	Rect GetBottomRightRect(){
		return GetGUIRect(
			new Vector2(.3f, .3f),
			new Vector2(1f, 1f)
		);
	}
	Rect GetGUIRect(
		Vector2 normalizedSize,
		Vector2 normalizedPosition
	){
		Vector2 rectSize = new Vector2(
			Screen.width * normalizedSize.x,
			Screen.height * normalizedSize.y
		);
		Vector2 position = new Vector2(
			(Screen.width - rectSize.x) * normalizedPosition.x,
			(Screen.height - rectSize.y) * normalizedPosition.y
		);
		return new Rect(
			position,
			rectSize
		);
	}
	public RectTransform rectTransformToDebug;
	void DrawBottomRightRect(){
		DrawRectTransformDebug(bottomRightRect);
	}
	void DrawRectTransformDebug(
		Rect rect
	){
		string result = "";
		if(rectTransformToDebug != null){
			result += "localPos: " + rectTransformToDebug.localPosition.ToString() + "\n";
			result += "size: " + rectTransformToDebug.sizeDelta.ToString() + "\n";
			result += "rect: " + rectTransformToDebug.rect.ToString()+ "\n";
			result += "achoredPos: " + rectTransformToDebug.anchoredPosition.ToString() + "\n";
		}
		GUI.Label(
			rect,
			result
		);
	}

}
