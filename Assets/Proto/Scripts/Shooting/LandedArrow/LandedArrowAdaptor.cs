using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ILandedArrow GetLandedArrow();
		void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor landedArrowReserveAdaptor);
		void SetIndex(int index);
		void ToggleRenderer(bool toggle);
	}
	public class LandedArrowAdaptor : AppleShooterMonoBehaviourAdaptor, ILandedArrowAdaptor {
		ILandedArrow thisLandedArrow;
		public ILandedArrow GetLandedArrow(){
			return thisLandedArrow;
		}
		public override void SetUp(){
			LandedArrow.IConstArg arg = new LandedArrow.ConstArg(
				this,
				thisIndex
			);
			thisLandedArrow = new LandedArrow(arg);

			thisTwangAdaptor = CollectArrowTwangAdaptor();
			thisTwangAdaptor.SetUp();
		}
		ILandedArrowReserveAdaptor thisReserveAdaptor;
		public void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		IArrowTwangAdaptor thisTwangAdaptor;
		IArrowTwangAdaptor CollectArrowTwangAdaptor(){
			Component[] components = transform.GetComponentsInChildren(typeof(Component));
			foreach(Component component in components){
				if(component is IArrowTwangAdaptor)
					return (IArrowTwangAdaptor)component;
			}
			throw new System.InvalidOperationException(
				"arrowTwangAdaptor is not found among child components"
			);
		}
		public override void SetUpReference(){
			ILandedArrowReserve reserve = thisReserveAdaptor.GetLandedArrowReserve();
			thisLandedArrow.SetLandedArrowReserve(reserve);

			IArrowTwang twang = thisTwangAdaptor.GetArrowTwang();
			thisLandedArrow.SetArrowTwang(twang);
		}
		public override void FinalizeSetUp(){
			thisLandedArrow.Deactivate();
		}

		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public void ToggleRenderer(bool toggle){
			meshRenderer.enabled = toggle;
		}
		public MeshRenderer meshRenderer;
	}
}

