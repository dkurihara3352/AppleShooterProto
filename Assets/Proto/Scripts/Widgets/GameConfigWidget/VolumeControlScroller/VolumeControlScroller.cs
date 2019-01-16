using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

namespace SlickBowShooting{
	public interface IVolumeControlScroller: IGenericSingleElementScroller{
		void SetGameConfigWidget(IGameConfigWidget widget);
		void SetVolumeVisual(float volume);

		void SetReady();
	}
	public class VolumeControlScroller: GenericSingleElementScroller, IVolumeControlScroller{
		public VolumeControlScroller(IConstArg arg): base(arg){
		}
		public IVolumeControlScrollerAdaptor thisVolumeControlScrollerAdaptor{
			get{
				return (IVolumeControlScrollerAdaptor)thisUIAdaptor;
			}
		}
		public void SetVolumeVisual(float volume){
			PlaceScrollerElement(1f - volume, 0);
		}
		bool thisIsReady = false;
		public void SetReady(){
			thisIsReady = true;
		}
		protected override void OnScrollerElementDisplace(float normalizedCursoredPositionOnAxis, int dimension){
			if(thisIsReady)
				if(thisGameConfigWidget != null){
					if(dimension == 0){
						float newVolume = 1f - normalizedCursoredPositionOnAxis;
						if(thisControlsBGM)
							thisGameConfigWidget.SetBGMVolume(newVolume);
						else
							thisGameConfigWidget.SetSFXVolume(newVolume);
					}
				}
		}
		bool thisControlsBGM{
			get{
				return thisVolumeControlScrollerAdaptor.ControlsBGM();
			}
		}
		IGameConfigWidget thisGameConfigWidget;
		public void SetGameConfigWidget(IGameConfigWidget widget){
			thisGameConfigWidget = widget;
		}
	}
}


