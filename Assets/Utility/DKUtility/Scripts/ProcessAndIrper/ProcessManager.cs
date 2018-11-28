using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DKUtility.CurveUtility;

namespace DKUtility{
	public interface IUISystemProcessManager{
	}
	public interface IProcessManager{
		void AddRunningProcess(IProcess process);
		void RemoveRunningProcess(IProcess process);
		void UpdateAllRegisteredProcesses(float deltaT);
		bool RunningProcessesContains(IProcess process);

		float GetSpringT(float normalizedT);

	}	
	public class ProcessManager: MonoBehaviour, IProcessManager{
		void Awake(){
			thisRunningProcesses = new List<IProcess>();
			springCalculator = new NormalizedSpringValueCalculator(100);
		}
		void Update(){
			UpdateAllRegisteredProcesses(Time.deltaTime);
		}
		List<IProcess> thisRunningProcesses;
		AbsProcess.IProcessComparer thisProcessComparer = new AbsProcess.ProcessComparer();
		public void AddRunningProcess(IProcess process){
			if(!thisRunningProcesses.Contains(process)){
				List<IProcess> newList = new List<IProcess>(thisRunningProcesses);
				newList.Add(process);
				thisRunningProcesses = newList;
				thisRunningProcesses.Sort(thisProcessComparer);
			}
		}
		public void RemoveRunningProcess(IProcess process){
			List<IProcess> newList = new List<IProcess>(thisRunningProcesses);
			newList.Remove(process);
			thisRunningProcesses = newList;
		}
		public void UpdateAllRegisteredProcesses(float deltaTime){
			foreach(IProcess process in thisRunningProcesses){
				process.UpdateProcess(deltaTime);
			}
		}
		public bool RunningProcessesContains(IProcess process){
			return thisRunningProcesses.Contains(process);
		}
		/*  */
		NormalizedSpringValueCalculator springCalculator;
		public float GetSpringT(float normalizedT){
			return springCalculator.GetSpringValue(normalizedT);
		}
	}
}
