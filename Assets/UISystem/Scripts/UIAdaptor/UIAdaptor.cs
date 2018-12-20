using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityBase;

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
		void SetRectSize(Vector2 size);

		void SetIndex(int index);
		int GetIndex();

		void SetUpRecursively();
		void SetUpReferenceRecursively();

		void ToggleRaycastTarget(bool blocks);

		Vector2 GetPivotOffset();

		void RecalculateRect();
	}
	
	[RequireComponent(typeof(RectTransform))]
	public class UIAdaptor: /* UISystemMonoBehaviourAdaptor */UnityEngine.EventSystems.UIBehaviour, IUIAdaptor, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler{
		protected override void Awake(){
			base.Awake();
			thisMonoBehaviourAdaptorManager = FindAndSetMonoBehaviourAdaptorManager();
			thisMonoBehaviourAdaptorManager.AddAdaptor(this);
		}
		IMonoBehaviourAdaptorManager FindAndSetMonoBehaviourAdaptorManager(){
			IMonoBehaviourAdaptorManager result = (IMonoBehaviourAdaptorManager)GameObject.Find("MonoBehaviourAdaptorManagerGO").GetComponent(typeof(IMonoBehaviourAdaptorManager));
			if(result == null)
				throw new System.InvalidOperationException(
					"MonoBehaviourManager is not found"
				);
			return result;
		}
		public IMonoBehaviourAdaptorManager GetMonoBehaviourAdaptorManager(){
			return thisMonoBehaviourAdaptorManager;
		}
		IMonoBehaviourAdaptorManager thisMonoBehaviourAdaptorManager;
		
		/* SetUp */
			RectTransform thisRectTransform;
			Canvas thisCanvas{
				get{
					return thisUIManager.GetCanvas();
				}
			}
		
			public virtual void SetUp(){
				// Debug.Log(GetName() + DKUtility.DebugHelper.StringInColor(" SetUp", Color.red));
				thisRectTransform = GetComponent<RectTransform>();
				thisUIElement = CreateUIElement();

				if(activationMode == ActivationMode.Alpha)
					SetUpCanvasGroupComponent();
				
			}
			public virtual void SetUpReference(){
				// Debug.Log(GetName() + DKUtility.DebugHelper.StringInColor(" SetUpRef", Color.green));

				IUIImage uiImage = CreateUIImage();
				thisUIElement.SetUIImage(uiImage);

				IScroller proximateParentScroller = FindProximateParentTypedUIElement<IScroller>();
				thisUIElement.SetProximateParentScroller(proximateParentScroller);
				
				thisInputStateEngine = CreateUIAdaptorInputStateEngine();
				thisSetUpRefIsDone = true;
			}
			public virtual void FinalizeSetUp(){
				// Debug.Log(GetName() + DKUtility.DebugHelper.StringInColor(" Fin", Color.blue));
				thisUIElement.DeactivateImple();
				thisIsSetUp = true;
			}
			bool thisIsSetUp = false;
		/* accessing managers */
			public IUnityBaseProcessFactory GetProcessFactory(){
				return thisProcessFactory;
			}
			IUnityBaseProcessFactory thisProcessFactory{
				get{
					return (IUnityBaseProcessFactory)thisMonoBehaviourAdaptorManager.GetProcessFactory();
				}
			}
			IUISystemProcessFactory thisUISystemProcessFactory{
				get{
					return (IUISystemProcessFactory)thisProcessFactory;
				}
			}
			public IUISystemMonoBehaviourAdaptorManager GetUISystemMonoBehaviourAdaptorManager(){
				return thisUISystemMonoBehaviourAdaptorManager;
			}
			IUISystemMonoBehaviourAdaptorManager thisUISystemMonoBehaviourAdaptorManager{
				get{
					return (IUISystemMonoBehaviourAdaptorManager)thisMonoBehaviourAdaptorManager;
				}
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
			/* general */
				public Transform GetTransform(){
					return this.transform;
				}
				public void SetParentUIA(IUIAdaptor parentUIA, bool worldPositionStays){
					this.transform.SetParent(parentUIA.GetTransform(), worldPositionStays);
				}
				public void SetParent(Transform parent){
					this.transform.SetParent(parent, true);
				}
				public void ResetLocalTransform(){
					this.transform.localPosition = Vector3.zero;
					this.transform.localRotation = Quaternion.identity;
				}
				public Vector3 GetForwardDirection(){
					return this.transform.forward;
				}
				public bool IsEnabled(){
					return this.enabled;
				}

				public string GetName(){
					return this.name;
				}
				public void SetName(string name){
					this.name = name;
				}
			/* Rect */
				protected Vector2 GetScaledSize(Vector2 nonscaledSize){
					return new Vector2(
						nonscaledSize.x * thisCanvasLocalScale.x,
						nonscaledSize.y * thisCanvasLocalScale.y
					);
				}
				protected Vector2 GetNonscaledSize(Vector2 scaledSize){
					return new Vector2(
						scaledSize.x / thisCanvasLocalScale.x,
						scaledSize.y / thisCanvasLocalScale.y
					);
				}
				public Rect GetRect(){
					return thisRectTransform.rect;
				}
				public Vector2 GetRectSize(){
					return GetRect().size;
				}
				Vector2 GetPixelRectSize(){
					Vector2 scaledRectSize = GetRectSize();
					return GetScaledSize(scaledRectSize);
				}
				public void SetRectSize(Vector2 length){
					RectTransform rt = (RectTransform)this.transform;
					rt.SetSizeWithCurrentAnchors(
						RectTransform.Axis.Horizontal, 
						length.x
					);
					rt.SetSizeWithCurrentAnchors(
						RectTransform.Axis.Vertical, 
						length.y
					);
				}
				void SetPixelRectSize(Vector2 pixelSize){
					Vector2 nonscaledSize = GetNonscaledSize(pixelSize);
					SetRectSize(nonscaledSize);
				}

			/* Canvas */
				protected Vector3 thisCanvasLocalScale{
					get{
						return thisCanvas.transform.localScale;
					}
				}
			/* position */
				protected Vector3 GetScaledPosition(Vector3 nonscaledPosition){
					return new Vector3(
						nonscaledPosition.x * thisCanvasLocalScale.x,
						nonscaledPosition.y * thisCanvasLocalScale.y,
						nonscaledPosition.z * thisCanvasLocalScale.z
					);
				}
				protected Vector3 GetNonscaledPosition(Vector3 pixelPosition){
					return new Vector3(
						pixelPosition.x / thisCanvasLocalScale.x,
						pixelPosition.y / thisCanvasLocalScale.y,
						pixelPosition.z / thisCanvasLocalScale.z
					);
				}
				public Vector3 GetPosition(){
					return this.transform.position;
				}
				public void SetPosition(Vector3 position){
					this.transform.position = position;
				}
				public void SetLocalPosition(Vector3 pos){
					SetBottomLeftLocalPosition(pos);
				}
				void SetPixelLocalPosition(Vector3 position){
					Vector3 unscaledPosition = GetNonscaledPosition(position);
					SetLocalPosition(unscaledPosition);
				}
				Vector2 GetPixelLocalPosition(){
					return GetScaledPosition(
						GetLocalPosition()
					);
				}
				void SetBottomLeftLocalPosition(Vector2 position){
					Vector2 result = position + GetPivotOffset() - GetParentPivotOffset();
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
				public Vector3 GetLocalPosition(){
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
			/* rotation */
				public Quaternion GetRotation(){
					return this.transform.rotation;
				}
				public void SetRotation(Quaternion rotation){
					this.transform.rotation = rotation;
				}
				public Quaternion GetLocalRotation(){
					return this.transform.localRotation;
				}
				public void SetLocalRotation(Quaternion localRotation){
					this.transform.localRotation = localRotation;
				}
				public void Rotate(Vector3 euler){
					this.transform.eulerAngles = euler;
				}
				public void Rotate(
					float angleOnAxis,
					int axis
				){
					Vector3 original = this.transform.localEulerAngles;
					if(axis == 0)
						this.transform.localEulerAngles = new Vector3(
							angleOnAxis,
							original.y,
							original.z					
						);
					if(axis == 1)
						this.transform.localEulerAngles = new Vector3(
							original.x,
							angleOnAxis,
							original.z
						);
				}
				public void SetLookRotation(
					Vector3 forward,
					Vector3 up
				){
					if(forward != Vector3.zero)
						this.transform.rotation = Quaternion.LookRotation(
							forward,
							up
						);
				}
				public void SetLookRotation(Vector3 forward){
					this.SetLookRotation(
						forward,
						this.transform.up
					);
				}
			/* scale */
				public void ResetScale(){
					transform.localScale = Vector3.one;
				}
				public Vector3 GetLocalScale(){
					return transform.localScale;
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
						ICustomEventData customEventData = new CustomEventData(
							eventData, 
							Time.deltaTime,
							thisUIManager
						);
						thisInputStateEngine.OnPointerEnter(customEventData);
					}
				}
			}
			public void OnPointerExit(PointerEventData eventData){
				if(thisUIManager != null)
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(
							eventData, 
							Time.deltaTime,
							thisUIManager
						);
						thisInputStateEngine.OnPointerExit(customEventData);
					}
				}
			}
			public void OnPointerDown(PointerEventData eventData){
				if(!thisUIManager.TouchIDIsRegistered()){
					thisUIManager.RegisterTouchID(eventData.pointerId);
					ICustomEventData customEventData = new CustomEventData(
						eventData, 
						Time.deltaTime,
						thisUIManager
					);
					thisInputStateEngine.OnPointerDown(customEventData);
				}
			}
			public void OnPointerUp(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(
							eventData, 
							Time.deltaTime,
							thisUIManager
						);
						thisInputStateEngine.OnPointerUp(customEventData);
						thisUIManager.UnregisterTouchID();
					}
				}
			}
			public void OnBeginDrag(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(
							eventData, 
							Time.deltaTime,
							thisUIManager
						);
						thisInputStateEngine.OnBeginDrag(customEventData);
					}
				}
			}
			public void OnDrag(PointerEventData eventData){
				if(thisUIManager.TouchIDIsRegistered()){
					if(PointerIDMatchesTheRegistered(eventData.pointerId)){
						ICustomEventData customEventData = new CustomEventData(
							eventData, 
							Time.deltaTime,
							thisUIManager
						);
						thisInputStateEngine.OnDrag(customEventData);
					}
				}
			}
		/*  */
			bool thisSetUpRefIsDone = false;
			protected override void OnRectTransformDimensionsChange(){
				base.OnRectTransformDimensionsChange();
				if(thisSetUpRefIsDone){
					// Debug.Log(
					// 	GetName() + " is recalculating"
					// );
					RecalculateRect();
				}
			}
			public virtual void RecalculateRect(){
				// Debug.Log(GetName() + " is recaluc'ed");
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
