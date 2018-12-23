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
		public BowLockPaneAdaptor bowLockPaneAdaptor;
		public BowEquippedTextPaneAdaptor bowEquippedTextPaneAdaptor;
		public BowStarsPaneAdaptor bowLevelPaneAdaptor;
		public BowStarsPaneAdaptor[] bowAttributeLevelPaneAdaptors;
		public override void SetUpReference(){
			base.SetUpReference();
			IBowLockPane bowLockPane = bowLockPaneAdaptor.GetBowLockPane();
			thisBowPanel.SetBowLockPane(bowLockPane);

			IBowEquippedTextPane bowEquippedTextPane = bowEquippedTextPaneAdaptor.GetBowEquippedTextPane();
			thisBowPanel.SetBowEquippedTextPane(bowEquippedTextPane);

			IBowStarsPane bowLevelPane = bowLevelPaneAdaptor.GetBowStarsPane();
			thisBowPanel.SetBowLevelPane(bowLevelPane);

			IBowStarsPane[] bowAttributeLevelPanes = CollectBowAttributeLevelPanes();
			thisBowPanel.SetBowAttributeLevelPanes(bowAttributeLevelPanes);
		}
		IBowStarsPane[] CollectBowAttributeLevelPanes(){
			List<IBowStarsPane> resultList = new List<IBowStarsPane>();
			foreach(IBowStarsPaneAdaptor paneAdaptor in bowAttributeLevelPaneAdaptors){
				resultList.Add(paneAdaptor.GetBowStarsPane());
			}
			return resultList.ToArray();
		}
	}
}
