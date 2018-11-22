using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ITextMarkerUIReserve: IMarkerUIReserve{
		ITextMarkerUI ActivateTextMarkerUIAt(
			ISceneObject obj,
			string text
		);
	}
	public class TextMarkerUIReserve: MarkerUIReserve, ITextMarkerUIReserve{
		public TextMarkerUIReserve(
			IConstArg arg
		): base(arg){}
		public ITextMarkerUI ActivateTextMarkerUIAt(
			ISceneObject obj,
			string text
		){
			base.ActivateMarkerUIAt(obj);
			ITextMarkerUI current = (ITextMarkerUI)thisCurrent;
			current.SetText(text);
			current.SetAttachedSceneObject(obj);
			return current;
		}
	}
}

