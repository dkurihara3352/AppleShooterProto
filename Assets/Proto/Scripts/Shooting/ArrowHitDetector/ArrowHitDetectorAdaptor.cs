using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowHitDetectorAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IArrowHitDetector GetArrowHitDetector();
		void ToggleCollider(bool toggle);
	}
	public abstract class ArrowHitDetectorAdaptor: AppleShooterMonoBehaviourAdaptor, IArrowHitDetectorAdaptor{
		protected IArrowHitDetector thisDetector;
		public IArrowHitDetector GetArrowHitDetector(){
			return thisDetector;
		}
		protected abstract IArrowHitDetector CreateArrowHitDetector();
		public override void SetUp(){
			thisDetector = CreateArrowHitDetector();
			thisColliders = CollectColliders();
		}
		Collider[] thisColliders;
		Collider[] CollectColliders(){
			List<Collider> resultList = new List<Collider>();
			Collider[] collidersOnThis = GetCollidersOnThisTransform();
			resultList.AddRange(collidersOnThis);
			Collider[] collidersOnImmediateChildren = GetCollidersOnChildren();
			resultList.AddRange(collidersOnImmediateChildren);
			return resultList.ToArray();
		}
		Collider[] GetCollidersOnThisTransform(){
			List<Collider> resultList = new List<Collider>();
			Component[] comps = GetComponents<Component>();
			foreach(Component comp in comps){
				if(comp != null && comp is Collider)
					resultList.Add((Collider)comp);
			}
			return resultList.ToArray();
		}
		Collider[] GetCollidersOnChildren(){
			List<Collider> resultList = new List<Collider>();
			int childCount = transform.childCount;
			for(int i = 0; i < childCount; i++){
				Transform child = transform.GetChild(i);
				Component[] comps = child.GetComponents<Component>();
				foreach(Component comp in comps){
					if(comp != null && comp is Collider)
						resultList.Add((Collider)comp);
				}
			}
			return resultList.ToArray();
		}
		public void ToggleCollider(bool toggle){
			foreach(Collider collider in thisColliders)
				collider.enabled = toggle;
		}
	}
}
