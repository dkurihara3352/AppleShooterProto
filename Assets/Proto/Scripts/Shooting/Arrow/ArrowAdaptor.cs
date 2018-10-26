using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowAdaptor: IInstatiableMonoBehaviourAdaptor{
		void SetArrowReserveTransform(Transform arrowReserveTrans);
		void SetLaunchPointAdaptor(ILaunchPointAdaptor launchPointAdaptor);
		void SetCollisionDetectionIntervalFrameCount(int count);

		IArrow GetArrow();
		void BecomeChildToLaunchPoint();
		void BecomeChildToReserve();

		void StartCollisionCheck();
		void StopCollisionCheck();

		void SetIndex(int index);
		string GetParentName();
	}
	public class ArrowAdaptor : InstatiableMonoBehaviourAdaptor, IArrowAdaptor {
		
		/* Ref */
			ILaunchPointAdaptor thisLaunchPointAdaptor;
			public void SetLaunchPointAdaptor(ILaunchPointAdaptor launchPointAdaptor){
				thisLaunchPointAdaptor = launchPointAdaptor;
			}
			Transform thisArrowReserveTrans;
			public void SetArrowReserveTransform(Transform reserveTrans){
				thisArrowReserveTrans = reserveTrans;
			}
			public void SetCollisionDetectionIntervalFrameCount(int count){
				checkPerEveryThisFrames = count;
			}
		/*  */
		public override void SetUp(){
			Arrow.IConstArg arg = new Arrow.ConstArg(
				this,
				processFactory,
				thisIndex
			);
			thisArrow = new Arrow(arg);
			thisIsReadyForGizmo = true;
		}
		/* Action */
			IArrow thisArrow;
			public IArrow GetArrow(){
				return thisArrow;
			}
			public void BecomeChildToLaunchPoint(){
				this.transform.SetParent(thisLaunchPointAdaptor.GetTransform(), true);
			}
			public void BecomeChildToReserve(){
				this.transform.SetParent(thisArrowReserveTrans, true);
			}
		
		/* Debug */
			bool thisIsReadyForGizmo = false;
			public void OnDrawGizmos(){
				if(thisIsReadyForGizmo){
					if(thisChecksForCollision){
						Gizmos.color = Color.red;
						Gizmos.DrawLine(prevForDebug, curForDebug);
					}
					Gizmos.color = Color.magenta;
					Gizmos.DrawWireSphere(hitPosition, 2f);
				}
			}
			public string GetParentName(){
				return this.transform.parent.ToString();
			}
			int thisIndex;
			public void SetIndex(int index){
				thisIndex = index;
			}
		/* Collision Detection */
			int checkPerEveryThisFrames;
			Vector3 thisPrevPosition;
			int layerNumber = 8;
			Vector3 prevForDebug;
			Vector3 curForDebug;
			int count = 0;
			Vector3 hitPosition = Vector3.zero;
			bool thisChecksForCollision = false;
			public void StartCollisionCheck(){
				count = 0;
				thisPrevPosition = GetPosition();
				thisChecksForCollision = true;
			}
			public void StopCollisionCheck(){
				thisChecksForCollision = false;
			}
			public void FixedUpdate(){
				if(thisChecksForCollision){
					count ++;
					if(count > checkPerEveryThisFrames)
						throw new System.InvalidOperationException(
							"fixed update frame skipped?"
						);
					if(count == checkPerEveryThisFrames){
						count = 0;
						Vector3 position = GetPosition();
						RaycastHit hit;
						int layerMask = 1<<(layerNumber);
						bool hasHit = Physics.Linecast(thisPrevPosition, position, out hit, layerMask);
						prevForDebug = thisPrevPosition;
						curForDebug = position;
						thisPrevPosition = position;
						if(hasHit){
							Transform hitTrans = hit.transform;
							Debug.Log(
								"hit detected: " + " "+
								"hit transform: " + hitTrans.name.ToString() + ", " +
								"hit attack: " + thisArrow.GetAttack().ToString()
							);
							hitPosition = hit.point;
							IShootingTargetAdaptor targetAdaptor = hitTrans.GetComponent(typeof(IShootingTargetAdaptor)) as IShootingTargetAdaptor;
							if(targetAdaptor == null)
								throw new System.InvalidOperationException(
									"hitTrans seems not to have IShootingTargetAdaptor"
								);
							Vector3 hitPos = hit.point;
							IShootingTarget target = targetAdaptor.GetShootingTarget();
							target.Hit(thisArrow);
							thisArrow.Land(
								target,
								hitPos
							);
						}
					}
				}
			}
	}
}
