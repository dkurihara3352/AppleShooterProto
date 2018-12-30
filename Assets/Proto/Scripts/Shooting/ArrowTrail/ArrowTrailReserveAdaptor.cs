using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IArrowTrailReserveAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IArrowTrailReserve GetArrowTrailReserve();
		Color GetMinDrawColor();
		Color GetMaxDrawColor();
	}
	public class ArrowTrailReserveAdaptor: AppleShooterMonoBehaviourAdaptor, IArrowTrailReserveAdaptor{
		public Color minDrawColor = Color.white;
		public Color GetMinDrawColor(){
			return minDrawColor;
		}
		public Color maxDrawColor = Color.red;
		public Color GetMaxDrawColor(){
			return maxDrawColor;
		}
		public override void SetUp(){
			thisReserve = CreateArrowTrailReserve();
			thisAdaptors = CollectArrowTrailAdaptors();
		}
		IArrowTrailReserve thisReserve;
		public IArrowTrailReserve GetArrowTrailReserve(){
			return thisReserve;
		}
		IArrowTrailReserve CreateArrowTrailReserve(){
			ArrowTrailReserve.IConstArg arg = new ArrowTrailReserve.ConstArg(
				this
			);
			return new ArrowTrailReserve(arg);
		}

		IArrowTrailAdaptor[] thisAdaptors;
		public GameObject trailAdaptorPrefab;
		public int numToCreate;
		IArrowTrailAdaptor[] CollectArrowTrailAdaptors(){
			List<IArrowTrailAdaptor> resultList = new List<IArrowTrailAdaptor>(numToCreate);
			for(int i = 0 ; i < numToCreate; i ++){
				GameObject trailGO = GameObject.Instantiate(
					trailAdaptorPrefab
				);
				trailGO.transform.SetParent(this.GetTransform());
				IArrowTrailAdaptor adaptor = (IArrowTrailAdaptor)trailGO.GetComponent(typeof(IArrowTrailAdaptor));
				resultList.Add(adaptor);
				adaptor.SetUp();
				adaptor.SetArrowTrailReserveAdaptor(this);
			}
			return resultList.ToArray();
		}
		public override void SetUpReference(){
			IArrowTrail[] arrowTrails = CollectArrowTrails();
			thisReserve.SetSceneObjects(arrowTrails);
		}

		IArrowTrail[] CollectArrowTrails(){
			List<IArrowTrail> resultList = new List<IArrowTrail>();
			foreach(IArrowTrailAdaptor adaptor in thisAdaptors)
				resultList.Add(adaptor.GetArrowTrail());
			return resultList.ToArray();
		}
	}
}

