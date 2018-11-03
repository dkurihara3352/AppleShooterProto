using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppleShooterProto{
	public interface ITextMarkerUIAdaptor: IMarkerUIAdaptor{
		void SetText(string text);
	}
	public class TextMarkerUIAdaptor : MarkerUIAdaptor, ITextMarkerUIAdaptor {

		protected override ISceneUI CreateSceneUI(){
			TextMarkerUI.IConstArg arg = new TextMarkerUI.ConstArg(
				this,
				thisCamera,
				minUISize,
				maxUISize,
				nearUIDistance,
				farUIDistance,
				thisIndex
			);
			return new TextMarkerUI(arg);
		}
		public override void SetUp(){
			base.SetUp();
			thisText = CollectText();
		}
		Text thisText;
		Text CollectText(){
			return GetComponentInChildren<Text>();
		}
		public void SetText(string text){
			thisText.text = text;
		}
	}
}
