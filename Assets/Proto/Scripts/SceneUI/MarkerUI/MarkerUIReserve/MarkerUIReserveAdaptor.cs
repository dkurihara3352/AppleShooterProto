using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBase{
	public interface IMarkerUIReserveAdaptor: IMonoBehaviourAdaptor{
		IMarkerUIReserve GetMarkerUIReserve();
	}
	public class MarkerUIReserveAdaptor : MonoBehaviourAdaptor, IMarkerUIReserveAdaptor {
		protected IMarkerUIReserve thisReserve;
		public IMarkerUIReserve GetMarkerUIReserve(){
			return thisReserve;
		}
		public override void SetUp(){
			thisReserve = CreateReserve();
			thisCanvas = CollectCanvas();
			thisMarkerUIAdaptors = CreateMarkerUIAdaptors();
		}
		protected virtual IMarkerUIReserve CreateReserve(){
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
		}
		Canvas thisCanvas;
		Canvas CollectCanvas(){
			return this.transform.GetComponentInParent<Canvas>();
		}
		IMarkerUIAdaptor[] thisMarkerUIAdaptors;
		IMarkerUIAdaptor[] CreateMarkerUIAdaptors(){
			List<IMarkerUIAdaptor> resultList = new List<IMarkerUIAdaptor>();
			for(int i = 0; i < markerUIsCount; i++){
				GameObject markerUIGO = GameObject.Instantiate(
					markerUIPrefab
				);
				IMarkerUIAdaptor markerUIAdaptor = (IMarkerUIAdaptor)markerUIGO.GetComponent(typeof(IMarkerUIAdaptor));
				markerUIAdaptor.SetIndex(i);
				markerUIAdaptor.SetMarkerUIReserve(thisReserve);
				markerUIAdaptor.SetCamera(uiCamera);
				markerUIAdaptor.SetCanvas(thisCanvas);

				markerUIAdaptor.SetUp();

				resultList.Add(markerUIAdaptor);
			}
			return resultList.ToArray();
		}
		IMarkerUI[] CreateMarkerUIs(){
			List<IMarkerUI> resultList = new List<IMarkerUI>();
			foreach(IMarkerUIAdaptor adaptor in thisMarkerUIAdaptors)
				resultList.Add(adaptor.GetMarkerUI());
			return resultList.ToArray();
		}
	}

}
