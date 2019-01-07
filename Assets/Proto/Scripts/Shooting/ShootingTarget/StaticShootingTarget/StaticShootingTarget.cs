using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlickBowShooting{
	public interface IStaticShootingTarget: IShootingTarget{
		void SetStaticShootingTargetReserve(
			IStaticShootingTargetReserve reserve
		);
		void ActivateAt(IStaticTargetSpawnPoint point);
		IStaticTargetSpawnPoint GetShootingTargetSpawnPoint();
	}
	public class StaticShootingTarget: AbsShootingTarget, IStaticShootingTarget{
		public StaticShootingTarget(
			IConstArg arg
		): base(arg){
		}
		public void SetStaticShootingTargetReserve(
			IStaticShootingTargetReserve reserve
		){
			thisStaticShootingTargetReserve = reserve;
		}
		IStaticShootingTargetReserve thisStaticShootingTargetReserve;
		protected override void ReserveSelf(){
			thisStaticShootingTargetReserve.Reserve(this);
		}
		public override void SetIndex(int index){
			base.SetIndex(index);
		}
		IStaticShootingTargetAdaptor thisTypedAdaptor{
			get{
				return (IStaticShootingTargetAdaptor)thisAdaptor;
			}
		}
		IStaticTargetSpawnPoint thisSpawnPoint;
		public IStaticTargetSpawnPoint GetShootingTargetSpawnPoint(){
			return thisSpawnPoint;
		}
		public void ActivateAt(IStaticTargetSpawnPoint point){
			Deactivate();
			SetSpawnPoint(point);
			Activate();
		}
		void SetSpawnPoint(IStaticTargetSpawnPoint point){
			thisSpawnPoint = point;
			point.SetTarget(this);
			SetParent(point);
			ResetLocalTransform();
		}
		public override void DeactivateImple(){
			base.DeactivateImple();
			ClearSpawnPoint();
		}
		void ClearSpawnPoint(){
			if(thisSpawnPoint != null)
				thisSpawnPoint.CheckAndClearTarget(this);
			thisSpawnPoint = null;
		}
		
		/* const */
			public new interface IConstArg: AbsShootingTarget.IConstArg{
			}
			public new class ConstArg: AbsShootingTarget.ConstArg, IConstArg{
				public ConstArg(
					int index,
					UnityBase.IBellCurve healthBellCurve,
					IStaticShootingTargetAdaptor adaptor,
					ITargetData targetData
				): base(
					index,
					healthBellCurve,
					adaptor,
					targetData
				){
				}
			}
	}
}
