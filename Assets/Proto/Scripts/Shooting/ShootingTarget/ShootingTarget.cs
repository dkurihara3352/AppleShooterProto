using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTarget{
		void Hit(IArrow arrow);
		void SetPosition(Vector3 position);
		Vector3 GetPosition();
		void SetRotation(Quaternion rotation);
		void SetParent(Transform parent);
		Transform GetTransform();
		void AddLandedArrow(ILandedArrow landedArrow);
		void RemoveLandedArrow(ILandedArrow landedArrow);
		void ReserveAllLandedArrow();
		void SetIndex(int index);
		int GetIndex();

		bool IsActivated();
		void Activate();
		void Deactivate();
		void ActivateImple();
		void DeactivateImple();

	}
	public abstract class AbsShootingTarget : IShootingTarget {
		public AbsShootingTarget(
			IConstArg arg
		){
			thisOriginalHealth = arg.health;
			thisHealth = thisOriginalHealth;
			thisAdaptor = arg.adaptor;
			thisActivationStateEngine = new ShootingTargetActivationStateEngine(this);
		}
		protected float thisOriginalHealth;
		protected float thisHealth;
		protected readonly IShootingTargetAdaptor thisAdaptor;
		/* Activation */
			IShootingTargetActivationStateEngine thisActivationStateEngine;
			public bool IsActivated(){
				return thisActivationStateEngine.IsActivated();
			}
			public void Activate(){
				thisActivationStateEngine.Activate();
			}
			public virtual void ActivateImple(){
				thisHealth = thisOriginalHealth;
			}
			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public virtual void DeactivateImple(){
				ReserveAllLandedArrow();
				ResetTransformAtReserve();
			}
		/*  */
		public void Hit(
			IArrow arrow
		){
			float attack = arrow.GetAttack();
			thisHealth -= attack;
			if(thisHealth <= 0f){
				Debug.Log(
					"destroyed"
				);
				DestroyTarget();
			}
			else{
				Debug.Log(
					"hit " + ", " +
					"health is " + thisHealth.ToString()
				);
				IndicateHealth(
					thisHealth,
					attack
				);
				IndicateHit(attack);
			}
		}
		protected virtual void DestroyTarget(){
			Deactivate();
		}

		protected abstract void IndicateHealth(
			float health,
			float delta
		);
		protected abstract void IndicateHit(
			float delta
		);
		public void SetPosition(Vector3 position){
			thisAdaptor.SetPosition(position);
		}
		public Vector3 GetPosition(){
			return thisAdaptor.GetPosition();
		}
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public void SetParent(Transform parent){
			thisAdaptor.SetParent(parent);
		}
		Transform thisReserveTransform;
		protected virtual void ResetTransformAtReserve(){
			thisAdaptor.ResetTransformAtReserve();
		}
		public Transform GetTransform(){
			return thisAdaptor.GetTransform();
		}
		List<ILandedArrow> thisLandedArrows = new List<ILandedArrow>();
		public void AddLandedArrow(ILandedArrow arrow){
			thisLandedArrows.Add(arrow);
		}
		public void RemoveLandedArrow(ILandedArrow arrow){
			if(thisLandedArrows.Contains(arrow))
				thisLandedArrows.Remove(arrow);
		}
		public void ReserveAllLandedArrow(){
			List<ILandedArrow> temp = new List<ILandedArrow>(thisLandedArrows);
			foreach(ILandedArrow arrow in temp)
				if(arrow != null)
					arrow.Reserve();
		}
		int thisIndex;
		public virtual void SetIndex(int index){
			thisIndex = index;
		}
		public int GetIndex(){
			return thisIndex;
		}
		/*  */
		public interface IConstArg{
			float health{get;}
			IShootingTargetAdaptor adaptor{get;}
		}
		public struct ConstArg: IConstArg{
			public ConstArg(
				float health,
				IShootingTargetAdaptor adaptor
			){
				thisHealth = health;
				thisAdaptor = adaptor;
			}
			readonly float thisHealth;
			public float health{get{return thisHealth;}}
			readonly IShootingTargetAdaptor thisAdaptor;
			public IShootingTargetAdaptor adaptor{get{return thisAdaptor;}}
		}
	}

}
