using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ITitlePane: IAlphaVisibilityTogglableUIElement{}
	public class TitlePane: AlphaVisibilityTogglableUIElement, ITitlePane{
		public TitlePane(IConstArg arg): base(arg){}
	}
}

