using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IMarkerUIReserveAdaptor: IMonoBehaviourAdaptor{
		IMarkerUIReserve GetMarkerUIReserve();
	}
	public class MarkerUIReserveAdaptor : MonoBehaviourAdaptor, IMarkerUIReserveAdaptor {
		IMarkerUIReserve thisReserve;
		public IMarkerUIReserve GetMarkerUIReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			thisReserve = CreateReserve();
		}
		IMarkerUIReserve CreateReserve(){
			MarkerUIReserve.IConstArg arg = new MarkerUIReserve.ConstArg(
				this
			);
			return new MarkerUIReserve(arg);
		}
		public int markerUIsCount;
		public GameObject markerUIPrefab;
		public Camera uiCamera;
		public override void SetUpReference(){
			IMarkerUI[] markerUIs = CreateMarkerUIs();
			thisReserve.SetSceneObjects(markerUIs);
			thisMarkerUIAdaptors = GetMarkerUIAdaptors(markerUIs);
		}
		IMarkerUI[] CreateMarkerUIs(){
			List<IMarkerUI> resultList = new List<IMarkerUI>();
			for(int i = 0; i < markerUIsCount; i++){
				GameObject markerUIGO = GameObject.Instantiate(
					markerUIPrefab
				);
				IMarkerUIAdaptor markerUIAdaptor = (IMarkerUIAdaptor)markerUIGO.GetComponent(typeof(IMarkerUIAdaptor));
				markerUIAdaptor.SetIndex(i);
				markerUIAdaptor.SetMonoBehaviourAdaptorManager(
					thisMonoBehaviourAdaptorManager
				);
				markerUIAdaptor.SetMarkerUIReserve(thisReserve);
				markerUIAdaptor.SetCamera(uiCamera);

				markerUIAdaptor.SetUp();
				markerUIAdaptor.SetUpReference();

				IMarkerUI markerUI = markerUIAdaptor.GetMarkerUI();
				resultList.Add(markerUI);
			}
			return resultList.ToArray();
		}
		IMarkerUIAdaptor[] GetMarkerUIAdaptors(IMarkerUI[] markerUIs){
			List<IMarkerUIAdaptor> resultList = new List<IMarkerUIAdaptor>();
			foreach(IMarkerUI markerUI in markerUIs)
				resultList.Add((IMarkerUIAdaptor)markerUI.GetAdaptor());
			return resultList.ToArray();
		}
		IMarkerUIAdaptor[] thisMarkerUIAdaptors;
		public override void FinalizeSetUp(){
			foreach(IMarkerUIAdaptor adaptor in thisMarkerUIAdaptors)
				adaptor.FinalizeSetUp();
		}

	}
}
