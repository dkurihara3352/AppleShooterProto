using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IStaticShootingTarget: IShootingTarget{
		void SetStaticShootingTargetReserve(
			IStaticShootingTargetReserve reserve
		);
		void ActivateAt(IShootingTargetSpawnPoint point);
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
			// thisTypedAdaptor.SetIndexOnTextMesh(index);
		}
		IStaticShootingTargetAdaptor thisTypedAdaptor{
			get{
				return (IStaticShootingTargetAdaptor)thisAdaptor;
			}
		}
		IShootingTargetSpawnPoint thisSpawnPoint;
		public void ActivateAt(IShootingTargetSpawnPoint point){
			thisSpawnPoint = point;
			point.SetTarget(this);
			SetParent(point);
			ResetLocalTransform();
			Activate();
		}
		public override void DeactivateImple(){
			if(thisSpawnPoint != null)
				thisSpawnPoint.SetTarget(null);
			thisSpawnPoint = null;
			base.DeactivateImple();
		}
		
		/* const */
			public new interface IConstArg: AbsShootingTarget.IConstArg{
			}
			public new class ConstArg: AbsShootingTarget.ConstArg, IConstArg{
				public ConstArg(
					int index,
					float health,
					Color defaultColor,
					IStaticShootingTargetAdaptor adaptor
				): base(
					index,
					health,
					defaultColor,
					adaptor
				){
				}
			}
	}
}
