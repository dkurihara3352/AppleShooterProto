using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowAdaptor: IAppleShooterMonoBehaviourAdaptor{
		ILandedArrow GetLandedArrow();
		void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor landedArrowReserveAdaptor);
		void SetIndex(int index);
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

			// thisTextMesh = CollectTextMesh();
			// SetIndexOnTextMesh(thisIndex);
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

		// TextMesh thisTextMesh;
		// TextMesh CollectTextMesh(){
		// 	Component[] childComponents = this.transform.GetComponentsInChildren(typeof(Component));
		// 	foreach(Component comp in childComponents){
		// 		if(comp is TextMesh)
		// 			return (TextMesh)comp;
		// 	}
		// 	throw new System.InvalidOperationException(
		// 		"textMesh is not set right"
		// 	);
		// }
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
			// SetIndexOnTextMesh(index);
		}
		// void SetIndexOnTextMesh(int index){
		// 	thisTextMesh.text = index.ToString();
		// }
	}
}

