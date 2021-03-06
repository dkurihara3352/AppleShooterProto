﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	public interface IPlayerCameraAdaptor: ISlickBowShootingMonoBehaviourAdaptor{
		IPlayerCamera GetPlayerCamera();
		Vector2 GetDefaultFOVs();
		Vector2 GetMaxPanAngle();
	}
	public class PlayerCameraAdaptor : SlickBowShootingMonoBehaviourAdaptor, IPlayerCameraAdaptor {
		public Vector2 maxPanAngle;
		public Vector2 GetMaxPanAngle(){
			return maxPanAngle;
		}
		public UnityBase.MonoBehaviourAdaptor lookAtPivot;
		public float defaultVerticalFOV = 60f;
		public Vector2 GetDefaultFOVs(){
			Vector2 result = new Vector2();
			result[1] = defaultVerticalFOV;
			float aspectRatio = mainCamera.aspect;
			float defaultHorizontalFOV = CalculateHorizontalFOV(
				defaultVerticalFOV,
				aspectRatio
			);
			result[0] = defaultHorizontalFOV;
			return result;
		}
		float CalculateHorizontalFOV(
			float verticalFOV,
			float aspectRatio
		){
			float verAngInRad = verticalFOV * Mathf.Deg2Rad;
			float tanHalfVerTheta = Mathf.Tan(verAngInRad * .5f);
			float halfResultInRad = Mathf.Atan(tanHalfVerTheta * aspectRatio);
			float result = halfResultInRad * Mathf.Rad2Deg * 2f;
			return result;
		}
		public Camera mainCamera;
		public float smoothCoefficient = 5f;
		public override void SetUp(){
			thisPlayerCamera = CreatePlayerCamera();
		}
		public CoreGameplayInputScrollerAdaptor thisScrollerAdaptor;
		public override void SetUpReference(){
			ICoreGameplayInputScroller scroller = thisScrollerAdaptor.GetInputScroller();
			thisPlayerCamera.SetInputScroller(scroller);
			thisPlayerCamera.InitializeFOV();
		}
		IPlayerCamera thisPlayerCamera;
		public IPlayerCamera GetPlayerCamera(){
			return thisPlayerCamera;
		}
		IPlayerCamera CreatePlayerCamera(){
			PlayerCamera.IConstArg arg = new PlayerCamera.ConstArg(
				this,
				maxPanAngle,
				lookAtPivot,
				mainCamera,
				defaultVerticalFOV,
				smoothCoefficient
			);
			return new PlayerCamera(arg);
		}
	}
}
