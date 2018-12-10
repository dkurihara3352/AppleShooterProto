using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UnityBase{
	public interface IPopUIReserveAdaptor: IMonoBehaviourAdaptor{
		IPopUIReserve GetPopUIReserve();
	}
	[RequireComponent(typeof(RectTransform))]
	public class PopUIReserveAdaptor : MonoBehaviourAdaptor, IPopUIReserveAdaptor {

		public override void SetUp(){
			thisReserve = CreatePopUIReserve();
			thisCanvas = CollectCanvas();
			thisPopUIAdaptors = CreatePopUIAdaptors();
		}
		IPopUIReserve thisReserve;
		public IPopUIReserve GetPopUIReserve(){
			return thisReserve;
		}
		IPopUIReserve CreatePopUIReserve(){
			PopUIReserve.IConstArg arg = new PopUIReserve.ConstArg(
				this
			);
			return new PopUIReserve(arg);

		}
		public override void SetUpReference(){
			IPopUI[] popUIs = CreatePopUIs();
			thisReserve.SetSceneObjects(popUIs);
		}
		public int popUICount;
		public GameObject popUIPrefab;
		public Camera uiCamera;
		Canvas thisCanvas;
		Canvas CollectCanvas(){
			return this.transform.GetComponentInParent<Canvas>();
		}
		IPopUIAdaptor[] thisPopUIAdaptors;
		IPopUIAdaptor[] CreatePopUIAdaptors(){
			List<IPopUIAdaptor> resultList = new List<IPopUIAdaptor>();
			for(int i = 0; i < popUICount; i++){
				GameObject popUIGO = GameObject.Instantiate(
					popUIPrefab
				);
				popUIGO.transform.localScale = thisCanvas.transform.localScale;
				IPopUIAdaptor popUIAdaptor = (IPopUIAdaptor)popUIGO.GetComponent(typeof(IPopUIAdaptor));
				if(popUIAdaptor == null)
					throw new System.InvalidOperationException(
						"popUIAdaptor not set to prefab"
					);
				popUIAdaptor.SetIndex(i);
				popUIAdaptor.SetPopUIReserve(thisReserve);
				popUIAdaptor.SetCamera(uiCamera);
				popUIAdaptor.SetCanvas(thisCanvas);

				popUIAdaptor.SetUp();

				resultList.Add(popUIAdaptor);
			}
			return resultList.ToArray();
		}
	
		IPopUI[] CreatePopUIs(){
			List<IPopUI> resultList = new List<IPopUI>();
			foreach(IPopUIAdaptor adaptor in thisPopUIAdaptors)
				resultList.Add(adaptor.GetPopUI());
			return resultList.ToArray();
		}
	}
}
