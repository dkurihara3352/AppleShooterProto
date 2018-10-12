using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface ILandedArrowAdaptor: IMonoBehaviourAdaptor{
		ILandedArrow GetLandedArrow();
		void SetLandedArrowReserveAdaptor(ILandedArrowReserveAdaptor landedArrowReserveAdaptor);
	}
	public class LandedArrowAdaptor : MonoBehaviourAdaptor, ILandedArrowAdaptor {

		protected override void Awake(){
			/*  it's vital to override this method to MASK base method
				(don't wanna call addMBAdaptor and modify collection)
			*/
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
	}
}

