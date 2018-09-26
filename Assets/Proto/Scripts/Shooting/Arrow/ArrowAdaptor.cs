using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowAdaptor: IMonoBehaviourAdaptor{
		void SetProcessManager(IProcessManager processManager);
		void SetArrowReserve(IMonoBehaviourAdaptor arrowReserve);
		void SetLaunchPointAdaptor(ILaunchPointAdaptor launchPointAdaptor);

		IArrow GetArrow();
		void BecomeChildToLaunchPoint();
		void BecomeChildToReserve();
		void ResetTransform();

		void SetIndex(int index);
		string GetParentName();
	}
	public class ArrowAdaptor : MonoBehaviourAdaptor, IArrowAdaptor {
		public void Awake(){
			return;
		}
		/* Ref */
			ILaunchPointAdaptor thisLaunchPointAdaptor;
			public void SetLaunchPointAdaptor(ILaunchPointAdaptor launchPointAdaptor){
				thisLaunchPointAdaptor = launchPointAdaptor;
			}
			IProcessManager thisProcessManager;
			public void SetProcessManager(IProcessManager processManager){
				thisProcessManager = processManager;
			}
			IMonoBehaviourAdaptor thisArrowReserve;
			public void SetArrowReserve(IMonoBehaviourAdaptor reserve){
				thisArrowReserve = reserve;
			}
		/*  */
		public override void SetUp(){
			IAppleShooterProcessFactory processFactory = new AppleShooterProcessFactory(
				thisProcessManager
			);
			IArrowConstArg arg = new ArrowConstArg(
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
				this.transform.SetParent(thisArrowReserve.GetTransform(), true);
			}
			public void ResetTransform(){
				this.transform.localPosition  = Vector3.zero;
				this.transform.localRotation = Quaternion.identity;
			}
		
		/* Debug */
			bool thisIsReadyForGizmo = false;
			public void OnDrawGizmos(){
				if(thisIsReadyForGizmo){
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere(this.GetPosition(), 1f);
				}
			}
			public string GetParentName(){
				return this.transform.parent.ToString();
			}
			int thisIndex;
			public void SetIndex(int index){
				thisIndex = index;
			}
		/*  */
	}
}
