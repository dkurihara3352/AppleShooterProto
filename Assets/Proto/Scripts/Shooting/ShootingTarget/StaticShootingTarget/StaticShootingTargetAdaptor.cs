using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IStaticShootingTargetAdaptor: IShootingTargetAdaptor{
		IStaticShootingTarget GetStaticShootingTarget();
		void SetStaticShootingTargetReserveAdaptor(IStaticShootingTargetReserveAdaptor reserveAdaptor);
	}
	public class StaticShootingTargetAdaptor: AbsShootingTargetAdaptor, IStaticShootingTargetAdaptor{
		public override void SetUp(){
			base.SetUp();
		}
		protected override IShootingTarget CreateShootingTarget(){
			StaticShootingTarget.IConstArg arg = new StaticShootingTarget.ConstArg(
				thisIndex,
				thisHealthBellCurve,
				this,
				targetData
			);
			return new StaticShootingTarget(arg);

		}
		IStaticShootingTarget thisStaticShootingTarget{
			get{
				return(IStaticShootingTarget)thisShootingTarget;
			}
		}
		public IStaticShootingTarget GetStaticShootingTarget(){
			return thisStaticShootingTarget;
		}
		IStaticShootingTargetReserveAdaptor thisReserveAdaptor;
		public void SetStaticShootingTargetReserveAdaptor(IStaticShootingTargetReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		public StaticShootingTargetReserveAdaptor staticShootingTargetReserveAdaptor;
		public override void SetUpReference(){
			base.SetUpReference();
			if(staticShootingTargetReserveAdaptor != null)
				thisReserveAdaptor = staticShootingTargetReserveAdaptor;
			IStaticShootingTargetReserve reserve = thisReserveAdaptor.GetStaticShootingTargetReserve();
			thisStaticShootingTarget.SetStaticShootingTargetReserve(reserve);
		}
	}	
}

