using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface ITextMarkerUI: IMarkerUI{
		void SetText(string text);
		void SetAttachedSceneObject(ISceneObject sceneObject);
		void CheckAndDeactivate(ISceneObject obj);
	}
	public class TextMarkerUI: MarkerUI, ITextMarkerUI{
		public TextMarkerUI(
			IConstArg arg
		): base(arg){
		}
		ITextMarkerUIAdaptor thisTypedAdaptor{
			get{
				return (ITextMarkerUIAdaptor)thisAdaptor;
			}
		}
		public void SetText(string text){
			thisTypedAdaptor.SetText(text);
		}
		ISceneObject thisAttachedSceneObject;
		public void SetAttachedSceneObject(
			ISceneObject obj
		){
			thisAttachedSceneObject = obj;
		}
		public void CheckAndDeactivate(ISceneObject obj){
			if(obj == thisAttachedSceneObject){
				thisAttachedSceneObject = null;
				Deactivate();
			}
		}
	}
}
