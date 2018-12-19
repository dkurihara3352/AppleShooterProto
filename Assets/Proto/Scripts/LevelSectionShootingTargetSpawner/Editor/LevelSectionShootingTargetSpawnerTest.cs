using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using AppleShooterProto;
using NSubstitute;

[TestFixture, Category("AppleShooterProto")]
public class LevelSectionShootingTargetSpawnerTest {


	public class TestSpawner: LevelSectionShootingTargetSpawner{
		public TestSpawner(IConstArg arg): base(arg){}
		public void SetCalculatorFields(
            TargetSpawnData returnedSpawnData
		){
			thisReturnedTargetSpawnData = returnedSpawnData;
		}
		TargetSpawnData thisReturnedTargetSpawnData;
		protected override IShootingTargetSpawnDataCalculator CreateCalculator(){
			IShootingTargetSpawnDataCalculator mockCalculator = Substitute.For<IShootingTargetSpawnDataCalculator>();
			mockCalculator.CalculateTargetSpawnDataByTargetType().Returns(thisReturnedTargetSpawnData);
			return mockCalculator;
		}	
		TargetSpawnData CreateReturnedSpawnData(
			int staticNumToCreate,
			out IShootingTargetReserve staticReserve,
            int staticSpawnPointCount,
            out IShootingTargetSpawnPoint[] staticSpawnPoints
		){
            IShootingTargetReserve thisStaticReserve;
            IShootingTargetSpawnPoint[] thisStaticSpawnPoints;
			TargetSpawnData reuslt =  CreateTargetSpawnData(
                staticNumToCreate,
                out thisStaticReserve,
                staticSpawnPointCount,
                out thisStaticSpawnPoints
            );
            staticReserve = thisStaticReserve;
            staticSpawnPoints = thisStaticSpawnPoints;
            return reuslt;
		}
        TargetSpawnData CreateTargetSpawnData(
            int staticCount,
            out IShootingTargetReserve staticReserve,
            int staticSpawnPointCount,
            out IShootingTargetSpawnPoint[] staticSpawnPoints
        ){
            IShootingTargetReserve staReserve = Substitute.For<IShootingTargetReserve>();
            staticReserve = staReserve;
            IShootingTargetSpawnPoint[] spawnPoints_sta = CreateMockSpawnPoints(staticSpawnPointCount);
            staticSpawnPoints = spawnPoints_sta;
            TargetSpawnData.Entry staticEntry = new TargetSpawnData.Entry(
                TargetType.Static,
                staticCount,
                staReserve,
                staticSpawnPoints
            );

            return new TargetSpawnData(
                new TargetSpawnData.Entry[]{
                    staticEntry
                }
            );
        }
        IShootingTargetSpawnPoint[] CreateMockSpawnPoints(int count ){
            List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
            for(int i = 0 ; i < count; i++){
                IShootingTargetSpawnPoint mockPoint = Substitute.For<IShootingTargetSpawnPoint>();
                mockPoint.GetIndex().Returns(i);
                mockPoint.GetEventPoint().Returns(i * .1f);
                resultList.Add(mockPoint);
            }
            return resultList.ToArray();
        }
        /* TestMethods */
	}

}
