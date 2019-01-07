using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility;

namespace SlickBowShooting{
	/*  Interstitial AD plays when 
			AD is NOT removed yet
			AND 3 mins have passed since last watch 
			AND at least 3 sessions have been played
			AND START Button is pressed
		
		Watching an ad (interstitial or rewarded) resets the timer and counter

		Timer
			when expired, flag timer is ready
		Counter
			when reached max count, flag ready
	*/
	public interface IInterstitialADManager: ISlickBowShootingSceneObject, IProcessHandler{
		void SetADManager(IADManager manager);
		void ResetTimerAndCounter();
		void StartCounting();
		void StopCounting();
		bool IsCountingDown();
		void IncrementSessionCountPlayed();
		bool ADIsDue();
		void StartAD();
		string GetDebugString();
	}
	public class InterstitialADManager: SlickBowShootingSceneObject, IInterstitialADManager{
		public InterstitialADManager(IConstArg arg): base(arg){
			thisCountDownTimerProcessSuite = new ProcessSuite(
				thisProcessManager,
				this,
				ProcessConstraint.None,
				0f
			);
		}
		IInterstitialADManagerAdaptor thisInterstitialADManagerAdaptor{
			get{
				return (IInterstitialADManagerAdaptor)thisAdaptor;
			}
		}
		IProcessSuite thisCountDownTimerProcessSuite;
		float thisTimerExpireTime = 180f;
		int thisMinSessionCount = 3;

		public void ResetTimerAndCounter(){
			thisElapsedTime = 0f;
			thisSessionCountPlayed = 0;
			thisTimerHasExpired = false;
		}

		float thisElapsedTime = 0f;
		bool thisTimerHasExpired = false;
		int thisSessionCountPlayed = 0;

		public void StartCounting(){
			thisCountDownTimerProcessSuite.Start();
		}
		public void StopCounting(){
			thisCountDownTimerProcessSuite.Expire();
		}
		public bool IsCountingDown(){
			return thisCountDownTimerProcessSuite.IsRunning();
		}
		public void OnProcessRun(IProcessSuite suite){
			if(suite == thisCountDownTimerProcessSuite){
			}
		}
		public void OnProcessUpdate(
			float deltaTime,
			float normalizedTime,
			IProcessSuite suite
		){
			if(suite == thisCountDownTimerProcessSuite){
				thisElapsedTime += deltaTime;
				if(thisElapsedTime >= thisTimerExpireTime)
					suite.Expire();
			}
		}
		public void OnProcessExpire(
			IProcessSuite suite
		){
			if(suite == thisCountDownTimerProcessSuite){
				thisTimerHasExpired = true;
			}
		}
		public void IncrementSessionCountPlayed(){
			thisSessionCountPlayed ++;
		}
		public bool ADIsDue(){
			return thisTimerHasExpired && thisMinNumOfSessionsPlayed;
		}
		bool thisMinNumOfSessionsPlayed{
			get{
				return thisSessionCountPlayed >= thisMinSessionCount; 
			}
		}
		public void StartAD(){
			thisADManager.StartNonRewardedADSequence();
		}
		IADManager thisADManager;
		public void SetADManager(IADManager manager){
			thisADManager = manager;
		}

		public string GetDebugString(){
			string result = "";
			result += "Due: " + this.ADIsDue().ToString() + "\n";
			result += "elapsed: " + thisElapsedTime.ToString() + "\n";
			result += "session#: " + thisSessionCountPlayed.ToString() + "\n";
			return result;
		}
	}
}