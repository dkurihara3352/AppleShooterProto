using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IResultLabelPane: IAlphaVisibilityTogglableUIElement{
		void ResetResultLabelPane();
	}
	public class ResultLabelPane: AlphaVisibilityTogglableUIElement, IResultLabelPane{
		public ResultLabelPane(IConstArg arg): base(arg){}
		IResultLabelPaneAdaptor thisResultLabelPaneAdaptor{
			get{
				return(IResultLabelPaneAdaptor)thisUIAdaptor;
			}
		}
		public void ResetResultLabelPane(){
			thisResultLabelPaneAdaptor.SetAlpha(0f);
		}
	}
}

