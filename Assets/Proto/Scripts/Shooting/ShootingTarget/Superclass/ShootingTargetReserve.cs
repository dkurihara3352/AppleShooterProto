using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{

	public interface IShootingTargetReserve{
		void SetTargetSpawnData(TargetSpawnData data);
		TargetType GetTargetType();
		void ActivateShootingTargetAt(IShootingTargetSpawnPoint point);
	}
	public abstract class AbsShootingTargetReserve<T> : AbsSceneObjectReserve<T>, IShootingTargetReserve where T: IShootingTarget{
		public AbsShootingTargetReserve(
			IConstArg arg
		): base(arg){
			thisReservedSpace = arg.reservedSpace;
		}
		public override void Reserve(T target){
			target.SetParent(this);
			target.ResetLocalTransform();
			Vector3 reservedPosition = GetReservedLocalPosition(target.GetIndex());
			target.SetLocalPosition(reservedPosition);
		}
		protected float thisReservedSpace;
		Vector3 GetReservedLocalPosition(int index){
			float posX = index * thisReservedSpace;
			return new Vector3(
				posX, 0f, 0f
			);
		}
		protected TargetSpawnData thisTargetSpawnData;
		public void SetTargetSpawnData(TargetSpawnData data){
			thisTargetSpawnData = data;
		}
		public TargetType GetTargetType(){
			return thisTypedAdaptor.GetTargetType();
		}
		IShootingTargetReserveAdaptor thisTypedAdaptor{
			get{
				return (IShootingTargetReserveAdaptor)thisAdaptor;
			}
		}
		public abstract void ActivateShootingTargetAt(IShootingTargetSpawnPoint point);
		/*  */
			public new interface IConstArg: AbsSceneObject.IConstArg{
				float reservedSpace{get;}
			}
			public new class ConstArg: AbsSceneObject.ConstArg, IConstArg{
				public ConstArg(
					IShootingTargetReserveAdaptor adaptor,
					float reservedSpace
				): base(
					adaptor
				){
					thisReservedSpace = reservedSpace;
				}
				readonly float thisReservedSpace;
				public float reservedSpace{
					get{return thisReservedSpace;}
				}
			}
		/*  */
	}
}
