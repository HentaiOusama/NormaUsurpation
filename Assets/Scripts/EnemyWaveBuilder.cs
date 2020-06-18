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
    int totalNumberOfActiveEnemyHolders = 0;



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

        if(waveBuildingData.waveLevel > waveBuildingData.maxWaves) {
            shouldBuildEnemyWave = false;
        }

        if(shouldBuildEnemyWave && (waveBuildingData.waveLevel <= waveBuildingData.maxWaves)) {
            shouldBuildEnemyWave = false;
            switch(waveBuildingData.waveLevel) {
                case 1:
                    buildWave1("right", 0, 0, 5, 6, viewableScreenConstrains.top - 4f, 1);
                    break;

                case 2:
                    buildWave2(0, 0, 5, 7, viewableScreenConstrains.top - 4f, 1);
                    break;
            }
        }
    }



    // Not fully complete
    void buildWave1(string startDirection, int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, int enemiesPerLayer, float startUpperHeight, float verticalGapSize){
        // index => which enemyShip to build
        // startDirection can only be left or right ATM.
        int totalEnemyCount = 0;
        startDirection = startDirection.ToLower();

        float screenWidth, enemyShipWidth, enemyShipHeight, horizontalGapSize, startPosHorizontal, startPosTop = startUpperHeight;
        screenWidth = 2*viewableScreenConstrains.right;
        Vector3 centerPosition;
        GameObject tempGameObject, multipleEnemyHolder;
        SizeData tempSizeData;
        
        // Get data regarding the ship we are going to build by instantiating a temporary enemyShip to get SizeData and then delete it.
        tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(0, 25, 0), Quaternion.Euler(0, 0, 0)) as GameObject; // Temporary Instantiation
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.defaultScaleForUse;
        enemyShipWidth = (tempSizeData.occupiedDistance.x/tempSizeData.referenceScale.x)*tempGameObject.transform.localScale.x;
        enemyShipHeight = (tempSizeData.occupiedDistance.z/tempSizeData.referenceScale.z)*tempGameObject.transform.localScale.z;
        Destroy(tempGameObject);
        horizontalGapSize = (screenWidth/enemiesPerLayer) - enemyShipWidth;
        startPosTop -= numberOfLayersToSkip*(enemyShipHeight);
        if(startDirection == "right") {
            centerPosition = new Vector3(2*viewableScreenConstrains.right, 0, startPosTop - ((enemyShipHeight/2) * numberOfLayersToBuild));
        } else if(startDirection == "left") {
            centerPosition = new Vector3(2*viewableScreenConstrains.left, 0, startPosTop - ((enemyShipHeight/2) * numberOfLayersToBuild));
        } else {
            Debug.Log("Error Building Wave 1");
            return;
        }
        
        multipleEnemyHolder = Instantiate(waveBuildingData.multipleEnemyHolder, new Vector3(centerPosition.x, 0, centerPosition.z), 
                                        Quaternion.Euler(0, 0, 0)) as GameObject;
        totalNumberOfActiveEnemyHolders++;

        for(int i = 0; i < numberOfLayersToBuild; i++) {
            startPosTop -= verticalGapSize/2;
            startPosTop -= enemyShipHeight/2;
            if(startDirection == "right") {
                startPosHorizontal = viewableScreenConstrains.right;
            } else if(startDirection == "left") {
                startPosHorizontal = viewableScreenConstrains.left;
            } else {
                return;
            }
            for(int j = 0; j < enemiesPerLayer; j++) {
                if(startDirection == "right") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "left") {
                    startPosHorizontal -= horizontalGapSize/2;
                    startPosHorizontal -= enemyShipWidth/2;
                }
                tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(startPosHorizontal, 0, startPosTop),
                                            Quaternion.Euler(0, 0, 0)) as GameObject;
                tempGameObject.transform.localScale = tempGameObject.GetComponent<SizeData>().defaultScaleForUse;
                tempGameObject.transform.SetParent(multipleEnemyHolder.transform);
                if(startDirection == "right") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "left") {
                    startPosHorizontal -= horizontalGapSize/2;
                    startPosHorizontal -= enemyShipWidth/2;
                }
                totalEnemyCount++;
            }
            startPosTop -= enemyShipHeight/2;
            startPosTop -= verticalGapSize/2;
        }
        float rotation_Y = 0;
        if(startDirection == "right") {
            rotation_Y = 0;
        } else if(startDirection == "left") {
            rotation_Y = 180;
        }
        tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, centerPosition.z), 
                                    Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
        if(startDirection == "right") {
            rotation_Y = 180;
        } else if(startDirection == "left") {
            rotation_Y = 0;
        }
        GameObject introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(centerPosition.x/2, 0, centerPosition.z), 
                                            Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
        tempSizeData = introPath.GetComponent<SizeData>();
        introPath.transform.localScale = tempSizeData.referenceScale * (Mathf.Abs(multipleEnemyHolder.transform.position.x - 
                                            tempGameObject.transform.position.x) / tempSizeData.occupiedDistance.x);
        GameObject[] movementPaths = new GameObject[] {tempGameObject};
        EnemyHolderDataHub holderDataHub =  multipleEnemyHolder.GetComponent<EnemyHolderDataHub>();
        holderDataHub.TakePathsForDeletion(this, introPath, movementPaths);
        holderDataHub.increaseChildrenShipsBy(totalEnemyCount);
        multipleEnemyHolder.AddComponent<EnemyShipMovementScript>().startMovingEnemies(1, introPath, movementPaths, 2, 4);
    }


    void buildWave2(int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, int enemiesPerLayer, float startUpperHeight, float verticalGapSize){
        // index => which enemyShip to build
        for(int i = 0; i < numberOfLayersToBuild; i++) {
            buildWave1((i%2==0)?"left":"right", enemyShipIndex, i, 1, enemiesPerLayer, startUpperHeight, verticalGapSize);
        }
    }


    public void startBuildingWaves(int waveLevel_, LBRTValues viewableScreenConstrains, Object enemyHolderObject) {
        this.viewableScreenConstrains = viewableScreenConstrains;
        tempWaveLevel = waveLevel_;
        this.enemyHolderObject = enemyHolderObject;
        shouldBuildEnemyWave = true;
    }


    public void buildNextWave() {
        tempWaveLevel++;
    }


    public void allChilderEnemiesDefeated() {
        totalNumberOfActiveEnemyHolders--;
        if(totalNumberOfActiveEnemyHolders == 0) {
            buildNextWave();
        }
    }
}