using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityBase;
namespace AppleShooterProto{
	public interface ILandedArrow: IAppleShooterSceneObject, IActivationStateHandler, IActivationStateImplementor{
		void SetLandedArrowReserve(ILandedArrowReserve reserve);
		void ActivateAt(
			IArrowHitDetector detector,
			Vector3 position,
			Quaternion rotation
		);

		IArrowHitDetector GetHitDetector();
		int GetIndex();
		void SetArrowTwang(IArrowTwang twang);
	}
	public class LandedArrow: AppleShooterSceneObject, ILandedArrow{
		public LandedArrow(
			IConstArg arg
		): base(
			arg
		){
			thisIndex = arg.index;
			thisActivationStateEngine = new ActivationStateEngine(this);
		}
		ILandedArrowReserve thisReserve;
		ILandedArrowAdaptor thisTypedAdaptor{
			get{
				return (ILandedArrowAdaptor)thisAdaptor;
			}
		}
		public void SetLandedArrowReserve(ILandedArrowReserve reserve){
			thisReserve = reserve;
		}
		public void ActivateAt(
			IArrowHitDetector detector,
			Vector3 position,
			Quaternion rotation
		){
			
			SetHitDetector(
				detector,
				position,
				rotation
			);
			Activate();
		}
		IArrowHitDetector thisDetector;
		void SetHitDetector(
			IArrowHitDetector detector,
			Vector3 position,
			Quaternion rotation
		){
			RemoveSelfFromCurrentDetector();
			thisDetector = detector;
			AddSelfToCurrentDetector();

			SetParent(detector);
			ResetLocalTransform();
			SetPosition(position);
			SetRotation(rotation);
		}
		void RemoveSelfFromCurrentDetector(){
			if(thisDetector != null)
				thisDetector.RemoveLandedArrow(this);
		}
		void AddSelfToCurrentDetector(){
			if(thisDetector != null)
				thisDetector.AddLandedArrow(this);
		}
		public IArrowHitDetector GetHitDetector(){
			return thisDetector;
		}
		int thisIndex;
		public int GetIndex(){
			return thisIndex;
		}
		public void SetArrowTwang(IArrowTwang twang){
			thisArrowTwang = twang;
		}
		IArrowTwang thisArrowTwang;
		/* ActivationState */
			IActivationStateEngine thisActivationStateEngine;
			public void Activate(){
				thisActivationStateEngine.Activate();
			}
			public void Deactivate(){
				thisActivationStateEngine.Deactivate();
			}
			public bool IsActivated(){
				return thisActivationStateEngine.IsActivated();
			}
			public void ActivateImple(){
				thisArrowTwang.Twang();
			}
			public void DeactivateImple(){
				thisArrowTwang.StopTwang();
				RemoveSelfFromCurrentDetector();
				thisReserve.Reserve(this);
			}
		/* Const */
			public new interface IConstArg: AppleShooterSceneObject.IConstArg{
				int index{get;}
			}
			public new class ConstArg: AppleShooterSceneObject.ConstArg, IConstArg{
				public ConstArg(
					ILandedArrowAdaptor adaptor,
					int index
				): base(
					adaptor
				){
					thisIndex = index;
				}
				readonly int thisIndex;
				public int index{get{return thisIndex;}}
			}
		/*  */
	}
}
