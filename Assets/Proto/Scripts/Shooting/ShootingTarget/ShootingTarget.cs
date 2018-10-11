using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTarget{
		void Hit(IArrow arrow);
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
		void SetParent(Transform parent);
		void ResetTransform();
		void ResetTarget();
	}
	public abstract class AbsShootingTarget : IShootingTarget {
		public AbsShootingTarget(
			IAbsConstArg arg
		){
			thisOriginalHealth = arg.health;
			thisHealth = thisOriginalHealth;
			thisAdaptor = arg.adaptor;
		}
		protected float thisOriginalHealth;
		protected float thisHealth;
		protected readonly IShootingTargetAdaptor thisAdaptor;
		public void Hit(IArrow arrow){
			float attack = arrow.GetAttack();
			thisHealth -= attack;
			if(thisHealth <= 0f){
				Debug.Log(
					"destroyed"
				);
				this.DestroyTarget();
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
		protected abstract void DestroyTarget();
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
		public void SetRotation(Quaternion rotation){
			thisAdaptor.SetRotation(rotation);
		}
		public void SetParent(Transform parent){
			thisAdaptor.SetParent(parent);
		}
		public void ResetTransform(){
			thisAdaptor.ResetTransform();
		}
		public virtual void ResetTarget(){
			thisHealth = thisOriginalHealth;
		}
		/*  */
		public interface IAbsConstArg{
			float health{get;}
			IShootingTargetAdaptor adaptor{get;}
		}
	}

}
