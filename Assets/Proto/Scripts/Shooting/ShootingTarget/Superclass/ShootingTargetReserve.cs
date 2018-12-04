using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;

namespace AppleShooterProto{

	public interface IShootingTargetReserve{
		void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve);
		TargetType GetTargetType();
		void ActivateShootingTargetAt(IShootingTargetSpawnPoint point);
		void SetTier(int tier);
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
		public TargetType GetTargetType(){
			return thisTypedAdaptor.GetTargetType();
		}
		IShootingTargetReserveAdaptor thisTypedAdaptor{
			get{
				return (IShootingTargetReserveAdaptor)thisAdaptor;
			}
		}
		public abstract void ActivateShootingTargetAt(IShootingTargetSpawnPoint point);
		public void SetTier(int tier){
			Material mat = thisTypedAdaptor.GetMaterialForTier(tier);
			TargetData targetData = thisTypedAdaptor.GetTargetDataForTier(tier);
			TargetTierData tierData = new TargetTierData(
				mat,
				targetData
			);
			SetAllTargetTier(
				tierData
			);
			SetAllDestroyedTargetTier(
				tierData
			);
		}
		void SetAllTargetTier(
			TargetTierData tierData
		){
			foreach(IShootingTarget target in thisSceneObjects){
				if(!target.IsActivated()){
					target.SetTier(
						tierData
					);
				}else{
					target.SetTargetTierDataOnQueue(
						tierData
					);
				}
			}
		}
		IDestroyedTargetReserve thisDestroyedTargetReserve;
		public void SetDestroyedTargetReserve(IDestroyedTargetReserve reserve){
			thisDestroyedTargetReserve = reserve;
		}
		void SetAllDestroyedTargetTier(
			TargetTierData tierData
		){
			foreach(IDestroyedTarget target in thisDestroyedTargetReserve.GetTargets()){
				if(!target.IsActivated()){
					target.SetTier(
						tierData
					);
				}else{
					target.SetTargetTierDataOnQueue(
						tierData
					);
				}
			}
		}
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
	public class TargetTierData{
		public TargetTierData(
			Material mat,
			TargetData data
		){
			material = mat;
			targetData = data;
		}
		public Material material;
		public TargetData targetData;
	}
}
