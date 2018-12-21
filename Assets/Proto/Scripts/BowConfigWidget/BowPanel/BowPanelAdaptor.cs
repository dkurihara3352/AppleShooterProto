using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IBowPanelAdaptor: IUIAdaptor{
		int GetBowIndex();
		IBowPanel GetBowPanel();
	}
	public class BowPanelAdaptor: UIAdaptor, IBowPanelAdaptor{
		protected override IUIElement CreateUIElement(){
			return CreateBowPanel();
		}
		IBowPanel thisBowPanel{
			get{
				return (IBowPanel)thisUIElement;
			}
		}
		public IBowPanel GetBowPanel(){
			return thisBowPanel;
		}
		IBowPanel CreateBowPanel(){
			BowPanel.IConstArg arg = new BowPanel.ConstArg(
				this,
				activationMode
			);
			return new BowPanel(arg);
		}
		public int bowIndex;
		public int GetBowIndex(){
			return bowIndex;
		}
	}
}
