using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IColorSchemeManagerAdaptor: IAppleShooterMonoBehaviourAdaptor{

		IColorSchemeManager GetColorSchemeManager();

		Color GetSkyColor();
		Color GetEquatorColor();
		Color GetGroundColor();
		Color GetFogColor();

		Color GetSkyColorForTier(int index);
		Color GetEquatorColorForTier(int index);
		Color GetGroundColorForTier(int index);
		Color GetFogColorForTier(int index);
		
		void SetSkyColor(Color color);
		void SetEquatorColor(Color color);
		void SetGroundColor(Color color);
		void SetFogColor(Color color);
		void SetCameraBackgroundColor(Color color);
	}
	public class ColorSchemeManagerAdaptor: AppleShooterMonoBehaviourAdaptor, IColorSchemeManagerAdaptor{
		public override void SetUp(){
			thisColorSchemeManager = CreateColorSchemeManager();
		}
		IColorSchemeManager thisColorSchemeManager;
		public IColorSchemeManager GetColorSchemeManager(){
			return thisColorSchemeManager;
		}
		IColorSchemeManager CreateColorSchemeManager(){
			ColorSchemeManager.IConstArg arg = new ColorSchemeManager.ConstArg(
				this
			);
			return new ColorSchemeManager(arg);
		}

		public Color GetSkyColor(){
			return RenderSettings.ambientSkyColor;
		}
		public Color GetEquatorColor(){
			return RenderSettings.ambientEquatorColor;
		}
		public Color GetGroundColor(){
			return RenderSettings.ambientGroundColor;
		}
		public Color GetFogColor(){
			return RenderSettings.fogColor;
		}
		
		public Color GetSkyColorForTier(int index){
			return colorSchemes[index].skyColor;
		}
		public Color GetEquatorColorForTier(int index){
			return colorSchemes[index].equaotorColor;
		}
		public Color GetGroundColorForTier(int index){
			return colorSchemes[index].groundColor;
		}
		public Color GetFogColorForTier(int index){
			return colorSchemes[index].fogColor;
		}
		
		public ColorScheme[] colorSchemes;

		public void SetSkyColor(Color color){
			RenderSettings.ambientSkyColor = color;
		}
		public void SetEquatorColor(Color color){
			RenderSettings.ambientEquatorColor = color;
		}
		public void SetGroundColor(Color color){
			RenderSettings.ambientGroundColor = color;
		}
		public void SetFogColor(Color color){
			RenderSettings.fogColor = color;
		}
		public void SetCameraBackgroundColor(Color color){
			thisCamera.backgroundColor = color;
		}
		public Camera thisCamera;

	}
	[System.Serializable]
	public struct ColorScheme{
		public Color skyColor;
		public Color equaotorColor;
		public Color groundColor;
		public Color fogColor;
	}
}

