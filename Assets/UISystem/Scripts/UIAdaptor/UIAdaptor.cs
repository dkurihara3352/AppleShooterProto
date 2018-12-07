using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem{
	public interface IUIAdaptor: IUISystemMonoBehaviourAdaptor {

		IUIElement GetUIElement();
		IUIElement GetParentUIElement();
		IUIElement[] GetAllOffspringUIElements();
		IUIElement[] GetChildUIElements();

		void SetUpCanvasGroupComponent();
		float GetGroupAlpha();
		void SetGroupAlpha(float alpha);

		IUIManager GetUIManager();


		Vector2 GetRectSize();
		void SetRectLength(Vector2 length);

		void SetIndex(int index);
		int GetIndex();

		void SetUpRecursively();
		void SetUpReferenceRecursively();

		void ToggleRaycastTarget(bool blocks);

		Vector2 GetPivotOffset();
	}
	
	[RequireComponent(typeof(RectTransform))]
	public class UIAdaptor: UISystemMonoBehaviourAdaptor, IUIAdaptor, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler{
		RectTransform thisRectTransform;
		/* SetUp */
			public override void SetUp(){
				thisRectTransform = GetComponent<RectTransform>();
				thisUIElement = CreateUIElement();

				if(activationMode == ActivationMode.Alpha)
					SetUpCanvasGroupComponent();
				
			}
			public override void SetUpReference(){
				base.SetUpReference();

				IUIImage uiImage = CreateUIImage();
				thisUIElement.SetUIImage(uiImage);

				IScroller proximateParentScroller = FindProximateParentTypedUIElement<IScroller>();
				thisUIElement.SetProximateParentScroller(proximateParentScroller);
				
				thisInputStateEngine = CreateUIAdaptorInputStateEngine();
			}
			public override void FinalizeSetUp(){
				base.FinalizeSetUp();
				thisUIElement.DeactivateImple();
			}
		/*  */
			IUIAdaptorInputStateEngine CreateUIAdaptorInputStateEngine(){
				UIAdaptorInputStateEngine.IConstArg arg = new UIAdaptorInputStateEngine.ConstArg(
						thisUIManager,
						thisUIElement,
						this, 
						thisUISystemProcessFactory

				);
				return new UIAdaptorInputStateEngine(arg);
			}
			IUIManager thisUIManager{
				get{
					return thisUISystemMonoBehaviourAdaptorManager.GetUIManager();
				}
			}
			public IUIManager GetUIManager(){
				return thisUIManager;
			}
		/* hierarchy */
			public IUIElement GetParentUIElement(){

				Transform parent = transform.parent;
				Component[] comps = parent.GetComponents<Component>();
				foreach(Component comp in comps){
					if(comp is IUIAdaptor)
						return ((IUIAdaptor)comp).GetUIElement();
				}
				return null;
			}
			protected IUIElement thisUIElement;
			public IUIElement GetUIElement(){
				return thisUIElement;
			}
			protected virtual IUIElement CreateUIElement(){
				UIElement.IConstArg arg = new UIElement.ConstArg(
					this,
					activationMode
				);
				return new UIElement(arg);
			}
			public IUIElement[] GetChildUIElements(){
				List<IUIElement> resultList = new List<IUIElement>();
				IUIAdaptor[] childUIAs = GetChildUIAdaptors();
				foreach(IUIAdaptor uia in childUIAs)
					resultList.Add(uia.GetUIElement());
				return resultList.ToArray();
			}
			public IUIElement[] GetAllOffspringUIElements(){
				List<IUIElement> resultList = new List<IUIElement>();
				resultList.AddRange(GetChildUIElements());
				IUIAdaptor[] childUIAdaptors = GetChildUIAdaptors();

				foreach(IUIAdaptor childUIAdaptor in childUIAdaptors)
					resultList.AddRange(childUIAdaptor.GetAllOffspringUIElements());
				return resultList.ToArray();
			}
			IUIAdaptor[] GetChildUIAdaptors(){
				List<IUIAdaptor> resultList = new List<IUIAdaptor>();
				int childCount = transform.childCount;
				for(int i = 0; i < childCount; i ++){
					Transform child = transform.GetChild(i);
					Component[] components = child.GetComponents<Component>();
					foreach(Component comp in components){
						if(comp is IUIAdaptor){
							IUIAdaptor adaptor = (IUIAdaptor)comp;
							resultList.Add(adaptor);
						}
					}
				}
				return resultList.ToArray();

			}
			protected T FindProximateParentTypedUIElement<T>() where T: class, IUIElement{
				IProximateParentTypedUIECalculator<T> calculator = new ProximateParentTypedUIECalculator<T>(this.GetUIElement());
				return calculator.Calculate();
			}
			protected IUIAdaptor thisParentUIAdaptor{
				get{
					return CollectParentUIAdaptor();
				}
			}
			IUIAdaptor CollectParentUIAdaptor(){
				Transform parent = transform.parent;
				Component[] comps = parent.GetComponents<Component>();
				foreach(Component comp in comps)
					if(comp is IUIAdaptor)
						return (IUIAdaptor)comp;
				return null;
			}
		/* UIImage */
			float thisChangeColorTime = .1f;
			protected virtual IUIImage CreateUIImage(){
				Image image;
				Transform childWithImage = GetChildWithImage(out image);
				// RectTransform imageRectTrans = childWithImage.GetComponent<RectTransform>();
				// if(imageRectTrans == null)
				// 	throw new System.InvalidOperationException("image transform must have RectTransform component");
				// imageRectTrans.pivot = new Vector2(0f, 0f);
				// imageRectTrans.anchorMin = new Vector2(0f, 0f);
				// imageRectTrans.anchorMax = new Vector2(1f, 1f);
				// imageRectTrans.sizeDelta = Vector2.zero;
				// imageRectTrans.anchoredPosition = Vector3.zero;
				IUIImage uiImage = new UIImage(
					image, 
					childWithImage, 
					thisImageDefaultBrightness, 
					thisImageDarkenedBrightness, 
					thisUISystemProcessFactory,
					thisChangeColorTime
				);
				return uiImage;
			}
			protected Transform GetChildWithImage(out Image image){
				for(int i = 0; i < transform.childCount; i ++){
					Transform child = transform.GetChild(i);
					Image thisImage = child.GetComponent<Image>();
					if(thisImage != null){
						image = thisImage;
						return child;
					}
				}
				throw new System.InvalidOperationException("there's no child transform with Image component asigned");
			}
			public float thisImageDefaultBrightness = .8f;
			public float thisImageDarkenedBrightness = .5f;

		/* Activation */
			public ActivationMode activationMode;
			public void SetUpCanvasGroupComponent(){
				if(thisCanvasGroup == null){
					CanvasGroup canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
					canvasGroup.alpha = 0f;
					thisCanvasGroup = canvasGroup;
				}
			}
			protected CanvasGroup thisCanvasGroup = null;
			public float GetGroupAlpha(){
				return thisCanvasGroup.alpha;
			}
			public void SetGroupAlpha(float alpha){
				thisCanvasGroup.alpha = alpha;
			}
		/* MB adaptor */

			public Rect GetRect(){
				return ((RectTransform)this.GetComponent<RectTransform>()).rect;
			}
			public void SetRectLength(Vector2 length){
				RectTransform rt = (RectTransform)this.transform;
				// rt.sizeDelta = length;
				rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length.x);
				rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length.y);
			}
			public override void SetLocalPosition(Vector3 pos){
				SetBottomLeftLocalPosition(pos);
			}
			void SetBottomLeftLocalPosition(Vector2 position){
				Vector2 result = position + GetPivotOffset() - GetParentPivotOffset();
				// SetLocalPosition(result);
				thisRectTransform.localPosition = result;
			}
			Vector2 GetParentPivotOffset(){
				RectTransform parentRT = (RectTransform)transform.parent;
				Vector2 rectSize = parentRT.rect.size;
				Vector2 pivot = parentRT.pivot;
				return new Vector2(
					(rectSize.x * pivot.x),
					(rectSize.y * pivot.y)
				);
			}
			public override Vector3 GetLocalPosition(){
				return GetBottomLeftLocalPosition();
			}
			Vector2 GetBottomLeftLocalPosition(){
				Vector2 actualLocalPos = this.transform.localPosition;
				return GetParentPivotOffset() + actualLocalPos - GetPivotOffset();
			}
			public Vector2 GetPivotOffset(){
				Vector2 rectSize = GetRectSize();
				Vector2 pivot = thisRectTransform.pivot;
				return new Vector2(
					(rectSize.x * pivot.x),
					(rectSize.y * pivot.y)
				);
			}
			public void SetParentUIA(IUIAdaptor parentUIA, bool worldPositionStays){
				this.transform.SetParent(parentUIA.GetTransform(), worldPositionStays);
			}
			public Vector2 GetRectSize(){
				return GetRect().size;
			}
		/* Event System Imple */
			IUIAdaptorInputStateEngine thisInputStateEngine;
			bool PointerIDMatchesTheRegistered(int pointerId){
				return thisUIManager.registeredID == pointerId;
			}
			public virtual void OnPointerEnter(PointerEventData eventData){
				if(thisUIManager != null)
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
						thisInputStateEngine.OnPointerEnter(customEventData);
					}
				}
			}
			public void OnPointerExit(PointerEventData eventData){
				if(thisUIManager != null)
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
						thisInputStateEngine.OnPointerExit(customEventData);
					}
				}
			}
			public void OnPointerDown(PointerEventData eventData){
				if(!thisUIManager.TouchIDIsRegistered()){
					thisUIManager.RegisterTouchID(eventData.pointerId);
					ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
					thisInputStateEngine.OnPointerDown(customEventData);
				}
			}
			public void OnPointerUp(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
						thisInputStateEngine.OnPointerUp(customEventData);
						thisUIManager.UnregisterTouchID();
					}
				}
			}
			public void OnBeginDrag(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
						thisInputStateEngine.OnBeginDrag(customEventData);
					}
				}
			}
			public void OnDrag(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(eventData, Time.deltaTime);
						thisInputStateEngine.OnDrag(customEventData);
					}
				}
			}
		/*  */
			protected int thisIndex = -1;
			public void SetIndex(int index){
				thisIndex = index;
			}
			public int GetIndex(){
				return thisIndex;
			}
			public void SetUpRecursively(){
				this.SetUp();
				foreach(IUIAdaptor childUIAdaptor in GetChildUIAdaptors())
					childUIAdaptor.SetUpRecursively();
			}
			public void SetUpReferenceRecursively(){
				this.SetUpReference();
				foreach(IUIAdaptor childUIAdaptor in GetChildUIAdaptors())
					childUIAdaptor.SetUpReferenceRecursively();
			}
			public virtual void ToggleRaycastTarget(bool isTarget){
				IUIImage image = thisUIElement.GetUIImage();
				image.ToggleRaycastTarget(isTarget);
			}
	}
}
