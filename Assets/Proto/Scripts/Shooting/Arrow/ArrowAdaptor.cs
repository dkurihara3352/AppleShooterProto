﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowAdaptor: IMonoBehaviourAdaptor{
		IArrow GetArrow();

		void SetArrowReserveAdaptor(IArrowReserveAdaptor adaptor);
		void SetLaunchPointAdaptor(ILaunchPointAdaptor adaptor);
		void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor);
		void SetCollisionDetectionIntervalFrameCount(int count);

		void StartCollisionCheck();
		void StopCollisionCheck();

		void SetIndex(int index);
		string GetParentName();
	}
	public class ArrowAdaptor : MonoBehaviourAdaptor, IArrowAdaptor {
		
		/* Ref */
		public void SetCollisionDetectionIntervalFrameCount(int count){
			checkPerEveryThisFrames = count;
		}
		IArrowReserveAdaptor thisArrowReserveAdaptor;
		public void SetArrowReserveAdaptor(IArrowReserveAdaptor adaptor){
			thisArrowReserveAdaptor = adaptor;
		}
		ILaunchPointAdaptor thisLaunchPointAdaptor;
		public void SetLaunchPointAdaptor(ILaunchPointAdaptor adaptor){
			thisLaunchPointAdaptor  = adaptor;
		}
		IShootingManagerAdaptor thisShootingManagerAdaptor;
		public void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor){
			thisShootingManagerAdaptor = adaptor;
		}
		/*  */
		public override void SetUp(){
			thisArrow = CreateArrow();
		}
		IArrow thisArrow;
		public IArrow GetArrow(){
			return thisArrow;
		}
		IArrow CreateArrow(){
			Arrow.IConstArg arg = new Arrow.ConstArg(
				thisIndex,
				this
			);
			thisIsReadyForGizmo = true;
			return new Arrow(arg);
		}
		public override void SetUpReference(){
			IArrowReserve reserve = thisArrowReserveAdaptor.GetArrowReserve();
			thisArrow.SetArrowReserve(reserve);

			ILaunchPoint launchPoint = thisLaunchPointAdaptor.GetLaunchPoint();
			thisArrow.SetLaunchPoint(launchPoint);

			IShootingManager shootingManager = thisShootingManagerAdaptor.GetShootingManager();
			thisArrow.SetShootingManager(shootingManager);
		}
		public override void FinalizeSetUp(){
			thisArrow.Deactivate();
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
							hitPosition = hit.point;
							IShootingTargetAdaptor targetAdaptor = hitTrans.GetComponentInParent(typeof(IShootingTargetAdaptor)) as IShootingTargetAdaptor;
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
