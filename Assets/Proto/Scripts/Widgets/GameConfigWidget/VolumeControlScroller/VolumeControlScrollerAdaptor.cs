using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace AppleShooterProto{
	public interface IVolumeControlScrollerAdaptor: IGenericSingleElementScrollerAdaptor{
		IVolumeControlScroller GetVolumeControlScroller();
		bool ControlsBGM();
	}
	public class VolumeControlScrollerAdaptor: GenericSingleElementScrollerAdaptor, IVolumeControlScrollerAdaptor{
		protected override IUIElement CreateUIElement(){
			VolumeControlScroller.IConstArg arg = new VolumeControlScroller.ConstArg(
				relativeCursorLength,
				scrollerAxis,
				relativeCursorPosition,
				rubberBandLimitMultiplier,
				isEnabledInertia,
				inertiaDecay,
				locksInputAboveThisVelocity,
				this,
				activationMode
			);
			return new VolumeControlScroller(arg);
		}
		IVolumeControlScroller thisVolumeControlScroller{
			get{
				return (IVolumeControlScroller)thisUIElement;
			}
		}
		public IVolumeControlScroller GetVolumeControlScroller(){
			return thisVolumeControlScroller;
		}
		public bool controlsBGM;
		public bool ControlsBGM(){
			return controlsBGM;
		}

		public override void SetUpReference(){
			base.SetUpReference();
			IGameConfigWidget widget = gameConfigWidgetAdaptor.GetGameConfigWidget();
			thisVolumeControlScroller.SetGameConfigWidget(widget);
		}
		public GameConfigWidgetAdaptor gameConfigWidgetAdaptor;
	}
	
}


