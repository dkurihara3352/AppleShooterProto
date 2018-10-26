using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowAdaptor: IInstatiableMonoBehaviourAdaptor{
		ILandedArrow GetLandedArrow();
		void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor landedArrowReserveAdaptor);
		void SetIndexOnTextMesh(int index);
		void SetArrowTwangAdaptor(IArrowTwangAdaptor adaptor);
	}
	public class LandedArrowAdaptor : InstatiableMonoBehaviourAdaptor, ILandedArrowAdaptor {
		ILandedArrow thisLandedArrow;
		public ILandedArrow GetLandedArrow(){
			return thisLandedArrow;
		}
		public override void SetUp(){
			LandedArrow.IConstArg arg = new LandedArrow.ConstArg(
				this
			);
			thisLandedArrow = new LandedArrow(arg);

			thisTextMesh = CollectTextMesh();
		}
		ILandedArrowReserveAdaptor thisReserveAdaptor;
		public void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		public override void SetUpReference(){
			ILandedArrowReserve reserve = thisReserveAdaptor.GetLandedArrowReserve();
			thisLandedArrow.SetLandedArrowReserve(reserve);

			IArrowTwang twang = thisTwangAdaptor.GetArrowTwang();
			thisLandedArrow.SetArrowTwang(twang);
		}

		TextMesh thisTextMesh;
		TextMesh CollectTextMesh(){
			Component[] childComponents = this.transform.GetComponentsInChildren(typeof(Component));
			foreach(Component comp in childComponents){
				if(comp is TextMesh)
					return (TextMesh)comp;
			}
			throw new System.InvalidOperationException(
				"textMesh is not set right"
			);
		}
		public void SetIndexOnTextMesh(int index){
			thisTextMesh.text = index.ToString();
		}
		IArrowTwangAdaptor thisTwangAdaptor;
		public void SetArrowTwangAdaptor(IArrowTwangAdaptor twangAdaptor){
			thisTwangAdaptor = twangAdaptor;
		}
	}
}

