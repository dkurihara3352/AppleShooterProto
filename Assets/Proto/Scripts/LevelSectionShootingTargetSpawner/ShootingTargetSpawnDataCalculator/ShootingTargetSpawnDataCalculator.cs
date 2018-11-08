﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleShooterProto{
	public interface IShootingTargetSpawnDataCalculator{
		TargetSpawnData CalculateTargetSpawnDataByTargetType();
	}
	public class ShootingTargetSpawnDataCalculator: IShootingTargetSpawnDataCalculator{
		public ShootingTargetSpawnDataCalculator(
			IConstArg arg
		){
			thisSpawnManager = arg.shootingTargetSpawnManager;
			
			thisSpawnValueLimit = arg.spawnValueLimit;
		
			thisDataInput = arg.dataInput;
			float[] targetSpawnRelativeProbabilities = GetTargetSpawnRelativeProbabilities(arg.dataInput);

			thisIndexPool = new Pool(targetSpawnRelativeProbabilities);


		}
		readonly protected IShootingTargetSpawnManager thisSpawnManager;
		readonly float thisSpawnValueLimit;
		readonly IPool thisIndexPool;
		TargetSpawnDataInput[] thisDataInput;
		protected float[] GetTargetSpawnRelativeProbabilities(TargetSpawnDataInput[] inputs){
			float[] result = new float[inputs.Length];
			int targetTypeIndex = 0;
			foreach(TargetSpawnDataInput input in inputs)
				result[targetTypeIndex ++] = input.relativeProbability;
			return result;
		}

		public TargetSpawnData CalculateTargetSpawnDataByTargetType(){
			float[] spawnValueByTargetType = GetSpawnValueByTargetType();
			int[] numToCreateByTargetType = GetNumberToCreateByTargetType(spawnValueByTargetType);
			int typeIndex = 0;
			List<TargetSpawnData.Entry> entriesList = new List<TargetSpawnData.Entry>();
			foreach(TargetSpawnDataInput input in thisDataInput){
				TargetType targetType = input.targetType;
				int numToCreate = numToCreateByTargetType[typeIndex];
				IShootingTargetReserve reserve = input.reserve;
				ISpawnPointEventPointPair[] pairs = CreatePairs(input.spawnPointAdaptorEventPointPairs);
				TargetSpawnData.Entry entry = new TargetSpawnData.Entry(
					targetType,
					numToCreate,
					reserve,
					pairs
				);
				entriesList.Add(entry);
				typeIndex ++;
			}
			TargetSpawnData.Entry[] entries = entriesList.ToArray();
			return new TargetSpawnData(entries);
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
		protected float[] GetSpawnValueByTargetType(){
			List<float> resultList = new List<float>();
			foreach(TargetSpawnDataInput input in thisDataInput){
				float spawnValue = thisSpawnManager.GetSpawnValue(input.targetType);
				resultList.Add(spawnValue);
			}
			return resultList.ToArray();
		}
		protected int[] GetNumberToCreateByTargetType(float[] spawnValues){
			int[] numToCreateArray = new int[spawnValues.Length];
			float sumOfSpawnValue = 0f;
			while(true){
				if(sumOfSpawnValue > thisSpawnValueLimit){
					break;
				}
				int targetTypeIndex = thisIndexPool.Draw();
				TargetSpawnDataInput input = thisDataInput[targetTypeIndex];
				int numToCreate = numToCreateArray[targetTypeIndex];
				// Debug.Log("max: " + input.maxCount.ToString() + ", " + "numToCreate: "+ numToCreate.ToString());
				if(input.maxCount > numToCreate){
					// Debug.Log("eval");
					numToCreate ++;
					numToCreateArray[targetTypeIndex] = numToCreate;
					float spawnValue = spawnValues[targetTypeIndex];
					sumOfSpawnValue += spawnValue;
					// Debug.Log(
					// 	"numToCreateArray #" + targetTypeIndex.ToString() +": " + numToCreate.ToString()
					// );
				}
			}
			// Debug.Log("numToCreateArray: " + DKUtility.DebugHelper.GetIndicesString(numToCreateArray));
			return numToCreateArray;
		}

		/*  */
			public interface IConstArg{
				float spawnValueLimit{get;}
				TargetSpawnDataInput[] dataInput{get;}
				IShootingTargetSpawnManager shootingTargetSpawnManager{get;}
			}
			public struct ConstArg: IConstArg{
				public ConstArg(
					float spawnValueLimit,
					TargetSpawnDataInput[] dataInput,
					IShootingTargetSpawnManager shootingTargetSpawnManager
				){
					thisSpawnValueLimit = spawnValueLimit;
					thisDataInput = dataInput;
					thisShootingTargetSpawnManager = shootingTargetSpawnManager;
				}
					readonly float thisSpawnValueLimit;
					public float spawnValueLimit{
						get{return thisSpawnValueLimit;}
					}

					readonly TargetSpawnDataInput[] thisDataInput;
					public TargetSpawnDataInput[] dataInput{
						get{return thisDataInput;}
					}
					readonly IShootingTargetSpawnManager thisShootingTargetSpawnManager;
					public IShootingTargetSpawnManager shootingTargetSpawnManager{
						get{return thisShootingTargetSpawnManager;}
					}
				
			}
		/*  */
	}
	public enum TargetType{
		Static,
		Fatty,
		Glider,
		Flier
	}
	public struct TargetSpawnData{
		public TargetSpawnData(
			Entry[] entries
		){
			thisEntries = entries;
		}
		Entry[] thisEntries;
		public int GetNumToCreate(TargetType type){
			foreach(Entry entry in thisEntries)
				if(entry.targetType == type)
					return entry.numToCreate;
			throw new System.InvalidOperationException(
				"not match"
			);
		}
		public Entry[] GetEntries(){
			return thisEntries;
		}
		public struct Entry{
			public Entry(
				TargetType targetType,
				int numToCreate,
				IShootingTargetReserve reserve,
				ISpawnPointEventPointPair[] pointPairs
			){
				this.targetType = targetType;
				this.numToCreate = numToCreate;
				this.reserve = reserve;
				this.spawnPointEventPointPairs = pointPairs;
			}
			public TargetType targetType;
			public int numToCreate;
			public IShootingTargetReserve reserve;
			public ISpawnPointEventPointPair[] spawnPointEventPointPairs;
		}
	}
}

