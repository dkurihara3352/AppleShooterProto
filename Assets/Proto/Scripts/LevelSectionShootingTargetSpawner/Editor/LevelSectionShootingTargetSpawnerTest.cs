using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using AppleShooterProto;
using NSubstitute;

[TestFixture, Category("AppleShooterProto")]
public class LevelSectionShootingTargetSpawnerTest {
    [Test, TestCaseSource(typeof(GetSumOfTargetTypeRareChanceTestCase), "case1")]
    public void GetSumOfTargetTypeRareChance_Various(
        int spawnCount,
        float chance,
        float expected
    ){
        TestSpawner spawner = CreateTestSpawner();

        float actual = spawner.GetSumOfTargetTypeRareChance_Test(
            spawnCount,
            chance
        );

        Assert.That(actual, Is.EqualTo(expected));
    }
    public class GetSumOfTargetTypeRareChanceTestCase{
        public static object[] case1 = {
            new object[]{
                0,
                1f,
                0f
            },
            new object[]{
                1,
                .5f,
                .5f
            },
            new object[]{
                4,
                .5f,
                2f
            }
        };
    }

    [Test, TestCaseSource(typeof(GetSumOfRelativeRareChanceTestCase), "case1")]
    public void GetSumOfRelativeRareChance_Various(float[] relChanceArray, float expected){
        TestSpawner spawner = CreateTestSpawner();
        IShootingTargetSpawnPoint[] mockSpawnPoints = CreateMockSpawnPoints(
            relChanceArray
        );
        float actual = spawner.GetSumOfRelativeRareChance_Test(mockSpawnPoints, new int[]{0, 1, 2});

        Assert.That(actual, Is.EqualTo(expected));
    }
    IShootingTargetSpawnPoint[] CreateMockSpawnPoints(float[] relativeProbabilityArray){
        List<IShootingTargetSpawnPoint> resultList = new List<IShootingTargetSpawnPoint>();
        foreach(float relativeProb in relativeProbabilityArray){
            IShootingTargetSpawnPoint mockPoint = Substitute.For<IShootingTargetSpawnPoint>();
            mockPoint.GetRelativeRareChance().Returns(relativeProb);
            resultList.Add(mockPoint);
        }
        return resultList.ToArray();
    }
    public class GetSumOfRelativeRareChanceTestCase{
        public static object[] case1 = {
            new object[]{
                new float[]{
                    1f, 
                    1f, 
                    1f
                }, 
                3f
            },
            new object[]{
                new float[]{
                    10f, 
                    1f, 
                    1f
                }, 
                12f
            },
            new object[]{
                new float[]{
                    0f,
                    2f,
                    100f
                },
                102f
            },
        };
    }

    [Test, TestCaseSource(typeof(GetRareProbabilityTestCase), "case1")]
    public void GetRareProbability_Various(
        float relativeProb,
        float sumOfRelChance,
        float sumOfTypeChance,
        float expected
    ){
        TestSpawner spawner = CreateTestSpawner();

        float actual = spawner.GetRareProbability_Test(
            relativeProb,
            sumOfRelChance,
            sumOfTypeChance
        );
        
        Assert.That(actual, Is.EqualTo(expected));
    }  
    public class GetRareProbabilityTestCase{
        public static object[] case1 = {
            new object[]{
                0f,
                40f,
                5f,
                0f
            },
            new object[]{
                1f,
                10f,
                5f,
                .5f
            },
            new object[]{
                2f,
                10f,
                5f,
                1f
            },
            new object[]{
                .5f,
                10f,
                5f,
                .25f
            },
            new object[]{
                3f,
                10f,
                5f,
                1.5f
            }
        };
    }
    [Test]
    public void ShouldSpawnRare_Demo(){
        TestSpawner spawner = CreateTestSpawner();
        int trialCount = 1000;
        float prob = .01f;
        int successCount = 0;
        int failureCount = 0;
        for(int i = 0; i < trialCount; i ++){
            if(spawner.ShouldSpawnRare_Test(prob))
                successCount++;
            else
                failureCount++;
        }
        Debug.Log(
            "NumOfTrial: " + trialCount.ToString() + ", "+ 
            "prob: " + prob.ToString() + ", " + 
            "true: " + successCount.ToString() + ", " + 
            "false: " + failureCount.ToString() + ", " +
            "actualProb: " + ((successCount * 1f)/ trialCount).ToString()
        );
    }
    [Test, TestCaseSource(typeof(CreateSpawnEventsForEntryTestCase), "case1")]
    public void CreateSpawnEventsForEntry_Demo(
        float[] relativeChanceArray,
        float targetTypeRareChance
    ){

        TestSpawner spawner = CreateTestSpawner();

        IShootingTargetSpawnPoint[] mockSpawnPoints = CreateMockSpawnPoints(
            relativeChanceArray
        );

        IShootingTargetReserve mockReserve = Substitute.For<IShootingTargetReserve>();
        mockReserve.GetTargetTypeRareProbability().Returns(targetTypeRareChance);

        int[] indices = CreateIndices(relativeChanceArray.Length);

        int[] rareResults = new int[relativeChanceArray.Length];
        int trialCount = 100;
        for(int i = 0; i< trialCount; i ++){
            IShootingTargetSpawnWaypointEvent[] events = spawner.CraeteSpawnEventsForEntry_Test(
                mockSpawnPoints,
                mockReserve,
                indices
            );
            int index = 0;
            foreach(IShootingTargetSpawnWaypointEvent wpEvent in events){
                if(wpEvent.IsRare())
                    rareResults[index] ++;
                index ++;
            }
        }
        string result = "";
        int spawnPointIndex = 0;
        float sumOfRareProb = 0f;
        foreach(float relativeChance in relativeChanceArray){
            int rareResultCount = rareResults[spawnPointIndex];
            float rareProb = rareResultCount/ (trialCount * 1f);
            result += 
                DKUtility.DebugHelper.StringInColor(" spawnPoint ", Color.red) + spawnPointIndex.ToString() + ", " +
                "relChance: " + relativeChance.ToString() + ", " +
                "rareCount: " + rareResultCount.ToString() + " , " +
                "rareProb: " + rareProb.ToString();
            sumOfRareProb += rareProb;
            spawnPointIndex ++;
        }
        result += DKUtility.DebugHelper.StringInColor(", averageRareProb: ", Color.blue) + sumOfRareProb/ (spawnPointIndex);
        Debug.Log(
            result
        );
    }
    int[] CreateIndices(int length){
        List<int> resultList = new List<int>();
        for(int i = 0; i < length; i ++){
            resultList.Add(i);
        }
        return resultList.ToArray();
    }
    public class CreateSpawnEventsForEntryTestCase{
        public static object[] case1 = {
            new object[]{
                new float[]{
                    2f,
                    1f,
                    1f
                },
                .2f
            },
            new object[]{
                new float[]{
                    2f,
                    1f,
                    0f
                },
                .2f
            },
            new object[]{
                new float[]{
                    2f,
                    0f,
                    0f
                },
                .2f
            },
            new object[]{
                new float[]{
                    3f,
                    2f,
                    1f
                },
                .2f
            },
        };
    }



    TestSpawner CreateTestSpawner(){
        TestSpawner.IConstArg arg = new TestSpawner.ConstArg(
            Substitute.For<ILevelSectionShootingTargetSpawnerAdaptor>(),
            100
        );
        return new TestSpawner(arg);
    }
	public class TestSpawner: LevelSectionShootingTargetSpawner{
		public TestSpawner(IConstArg arg): base(arg){}
        /* TestMethods */
        public float GetSumOfTargetTypeRareChance_Test(
            int spawnCount,
            float targetTypeRareProbability
        ){
            return this.GetSumOfTargetTypeRareChance(
                spawnCount,
                targetTypeRareProbability
            );
        }
        public float GetSumOfRelativeRareChance_Test(
            IShootingTargetSpawnPoint[] spawnPoints,
            int[] indicesToSpawn
        ){
            return this.GetSumOfRelativeRareChance(
                spawnPoints,
                indicesToSpawn
            );
        }
        public float GetRareProbability_Test(
            float relativeRareChance,
            float sumOfRelativeChance,
            float sumOfTargetTypeRareChance
        ){
            return GetRareProbability(
                relativeRareChance,
                sumOfRelativeChance,
                sumOfTargetTypeRareChance
            );
        }
        public bool ShouldSpawnRare_Test(float prob){
            return this.ShouldSpawnRare(prob);
        }
        public IShootingTargetSpawnWaypointEvent[] CraeteSpawnEventsForEntry_Test(
            IShootingTargetSpawnPoint[] spawnPoints,
            IShootingTargetReserve reserve,
            int[] indicesToSpawn
        ){
            return this.CreateSpawnEventsForEntry(
                spawnPoints,
                reserve,
                indicesToSpawn
            );
        }
        protected override IShootingTargetReserve GetRareTargetReserve(TargetType targetType){
            return Substitute.For<IShootingTargetReserve>();
        }
	}

}
