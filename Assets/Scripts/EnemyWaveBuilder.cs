using UnityEngine;
using PathCreation;

[System.Serializable]
public class WaveBuildingData {
    public Object[] enemyObjects;
    public Object[] movementPathObjects;
    public Object multipleEnemyHolder;
    public int waveLevel;
    public int maxWaves;
}


public class EnemyWaveBuilder : MonoBehaviour
{
    // Serialized variables
    public WaveBuildingData waveBuildingData;

    // Non-Serialized Variables
    LBRTValues viewableScreenConstrains;
    Object enemyHolderObject;
    bool shouldBuildEnemyWave = false;
    int tempWaveLevel = 1;
    int totalEnemyCount = 0;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(waveBuildingData.waveLevel != tempWaveLevel) {
            waveBuildingData.waveLevel = tempWaveLevel;
            shouldBuildEnemyWave = true;
        }

        if(shouldBuildEnemyWave && (waveBuildingData.waveLevel <= waveBuildingData.maxWaves)) {
            shouldBuildEnemyWave = false;
            switch(waveBuildingData.waveLevel) {
                case 1:
                    buildWave1(0, 1, 5);
                    break;
            }
        }

        if(totalEnemyCount == 0) {
            tempWaveLevel++;
        }
        
    }



    // Not fully complete
    public void buildWave1(int index, int numberOfLayers, int enemiesPerLayer){
        // index => which enemyShip to build

        float screenWidth = 2*viewableScreenConstrains.right;
        
        GameObject tempObj = Instantiate(waveBuildingData.enemyObjects[index], new Vector3(0, 25, 0), Quaternion.Euler(0, 0, 0)) as GameObject; // Temporary Instantiation
        SizeData enemyShipSizeData = tempObj.GetComponent<SizeData>();

        float enemyShipWidth = (enemyShipSizeData.occupiedDistance.x/enemyShipSizeData.defaultScaleForUse.x)*tempObj.transform.localScale.x;
        float gapSize = (screenWidth/enemiesPerLayer) - enemyShipWidth;

        GameObject multipleEnemyHolder = Instantiate(waveBuildingData.multipleEnemyHolder, new Vector3(2*viewableScreenConstrains.right, 0, 0), 
                                                    Quaternion.Euler(0, 0, 0)) as GameObject;
        Destroy(tempObj);
        float startPos = viewableScreenConstrains.right;
        for(int i = 0; i < enemiesPerLayer; i++) {
            startPos += gapSize/2;
            startPos += enemyShipWidth/2;
            tempObj = Instantiate(waveBuildingData.enemyObjects[index], new Vector3(startPos, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            tempObj.transform.SetParent(multipleEnemyHolder.transform);
            startPos += enemyShipWidth/2;
            startPos += gapSize/2;
            totalEnemyCount++;
        }

        tempObj = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
    }



    public void startBuildingWaves(int waveLevel_, LBRTValues viewableScreenConstrains, Object enemyHolderObject) {
        this.viewableScreenConstrains = viewableScreenConstrains;
        tempWaveLevel = waveLevel_;
        this.enemyHolderObject = enemyHolderObject;
        shouldBuildEnemyWave = true;
    }


    public void decreaseEnemyCount() {
        totalEnemyCount--;
    }


}