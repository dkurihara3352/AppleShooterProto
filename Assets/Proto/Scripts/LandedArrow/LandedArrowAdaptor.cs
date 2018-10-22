using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowAdaptor: IMonoBehaviourAdaptor{
		ILandedArrow GetLandedArrow();
		void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor landedArrowReserveAdaptor);
		void SetIndexOnTextMesh(int index);
	}
	public class LandedArrowAdaptor : MonoBehaviourAdaptor, ILandedArrowAdaptor {

		protected override void Awake(){
			/*  it's vital to override this method to MASK base method
				(don't wanna call addMBAdaptor and modify collection)
			*/
			thisTextMesh = CollectTextMesh();
			return;
		}
		ILandedArrow thisLandedArrow;
		public ILandedArrow GetLandedArrow(){
			return thisLandedArrow;
		}
		public override void SetUp(){
			LandedArrow.IConstArg arg = new LandedArrow.ConstArg(
				this
			);
			thisLandedArrow = new LandedArrow(arg);
		}
		ILandedArrowReserveAdaptor thisReserveAdaptor;
		public void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		public override void SetUpReference(){
			ILandedArrowReserve reserve = thisReserveAdaptor.GetLandedArrowReserve();
			thisLandedArrow.SetLandedArrowReserve(reserve);
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
	}
}

