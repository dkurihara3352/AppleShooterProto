using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public abstract class AbsGUIManager: MonoBehaviour{
		protected Rect GetGUIRect(
			Vector2 normalizedSize,
			Vector2 normalizedPosition
		){
			Vector2 size = new Vector2(
				Screen.width * normalizedSize.x,
				Screen.height * normalizedSize.y
			);
			Vector2 position = new Vector2(
				normalizedPosition.x * (Screen.width - size.x),
				normalizedPosition.y * (Screen.height - size.y)
			);
			return new Rect(
				position, 
				size
			);
		}
		protected Rect GetSubRect(
			Rect guiRect,
			int index,
			int totalCount
		){
			Vector2 subRectSize = new Vector2(
				guiRect.width,
				guiRect.height /totalCount
			);
			Vector2 subRectPosition = new Vector2(
				guiRect.x,
				guiRect.y + (index * subRectSize.y)
			);
			Rect result = new Rect(
				subRectPosition,
				subRectSize
			);
			return result;
		}
		protected Rect GetHorizontalSubRect(
			Rect source,
			int index,
			int count
		){
			float width = source.width/ count;
			float height = source.height;
			float posX = (index * width) + source.position.x;
			float posY = source.position.y;

			return new Rect(posX, posY, width, height);
		}
	}
}
