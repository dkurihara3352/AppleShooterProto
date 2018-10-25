using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface ILandedArrowReserveAdaptor: IMonoBehaviourAdaptor{
		ILandedArrowReserve GetLandedArrowReserve();
	}
	public class LandedArrowReserveAdaptor: MonoBehaviourAdaptor, ILandedArrowReserveAdaptor{
		ILandedArrowReserve thisReserve;
		public ILandedArrowReserve GetLandedArrowReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			LandedArrowReserve.IConstArg arg = new LandedArrowReserve.ConstArg(
				this
			);
			thisReserve = new LandedArrowReserve(arg);
		}
		public override void SetUpReference(){
			ILandedArrow[] landedArrows = CreateLandedArrows();
			thisReserve.SetLandedArrows(landedArrows);
		}
		public int totalLandedArrowsCount;
		public GameObject landedArrowPrefab;
		public Camera guiCamera;
		public ProcessManager processManager;
		ILandedArrow[] CreateLandedArrows(){
			List<ILandedArrow> resultList = new List<ILandedArrow>();
			for(int i = 0; i < totalLandedArrowsCount; i ++){
				GameObject landedArrowGO = GameObject.Instantiate(
					landedArrowPrefab,
					Vector3.zero,
					Quaternion.identity
				);
				ILandedArrowAdaptor arrowAdaptor = (ILandedArrowAdaptor)landedArrowGO.GetComponent(typeof(ILandedArrowAdaptor));
				if(arrowAdaptor == null)
					throw new System.InvalidOperationException(
						"landedArrowAdaptor missing"
					);
				arrowAdaptor.SetLandedArrowReserveAdaptor(this);
				IArrowTwangAdaptor arrowTwangAdaptor = /* (IArrowTwangAdaptor)landedArrowGO.GetComponent(typeof(IArrowTwangAdaptor)) */GetArrowTwangAdaptorFromChild(landedArrowGO);
				if(arrowTwangAdaptor == null)
					throw new System.InvalidOperationException(
						"arrow twang adaptor is missing"
					);
				arrowTwangAdaptor.SetProcessManager(processManager);
				arrowAdaptor.SetArrowTwangAdaptor(
					arrowTwangAdaptor
				);


				arrowAdaptor.SetUp();
				arrowTwangAdaptor.SetUp();
				arrowAdaptor.SetUpReference();
				arrowTwangAdaptor.SetUpReference();
				arrowAdaptor.FinalizeSetUp();
				arrowTwangAdaptor.FinalizeSetUp();

				ILandedArrow landedArrow = arrowAdaptor.GetLandedArrow();
				landedArrow.SetIndex(i);
				landedArrow.Reserve();

				resultList.Add(landedArrow);
			}
			return resultList.ToArray();
		}
		IArrowTwangAdaptor GetArrowTwangAdaptorFromChild(GameObject landedArrowGO){
			Component[] components = landedArrowGO.GetComponentsInChildren(typeof(Component));
			foreach(Component component in components){
				if(component is IArrowTwangAdaptor)
					return (IArrowTwangAdaptor)component;
			}
			throw new System.InvalidOperationException(
				"arrowTwangAdaptor is not found among child components"
			);
		}
	}
}


