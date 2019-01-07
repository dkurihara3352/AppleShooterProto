using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlickBowShooting{
	public interface IShootingTargetSpawnDataCalculator{
		TargetSpawnData CalculateTargetSpawnDataByTargetType();
	}
	public class ShootingTargetSpawnDataCalculator: IShootingTargetSpawnDataCalculator{
		public ShootingTargetSpawnDataCalculator(
			IConstArg arg
		){
			
			thisSpawnValueLimit = arg.spawnValueLimit;
		
			thisDataInput = arg.dataInput;
			float[] targetSpawnRelativeProbabilities = GetTargetSpawnRelativeProbabilities(arg.dataInput);

			thisIndexPool = new UnityBase.Pool(targetSpawnRelativeProbabilities);


		}
		readonly int thisSpawnValueLimit;
		readonly UnityBase.IPool thisIndexPool;
		TargetSpawnDataInput[] thisDataInput;
		protected float[] GetTargetSpawnRelativeProbabilities(TargetSpawnDataInput[] inputs){
			float[] result = new float[inputs.Length];
			int targetTypeIndex = 0;
			foreach(TargetSpawnDataInput input in inputs)
				result[targetTypeIndex ++] = input.relativeProbability;
			return result;
		}

		public TargetSpawnData CalculateTargetSpawnDataByTargetType(){
			int[] spawnValueByTargetType = GetSpawnValueByTargetType();
			int[] numToCreateByTargetType = GetNumberToCreateByTargetType(spawnValueByTargetType);
			int typeIndex = 0;
			List<TargetSpawnData.Entry> entriesList = new List<TargetSpawnData.Entry>();
			foreach(TargetSpawnDataInput input in thisDataInput){
				TargetType targetType = input.targetType;
				int numToCreate = numToCreateByTargetType[typeIndex];
				IShootingTargetReserve reserve = input.reserve;
				IShootingTargetSpawnPoint[] spawnPoints = CreateSpawnPoints(input.spawnPointGroupAdaptor);
				TargetSpawnData.Entry entry = new TargetSpawnData.Entry(
					targetType,
					numToCreate,
					reserve,
					spawnPoints
				);
				entriesList.Add(entry);
				typeIndex ++;
			}
			TargetSpawnData.Entry[] entries = entriesList.ToArray();
			return new TargetSpawnData(entries);
		}
		IShootingTargetSpawnPoint[] CreateSpawnPoints(IShootingTargetSpawnPointGroupAdaptor spawnPointGroupAdaptor){
			// IShootingTargetSpawnPointGroup group = spawnPointGroupAdaptor.GetGroup();
			// return group.GetSpawnPoints();
			List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
			IShootingTargetSpawnPointAdaptor[] adaptors = spawnPointGroupAdaptor.GetAdaptors();
			foreach(IShootingTargetSpawnPointAdaptor adaptor in adaptors){
				resultList.Add(adaptor.GetSpawnPoint());
			}
			return resultList.ToArray();

		}
		ISpawnPointEventPointPair[] CreatePairs(SpawnPointAdaptorEventPointPair[] adaptorPairs){
			List<ISpawnPointEventPointPair> pairs = new List<ISpawnPointEventPointPair>();
			foreach(SpawnPointAdaptorEventPointPair adaptorPair in adaptorPairs){
				IShootingTargetSpawnPoint spawnPoint = adaptorPair.adaptor.GetSpawnPoint();
				float eventPoint = adaptorPair.eventPoint;
				pairs.Add(new SpawnPointEventPointPair(
					spawnPoint,
					eventPoint
				));
			}
			return pairs.ToArray();
		}
		protected int[] GetSpawnValueByTargetType(){
			List<int> resultList = new List<int>();
			foreach(TargetSpawnDataInput input in thisDataInput){
				int spawnValue = input.spawnValue;
				resultList.Add(spawnValue);
			}
			return resultList.ToArray();
		}
		protected int[] GetNumberToCreateByTargetType(int[] spawnValues){
			int[] numToCreateArray = new int[spawnValues.Length];
			int sumOfSpawnValue = 0;
			while(true){
				if(sumOfSpawnValue > thisSpawnValueLimit){
					break;
				}
				int targetTypeIndex = thisIndexPool.Draw();
				TargetSpawnDataInput input = thisDataInput[targetTypeIndex];
				int numToCreate = numToCreateArray[targetTypeIndex];

				if(input.maxCount > numToCreate){

					numToCreate ++;
					numToCreateArray[targetTypeIndex] = numToCreate;
					int spawnValue = spawnValues[targetTypeIndex];
					sumOfSpawnValue += spawnValue;
				}
			}
			return numToCreateArray;
		}

		/*  */
			public interface IConstArg{
				int spawnValueLimit{get;}
				TargetSpawnDataInput[] dataInput{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					int spawnValueLimit,
					TargetSpawnDataInput[] dataInput
				){
					thisSpawnValueLimit = spawnValueLimit;
					thisDataInput = dataInput;
				}
					readonly int thisSpawnValueLimit;
					public int spawnValueLimit{
						get{return thisSpawnValueLimit;}
					}

					readonly TargetSpawnDataInput[] thisDataInput;
					public TargetSpawnDataInput[] dataInput{
						get{return thisDataInput;}
					}
				
			}
		/*  */
	}
	public enum TargetType{
		Static,
		Fatty,
		Glider,
		Flyer
	}
	public struct TargetSpawnData{
		public TargetSpawnData(
			Entry[] entries
		){
			thisEntries = entries;
		}
		Entry[] thisEntries;

		public Entry[] GetEntries(){
			return thisEntries;
		}
		public struct Entry{
			public Entry(
				TargetType targetType,
				int numToCreate,
				IShootingTargetReserve reserve,
				IShootingTargetSpawnPoint[] spawnPoints
			){
				this.targetType = targetType;
				this.numToCreate = numToCreate;
				this.reserve = reserve;
				this.spawnPoints = spawnPoints;
			}
			public TargetType targetType;
			public int numToCreate;
			public IShootingTargetReserve reserve;
			public IShootingTargetSpawnPoint[] spawnPoints;
		}
	}
}

