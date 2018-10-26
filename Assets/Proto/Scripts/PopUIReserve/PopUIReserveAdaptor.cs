using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IPopUIReserveAdaptor: IMonoBehaviourAdaptor{
		RectTransform GetRectTransform();
		IPopUIReserve GetPopUIReserve();
	}
	[RequireComponent(typeof(RectTransform))]
	public class PopUIReserveAdaptor : MonoBehaviourAdaptor, IPopUIReserveAdaptor {

		public override void SetUp(){
			PopUIReserve.IConstArg arg = new PopUIReserve.ConstArg(
				this
			);
			thisReserve = new PopUIReserve(arg);

			thisRectTransform = CollectRectTransform();
			thisCanvas = CollectCanvas();
		}
		IPopUIReserve thisReserve;
		public IPopUIReserve GetPopUIReserve(){
			return thisReserve;
		}
		public override void SetUpReference(){
			IPopUI[] popUIs = CreatePopUIs();
			thisReserve.SetPopUIs(popUIs);
		}
		public override void FinalizeSetUp(){
			foreach(IInstatiablePopUIAdaptor adaptor in thisPopUIAdaptors){
				adaptor.SetCanvas(thisCanvas);
				adaptor.FinalizeSetUp();
			}
		}
		public int popUICount;
		public GameObject popUIPrefab;
		public Camera uiCamera;
		IPopUIAdaptor[] thisPopUIAdaptors;
	
		IPopUI[] CreatePopUIs(){
			List<IPopUI> resultList = new List<IPopUI>();
			List<IPopUIAdaptor> popUIAdaptorList = new List<IPopUIAdaptor>();
			for(int i = 0; i < popUICount; i++){
				GameObject popUIGO = GameObject.Instantiate(
					popUIPrefab
				);
				IInstatiablePopUIAdaptor popUIAdaptor = (IInstatiablePopUIAdaptor)popUIGO.GetComponent(typeof(IInstatiablePopUIAdaptor));
				if(popUIAdaptor == null)
					throw new System.InvalidOperationException(
						"popUIAdaptor not set to prefab"
					);
				popUIAdaptor.SetIndex(i);
				popUIAdaptor.SetMonoBehaviourAdaptorManager(thisMonoBehaviourAdaptorManager);
				popUIAdaptor.SetPopUIReserveAdaptor(this);
				popUIAdaptor.SetCamera(uiCamera);

				popUIAdaptor.SetUp();
				popUIAdaptor.SetUpReference();

				IPopUI popUI = popUIAdaptor.GetPopUI();
				resultList.Add(popUI);

				popUIAdaptorList.Add(popUIAdaptor);
			}
			thisPopUIAdaptors = popUIAdaptorList.ToArray();
			return resultList.ToArray();
		}
		RectTransform thisRectTransform;
		public RectTransform GetRectTransform(){
			return thisRectTransform;
		}
		RectTransform CollectRectTransform(){
			return this.GetComponent<RectTransform>();
		}
		Canvas thisCanvas;
		Canvas CollectCanvas(){
			return this.transform.GetComponentInParent<Canvas>();
		}
	}
}
