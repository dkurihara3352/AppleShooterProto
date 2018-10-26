using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppleShooterProto{
	public interface IInstatiablePopUIAdaptor: IPopUIAdaptor{
		void SetIndex(int index);
		void SetMonoBehaviourAdaptorManager(
			IMonoBehaviourAdaptorManager manager
		);
		void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor);
		void SetCamera(Camera camera);
		void SetCanvas(Canvas canvas);
	}
	public class InstatiablePopUIAdaptor: PopUIAdaptor, IInstatiablePopUIAdaptor{
		protected override void Awake(){
			return;
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		public void SetMonoBehaviourAdaptorManager(
			IMonoBehaviourAdaptorManager manager
		){
			thisMonoBehaviourAdaptorManager = manager;
		}
		IPopUIReserveAdaptor thisReserveAdaptor;
		IPopUIReserve thisReserve;
		public void SetPopUIReserveAdaptor(IPopUIReserveAdaptor adaptor){
			thisReserveAdaptor = adaptor;
		}
		public override void SetUpReference(){
			thisReserve = thisReserveAdaptor.GetPopUIReserve();
		}
		public override void ResetAtReserve(){
			RectTransform reserveRectTransform = thisReserve.GetReserveRectTransform();
			SetParent(reserveRectTransform);
			Vector2 localPosition = thisReserve.GetReservedLocalPosition(thisIndex);
			SetLocalPosition(localPosition);
			OnResetAtReserve();
		}
		public void SetCamera(Camera camera){
			uiCamera = camera;
		}
		public void SetCanvas(Canvas canvas){
			thisCanvas = canvas;
		}
	}
}
