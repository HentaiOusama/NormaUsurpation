using UnityEngine;

[System.Serializable]
public class WaveLevelData {
    public Object[] enemyObject;
}


public class EnemyWaveBuilder : MonoBehaviour
{
    // Serialized variables
    public WaveLevelData[] waveLevelData;
    public int waveLevel = 1;
    public int maxWaves;



    // Non-Serialized Variables
    static bool shouldBuildEnemyWave;
    static int tempWaveLevel = 1;
    static int totalEnemyCount = 0;



    // Start is called before the first frame update
    void Start()
    {
        shouldBuildEnemyWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(waveLevel != tempWaveLevel) {
            waveLevel = tempWaveLevel;
            shouldBuildEnemyWave = true;
        }

        if(shouldBuildEnemyWave && (waveLevel <= waveLevelData.Length)) {
            switch(waveLevel) {
                case 1:
                    Instantiate(waveLevelData[waveLevel-1].enemyObject[0], new Vector3(0, 0, 25), Quaternion.Euler(0, 0, 0));
                    totalEnemyCount++;
                    break;
            }
            shouldBuildEnemyWave = false;
        }

        if(totalEnemyCount == 0) {
            tempWaveLevel++;
        }
        
    }

    public static void buildEnemyWave(int waveLevel_) {
        shouldBuildEnemyWave = true;
        tempWaveLevel = waveLevel_;
    }


    public static void decreaseEnemyCount() {
        totalEnemyCount--;
    }


}