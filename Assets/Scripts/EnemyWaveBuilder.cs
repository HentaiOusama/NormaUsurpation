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
                    buildWave1(0, 0, 4, 6, viewableScreenConstrains.top - 4f, 1);
                    break;
            }
        }

        if(totalEnemyCount == 0) {
            tempWaveLevel++;
        }
        
    }



    // Not fully complete
    public void buildWave1(int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, int enemiesPerLayer, float startUpperHeight, float verticalGapSize){
        // index => which enemyShip to build
        float screenWidth, enemyShipWidth, enemyShipHeight, horizontalGapSize, startPosLeft, startPosTop = startUpperHeight;
        screenWidth = 2*viewableScreenConstrains.right;
        Vector3 centerPosition;
        GameObject tempGameObject, multipleEnemyHolder;
        SizeData tempSizeData;
        
        // Get data regarding the ship we are going to build by instantiating a temporary enemyShip to get SizeData and then delete it.
        tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(0, 25, 0), Quaternion.Euler(0, 0, 0)) as GameObject; // Temporary Instantiation
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        enemyShipWidth = (tempSizeData.occupiedDistance.x/tempSizeData.defaultScaleForUse.x)*tempGameObject.transform.localScale.x;
        enemyShipHeight = (tempSizeData.occupiedDistance.z/tempSizeData.referenceScale.z)*tempGameObject.transform.localScale.z;
        Destroy(tempGameObject);
        horizontalGapSize = (screenWidth/enemiesPerLayer) - enemyShipWidth;
        startPosTop -= numberOfLayersToSkip*(enemyShipHeight);
        centerPosition = new Vector3(2*viewableScreenConstrains.right, 0, startPosTop - ((enemyShipHeight/2) * numberOfLayersToBuild));
        multipleEnemyHolder = Instantiate(waveBuildingData.multipleEnemyHolder, new Vector3(centerPosition.x, 0, centerPosition.z), 
                                        Quaternion.Euler(0, 0, 0)) as GameObject;


        for(int i = 0; i < numberOfLayersToBuild; i++) {
            startPosTop -= verticalGapSize/2;
            startPosTop -= enemyShipHeight/2;
            startPosLeft = viewableScreenConstrains.right;
            for(int j = 0; j < enemiesPerLayer; j++) {
                startPosLeft += horizontalGapSize/2;
                startPosLeft += enemyShipWidth/2;
                tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(startPosLeft, 0, startPosTop),
                                            Quaternion.Euler(0, 0, 0)) as GameObject;
                tempGameObject.transform.SetParent(multipleEnemyHolder.transform);
                startPosLeft += enemyShipWidth/2;
                startPosLeft += horizontalGapSize/2;
                totalEnemyCount++;
            }
            startPosTop -= enemyShipHeight/2;
            startPosTop -= verticalGapSize/2;
        }

        tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, centerPosition.z), 
                                    Quaternion.Euler(0, 0, 0)) as GameObject;
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
        GameObject introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(centerPosition.x/2, 0, centerPosition.z), 
                                            Quaternion.Euler(0, 180, 0)) as GameObject;
        tempSizeData = introPath.GetComponent<SizeData>();
        introPath.transform.localScale = tempSizeData.referenceScale * ((multipleEnemyHolder.transform.position.x - 
                                            tempGameObject.transform.position.x) / tempSizeData.occupiedDistance.x);
        multipleEnemyHolder.AddComponent<EnemyShipMovementScript>().startMovingEnemies(1, introPath, new GameObject[]{tempGameObject}, 2, 4);
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