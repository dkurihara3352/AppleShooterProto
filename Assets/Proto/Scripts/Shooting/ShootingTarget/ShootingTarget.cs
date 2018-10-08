using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTarget{
		void Hit(IArrow arrow);
	}
	public abstract class AbsShootingTarget : IShootingTarget {
		public AbsShootingTarget(
			IShootingTargetConstArg arg
		){
			thisHealth = arg.health;
		}
		protected float thisHealth;
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
	}

	public interface IShootingTargetConstArg{
		float health{get;}
	}
	public struct ShootingTargetConstArg: IShootingTargetConstArg{
		public ShootingTargetConstArg(
			float health
		){
			thisHealth = health;
		}
		readonly float thisHealth;
		public float health{get{return thisHealth;}}
	}
}
