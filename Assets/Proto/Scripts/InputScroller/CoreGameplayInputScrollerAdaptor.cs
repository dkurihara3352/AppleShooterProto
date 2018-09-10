using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface ICoreGameplaceInputScrollerAdaptor: IGenericSingleElementScrollerAdaptor{}
	public class CoreGameplayInputScrollerAdaptor : GenericSingleElementScrollerAdaptor, ICoreGameplaceInputScrollerAdaptor {
		public IPlayerInputManagerAdaptor inputManagerAdaptor;
		protected override IUIElement CreateUIElement(IUIImage image){
			ICoreGameplayInputScrollerConstArg arg = new CoreGameplayInputScrollerConstArg(

			);
			return new CoreGameplayInputScroller(arg);
		}
	}
}
