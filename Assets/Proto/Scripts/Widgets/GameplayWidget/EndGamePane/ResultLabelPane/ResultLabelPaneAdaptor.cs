using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IResultLabelPaneAdaptor: IAlphaVisibilityTogglableUIAdaptor{
		IResultLabelPane GetResultLabelPane();
	}
	public class ResultLabelPaneAdaptor: AlphaVisibilityTogglableUIAdaptor, IResultLabelPaneAdaptor{
		protected override IUIElement CreateUIElement(){
			ResultLabelPane.IConstArg arg = new ResultLabelPane.ConstArg(
				this,
				activationMode
			);
			return new ResultLabelPane(arg);
		}
		IResultLabelPane thisResultLabelPane{
			get{
				return (IResultLabelPane)thisUIElement;
			}
		}
		public IResultLabelPane GetResultLabelPane(){
			return thisResultLabelPane;
		}
	}
}

