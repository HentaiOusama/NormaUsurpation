using UnityEngine;
using PathCreation;


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
                    buildWave1("right", "left", 0, 0, 5, 7, viewableScreenConstrains.top - 4f, 1, 2, 4);
                    break;

                case 2:
                    buildWave2("left", "right", 0, 0, 5, 7, viewableScreenConstrains.top - 4f, 1, 2, 4);
                    break;
                
                case 3:
                    buildWave3("top", "right", 0, 0, 5, 7, viewableScreenConstrains.top - 4f, 1, 2, 4);
                    break;

                case 4: 
                    buildWave4(0, 0, 6, 7, viewableScreenConstrains.top - 4f, 1, 2, 4);
                    break;
            }
        }
    }



    // Not fully complete
    void buildWave1(string startDirection, string oscillateStartDirection, int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, 
                    int enemiesPerLayer, float startUpperHeight, float verticalGapSize, float introSpeed, float movementSpeed){
        
        // index => which enemyShip to build.
        // startDirection can only be left, right or top. (It means where to build the enemyShips before introduction).
        // oscillateStartDirection can only be left or right.
        int totalEnemyCount = 0;
        startDirection = startDirection.ToLower();
        oscillateStartDirection = oscillateStartDirection.ToLower();
        float tempStartUpperHeight = startUpperHeight;


        float screenWidth, enemyShipWidth, enemyShipHeight, horizontalGapSize, startPosHorizontal, startPosVertical = startUpperHeight;
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


        // Calculating Centre and Start Positions Positions
        if(startDirection == "top") {
            startPosVertical = viewableScreenConstrains.top + numberOfLayersToBuild*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(0, 0, (viewableScreenConstrains.top + startPosVertical)/2);
        } else if(startDirection == "right") {
            startPosVertical -= numberOfLayersToSkip*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(2*viewableScreenConstrains.right, 0, startPosVertical - (((enemyShipHeight + verticalGapSize)/2) * numberOfLayersToBuild));
        } else if(startDirection == "left") {
            startPosVertical -= numberOfLayersToSkip*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(2*viewableScreenConstrains.left, 0, startPosVertical - (((enemyShipHeight + verticalGapSize)/2) * numberOfLayersToBuild));
        } else {
            Debug.LogError("Error Building Wave 1. Input startDirection is not acceptable.");
            return;
        }
        

        // Building Enemy Holder
        multipleEnemyHolder = Instantiate(waveBuildingData.multipleEnemyHolder, new Vector3(centerPosition.x, 0, centerPosition.z), 
                                        Quaternion.Euler(0, 0, 0)) as GameObject;
        totalNumberOfActiveEnemyHolders++;


        // Building all layers and their enemies
        for(int i = 0; i < numberOfLayersToBuild; i++) 
        {
            // Setting start Positon for current Layer to build
            startPosVertical -= verticalGapSize/2;
            startPosVertical -= enemyShipHeight/2;
            if(startDirection == "top"){
                startPosHorizontal = viewableScreenConstrains.left;
            }else if(startDirection == "right") {
                startPosHorizontal = viewableScreenConstrains.right;
            } else if(startDirection == "left") {
                startPosHorizontal = viewableScreenConstrains.left;
            } else {
                Debug.LogError("Error Building Wave 1. Input startDirection is not acceptable.");
                return;
            }

            // Building enemies of current Layer
            for(int j = 0; j < enemiesPerLayer; j++) {
                if(startDirection == "top") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "right") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "left") {
                    startPosHorizontal -= horizontalGapSize/2;
                    startPosHorizontal -= enemyShipWidth/2;
                }

                tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(startPosHorizontal, 0, startPosVertical),
                                            Quaternion.Euler(0, 0, 0)) as GameObject;
                tempGameObject.transform.localScale = tempGameObject.GetComponent<SizeData>().defaultScaleForUse;
                tempGameObject.transform.SetParent(multipleEnemyHolder.transform);
                tempGameObject.GetComponent<EnemyShipDataHub>().TakeHolder(multipleEnemyHolder);

                if(startDirection == "top") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "right") {
                    startPosHorizontal += horizontalGapSize/2;
                    startPosHorizontal += enemyShipWidth/2;
                } else if(startDirection == "left") {
                    startPosHorizontal -= horizontalGapSize/2;
                    startPosHorizontal -= enemyShipWidth/2;
                }
                totalEnemyCount++;
            }

            
            startPosVertical -= enemyShipHeight/2;
            startPosVertical -= verticalGapSize/2;
        }


        // Building Oscillating Path
        float rotation_Y = 0;
        float tempVerticalPos = viewableScreenConstrains.top;
        if(startDirection == "top") {
            tempVerticalPos = tempStartUpperHeight - (numberOfLayersToSkip * (enemyShipHeight + verticalGapSize)) - 
                                    ((numberOfLayersToBuild * (enemyShipHeight + verticalGapSize)) / 2);
            if(oscillateStartDirection == "right") {
                rotation_Y = 180;
                tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, tempVerticalPos), 
                                        Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
                tempSizeData = tempGameObject.GetComponent<SizeData>();
                tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
            } else if(oscillateStartDirection == "left") {
                rotation_Y = 0;
                tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, tempVerticalPos), 
                                        Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
                tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
            }
        }
        else {
            if(oscillateStartDirection == "right") {
                rotation_Y = 180;
                tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, centerPosition.z), 
                                        Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
                tempSizeData = tempGameObject.GetComponent<SizeData>();
                tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
            } else if(oscillateStartDirection == "left") {
                rotation_Y = 0;
                tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(0, 0, centerPosition.z), 
                                        Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
                tempSizeData = tempGameObject.GetComponent<SizeData>();
                tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);
            }
        }


        // Building Intro Path
        GameObject introPath = null;
        if(startDirection == "top") {
            rotation_Y = 90;
            float verticalIntroPathSize = centerPosition.z - tempVerticalPos;
            tempVerticalPos = tempVerticalPos + verticalIntroPathSize/2;
            introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(centerPosition.x/2, 0, tempVerticalPos), 
                                    Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
            tempSizeData = introPath.GetComponent<SizeData>();
            introPath.transform.localScale = tempSizeData.referenceScale * (Mathf.Abs(verticalIntroPathSize) / tempSizeData.occupiedDistance.x);
        } else if(startDirection == "right") {
            rotation_Y = 180;
            introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(centerPosition.x/2, 0, centerPosition.z), 
                                    Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
            tempSizeData = introPath.GetComponent<SizeData>();
            introPath.transform.localScale = tempSizeData.referenceScale * (Mathf.Abs(multipleEnemyHolder.transform.position.x - 
                                            tempGameObject.transform.position.x) / tempSizeData.occupiedDistance.x);
        } else if(startDirection == "left") {
            rotation_Y = 0;
            introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(centerPosition.x/2, 0, centerPosition.z), 
                                    Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
            tempSizeData = introPath.GetComponent<SizeData>();
            introPath.transform.localScale = tempSizeData.referenceScale * (Mathf.Abs(multipleEnemyHolder.transform.position.x - 
                                            tempGameObject.transform.position.x) / tempSizeData.occupiedDistance.x);
        }


        // Giving necessary data to multiple enemy holder.
        GameObject[] movementPaths = new GameObject[] {tempGameObject};
        EnemyHolderDataHub enemyHolderDataHub =  multipleEnemyHolder.GetComponent<EnemyHolderDataHub>();
        enemyHolderDataHub.TakePathsForDeletion(this, tempWaveLevel, introPath, movementPaths);
        enemyHolderDataHub.increaseChildrenShipsBy(totalEnemyCount);
        multipleEnemyHolder.AddComponent<EnemyShipMovementScript>().startMovingEnemies(1, introPath, movementPaths, introSpeed, movementSpeed);
    }


    void buildWave2(string startDirection, string oscillateStartDirection, int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, 
                    int enemiesPerLayer, float startUpperHeight, float verticalGapSize, float introSpeed, float movementSpeed){
        buildWave1(startDirection, oscillateStartDirection, enemyShipIndex, numberOfLayersToSkip, numberOfLayersToBuild, enemiesPerLayer, 
                startUpperHeight, verticalGapSize, introSpeed, movementSpeed);
    }


    void buildWave3(string startDirection, string oscillateStartDirection, int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, 
                    int enemiesPerLayer, float startUpperHeight, float verticalGapSize, float introSpeed, float movementSpeed){
        buildWave1(startDirection, oscillateStartDirection, enemyShipIndex, numberOfLayersToSkip, numberOfLayersToBuild, enemiesPerLayer, 
                startUpperHeight, verticalGapSize, introSpeed, movementSpeed);
    }


    void buildWave4(int enemyShipIndex,int numberOfLayersToSkip, int numberOfLayersToBuild, int enemiesPerLayer, float startUpperHeight, 
                    float verticalGapSize, float introSpeed, float movementSpeed){
        // index => which enemyShip to build
        for(int i = 0; i < numberOfLayersToBuild; i++) {
            buildWave1((i%2==0)?"left":"right", (i%2==0)?"right":"left", enemyShipIndex, i, 1, enemiesPerLayer, startUpperHeight, verticalGapSize, 
                        introSpeed, movementSpeed);
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