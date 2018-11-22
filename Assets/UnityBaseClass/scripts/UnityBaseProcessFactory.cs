using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace UnityBase{
	public interface IUnityBaseProcessFactory: IProcessFactory{
		IMarkerUIMarkProcess CreateMarkerUIMarkProcess(
			ISceneUI ui
		);
		IPopUIGlideProcess CreatePopUIGlideProcess(
			IPopUI popUI,
			PopUIAdaptor.PopMode popMode,
			float glideTime,
			float minGlideDistance,
			float maxGlideDistance,
			AnimationCurve normalizedDistanceCurve,
			AnimationCurve alhpaCurve,
			AnimationCurve scaleCurve,
			Vector2 graphicOriginalLocalPosition,
			Color graphicOriginalColor
		);
	}
	public abstract class UnityBaseProcessFactory: AbsProcessFactory, IUnityBaseProcessFactory{
		public UnityBaseProcessFactory(
			IProcessManager processManager
		)	: base(processManager){}

		public IMarkerUIMarkProcess CreateMarkerUIMarkProcess(
			ISceneUI sceneUI
		){
			MarkerUIMarkProcess.IConstArg arg = new MarkerUIMarkProcess.ConstArg(
				sceneUI,
				thisProcessManager
			);
			return new MarkerUIMarkProcess(arg);
		}
		public IPopUIGlideProcess CreatePopUIGlideProcess(
			IPopUI popUI,
			PopUIAdaptor.PopMode popMode,
			float glideTime,
			float minGlideDistance,
			float maxGlideDistance,
			AnimationCurve normalizedDistanceCurve,
			AnimationCurve alhpaCurve,
			AnimationCurve scaleCurve,
			Vector2 graphicOriginalLocalPosition,
			Color graphicOriginalColor
		){
			PopUIGlideProcess.IConstArg arg = new PopUIGlideProcess.ConstArg(
				popUI,
				popMode,
				minGlideDistance,
				maxGlideDistance,
				normalizedDistanceCurve,
				alhpaCurve,
				scaleCurve,
				graphicOriginalLocalPosition,
				graphicOriginalColor,

				thisProcessManager,
				glideTime
			);
			return new PopUIGlideProcess(arg);
		}
	}
}

