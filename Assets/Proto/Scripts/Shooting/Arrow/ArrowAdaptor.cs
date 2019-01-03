using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace AppleShooterProto{
	public interface IArrowAdaptor: IAppleShooterMonoBehaviourAdaptor{
		IArrow GetArrow();

		void SetArrowReserveAdaptor(IArrowReserveAdaptor adaptor);
		void SetLaunchPointAdaptor(ILaunchPointAdaptor adaptor);
		void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor);
		void SetCollisionDetectionIntervalFrameCount(int count);
		void SetArrowTrailReserveAdaptor(IArrowTrailReserveAdaptor adaptor);
		void SetAudioManagerAdaptor(IAudioManagerAdaptor adaptor);

		void StartCollisionCheck();
		void StopCollisionCheck();

		void SetIndex(int index);
		string GetParentName();
		Vector3 GetPrevPosition();
		void PlayArrowHitSound();
		void PlayArrowReleaseSound();
		void ToggleRenderer(bool toggle);
	}
	public class ArrowAdaptor : AppleShooterMonoBehaviourAdaptor, IArrowAdaptor {
		
		/* Ref */
		public void SetCollisionDetectionIntervalFrameCount(int count){
			checkPerEveryThisFrames = count;
		}
		IArrowReserveAdaptor thisArrowReserveAdaptor;
		public void SetArrowReserveAdaptor(IArrowReserveAdaptor adaptor){
			thisArrowReserveAdaptor = adaptor;
		}
		ILaunchPointAdaptor thisLaunchPointAdaptor;
		public void SetLaunchPointAdaptor(ILaunchPointAdaptor adaptor){
			thisLaunchPointAdaptor  = adaptor;
		}
		IShootingManagerAdaptor thisShootingManagerAdaptor;
		public void SetShootingManagerAdaptor(IShootingManagerAdaptor adaptor){
			thisShootingManagerAdaptor = adaptor;
		}
		/*  */
		public override void SetUp(){
			thisArrow = CreateArrow();
		}
		IArrow thisArrow;
		public IArrow GetArrow(){
			return thisArrow;
		}
		IArrow CreateArrow(){
			Arrow.IConstArg arg = new Arrow.ConstArg(
				thisIndex,
				this
			);
			// thisIsReadyForGizmo = true;
			return new Arrow(arg);
		}
		public override void SetUpReference(){
			IArrowReserve reserve = thisArrowReserveAdaptor.GetArrowReserve();
			thisArrow.SetArrowReserve(reserve);

			ILaunchPoint launchPoint = thisLaunchPointAdaptor.GetLaunchPoint();
			thisArrow.SetLaunchPoint(launchPoint);

			IShootingManager shootingManager = thisShootingManagerAdaptor.GetShootingManager();
			thisArrow.SetShootingManager(shootingManager);
			thisShootingManager = shootingManager;

			IArrowTrailReserve arrowTrailReserve = thisArrowTrailReserveAdaptor.GetArrowTrailReserve();
			thisArrow.SetArrowTrailReserve(arrowTrailReserve);

			thisAudioManager = thisAudioManagerAdaptor.GetAudioManager();
		}
		IArrowTrailReserveAdaptor thisArrowTrailReserveAdaptor;
		public void SetArrowTrailReserveAdaptor(IArrowTrailReserveAdaptor adaptor){
			thisArrowTrailReserveAdaptor = adaptor;
		}
		public override void FinalizeSetUp(){
			thisArrow.Deactivate();
		}
		/* Debug */

		public string GetParentName(){
			return this.transform.parent.ToString();
		}
		int thisIndex;
		public void SetIndex(int index){
			thisIndex = index;
		}
		/* Collision Detection */
			int checkPerEveryThisFrames;
			Vector3 thisPrevPosition;
			public Vector3 GetPrevPosition(){
				return thisPrevPosition;
			}
			int targetLayerNumber = 8;
			int critLayerNumber = 9;
			Vector3 prevForDebug;
			Vector3 curForDebug;
			int count = 0;
			bool thisChecksForCollision = false;
			public void StartCollisionCheck(){
				count = 0;
				thisPrevPosition = GetPosition();
				thisChecksForCollision = true;
			}
			public void StopCollisionCheck(){
				thisChecksForCollision = false;
			}
			IShootingManager thisShootingManager;
			public void FixedUpdate(){
				if(thisChecksForCollision){
					count ++;
					if(count > checkPerEveryThisFrames)
						throw new System.InvalidOperationException(
							"fixed update frame skipped?"
						);
					if(count == checkPerEveryThisFrames){
						count = 0;
						Vector3 position = GetPosition();
						RaycastHit critHit;
						int layerMask = 1<<(targetLayerNumber);
						bool hasCritHit = Physics.Linecast(thisPrevPosition, position, out critHit, layerMask);

						if(hasCritHit){
							Transform hitTrans = critHit.transform;

							IArrowHitDetectorAdaptor detectorAdaptor =  hitTrans.GetComponentInParent(typeof(IArrowHitDetectorAdaptor)) as IArrowHitDetectorAdaptor;
							if(detectorAdaptor == null)
								throw new System.InvalidOperationException(
									"there's no IArrowHitDetectorAdaptor assigned to the hit transform"
								);
							IArrowHitDetector detector = detectorAdaptor.GetArrowHitDetector();
							
							detector.Hit(thisArrow);

							thisArrow.Land(
								detector,
								critHit.point
							);
						}
						thisPrevPosition = position;
					}
				}
			}
			public void PlayArrowHitSound(){
				arrowHitAudioSource.volume = thisAudioManager.GetSFXVolume();
				arrowHitAudioSource.Play();
			}
			public AudioSource arrowHitAudioSource;
			public void PlayArrowReleaseSound(){
				arrowReleaseSoundSource.volume = thisAudioManager.GetSFXVolume();
				arrowReleaseSoundSource.Play();
			}
			public AudioSource arrowReleaseSoundSource;
			public void ToggleRenderer(bool toggle){
				meshRenderer.enabled = toggle;
			}
			public MeshRenderer meshRenderer;
			IAudioManagerAdaptor thisAudioManagerAdaptor;
			public void SetAudioManagerAdaptor(IAudioManagerAdaptor adaptor){
				thisAudioManagerAdaptor = adaptor;
			}

			IAudioManager thisAudioManager;
	}
}
