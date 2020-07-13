using UnityEngine;
using PathCreation;


public class EnemyWaveBuilder : MonoBehaviour
{
    // Serialized variables
    public WaveBuildingData waveBuildingData;
    public int currentWaveLevel;

    // Non-Serialized Variables
    LBRTValues viewableScreenConstrains;
    Object enemyHolderObject;
    bool shouldBuildEnemyWave = false;
    int totalNumberOfActiveEnemyHolders = 0;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(waveBuildingData.waveLevel != currentWaveLevel) {
            waveBuildingData.waveLevel = currentWaveLevel;
        }

        if(waveBuildingData.waveLevel > waveBuildingData.maxWaves) {
            shouldBuildEnemyWave = false;
        }

        if(shouldBuildEnemyWave && (waveBuildingData.waveLevel <= waveBuildingData.maxWaves)) {
            shouldBuildEnemyWave = false;
            Debug.Log("Building Wave Level : " + waveBuildingData.waveLevel);
            switch(waveBuildingData.waveLevel) {
                case 1:
                    buildWave1("top", "right", 0, 1f, 0, 5, 7, viewableScreenConstrains.top - 4f, 0, 0, 1, 2, 4);
                    break;
                
                case 2: 
                    buildWave2(0, 1f, 0, 6, 7, viewableScreenConstrains.top - 4f, 0, 0, 1, 2, 4);
                    break;

                case 3:
                    buildWave3("top", 0, 1f, 4, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    buildWave3("right", 0, 1f, 4, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    buildWave3("left", 0, 1f, 4, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    break;

                case 4:
                    buildWave3("top", 1, 0.8f, 4, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    break;

                case 5:
                    buildWave3("right", 0, 1f, 2, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    buildWave3("left", 0, 1f, 2, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    buildWave3("top", 1, 0.8f, 3, 10f, 2, new minMaxVariable(3.5f, 7.5f));
                    break;
            }
            Debug.Log("Total Number of EnemyHolders Built =  " + totalNumberOfActiveEnemyHolders);
        }
    }



    // Not fully complete
    void buildWave1(string buildDirection, string oscillateStartDirection, int enemyShipIndex, float percentageOfDefaultScaleToUse,
                    int numberOfLayersToSkip, int numberOfLayersToBuild, int enemiesPerLayer, float startUpperHeight, float startLeftGap, 
                    float endRightGap, float verticalGapSize, float introSpeed, float movementSpeed){
        
        // index => which enemyShip to build.
        // buildDirection can only be left, right or top. (It means where to build the enemyShips before introduction).
        // oscillateStartDirection can only be left or right.
        int totalEnemyCount = 0;
        buildDirection = buildDirection.ToLower();
        oscillateStartDirection = oscillateStartDirection.ToLower();
        float tempStartUpperHeight = startUpperHeight;


        float buildWindowWidth, enemyShipWidth, enemyShipHeight, horizontalGapSize, startPosVertical = startUpperHeight;
        float horizontalBuildPos, startPosHorizontal, endPosHorizontal;
        startPosHorizontal = viewableScreenConstrains.left + startLeftGap;
        endPosHorizontal = viewableScreenConstrains.right - endRightGap;
        buildWindowWidth = endPosHorizontal - startPosHorizontal;
        Vector3 centerPosition;
        GameObject tempGameObject, multipleEnemyHolder;
        SizeData tempSizeData;
        

        // Get data regarding the ship we are going to build by instantiating a temporary enemyShip to get SizeData and then delete it.
        tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(0, 25, 0), Quaternion.Euler(0, 0, 0)) as GameObject; // Temporary Instantiation
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.defaultScaleForUse * percentageOfDefaultScaleToUse;
        enemyShipWidth = (tempSizeData.occupiedDistance.x/tempSizeData.referenceScale.x)*tempGameObject.transform.localScale.x;
        enemyShipHeight = (tempSizeData.occupiedDistance.z/tempSizeData.referenceScale.z)*tempGameObject.transform.localScale.z;
        Destroy(tempGameObject);
        horizontalGapSize = (buildWindowWidth/enemiesPerLayer) - enemyShipWidth;


        // Calculating Centre and Start Positions Positions
        if(buildDirection == "top") {
            startPosVertical = viewableScreenConstrains.top + numberOfLayersToBuild*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(startPosHorizontal + (buildWindowWidth/2), 0, (viewableScreenConstrains.top + startPosVertical)/2);
        } else if(buildDirection == "right") {
            startPosVertical -= numberOfLayersToSkip*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(viewableScreenConstrains.right + (buildWindowWidth/2), 0, 
                                        startPosVertical - (((enemyShipHeight + verticalGapSize)/2) * numberOfLayersToBuild));
        } else if(buildDirection == "left") {
            startPosVertical -= numberOfLayersToSkip*(enemyShipHeight + verticalGapSize);
            centerPosition = new Vector3(viewableScreenConstrains.left - (buildWindowWidth/2), 0, 
                                        startPosVertical - (((enemyShipHeight + verticalGapSize)/2) * numberOfLayersToBuild));
        } else {
            Debug.LogError("Error Building Wave 1. Input buildDirection is not acceptable.");
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
            if(buildDirection == "top"){
                horizontalBuildPos = startPosHorizontal;
            }else if(buildDirection == "right") {
                horizontalBuildPos = viewableScreenConstrains.right;
            } else if(buildDirection == "left") {
                horizontalBuildPos = viewableScreenConstrains.left;
            } else {
                Debug.LogError("Error Building Wave 1. Input buildDirection is not acceptable.");
                return;
            }

            // Building enemies of current Layer
            for(int j = 0; j < enemiesPerLayer; j++) {
                if(buildDirection == "top") {
                    horizontalBuildPos += horizontalGapSize/2;
                    horizontalBuildPos += enemyShipWidth/2;
                } else if(buildDirection == "right") {
                    horizontalBuildPos += horizontalGapSize/2;
                    horizontalBuildPos += enemyShipWidth/2;
                } else if(buildDirection == "left") {
                    horizontalBuildPos -= horizontalGapSize/2;
                    horizontalBuildPos -= enemyShipWidth/2;
                }

                tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(horizontalBuildPos, 0, startPosVertical),
                                            Quaternion.Euler(0, 0, 0)) as GameObject;
                tempGameObject.transform.localScale = tempGameObject.GetComponent<SizeData>().defaultScaleForUse*percentageOfDefaultScaleToUse;
                tempGameObject.transform.SetParent(multipleEnemyHolder.transform);
                tempGameObject.GetComponent<EnemyShipDataHub>().TakeHolder(multipleEnemyHolder);

                if(buildDirection == "top") {
                    horizontalBuildPos += horizontalGapSize/2;
                    horizontalBuildPos += enemyShipWidth/2;
                } else if(buildDirection == "right") {
                    horizontalBuildPos += horizontalGapSize/2;
                    horizontalBuildPos += enemyShipWidth/2;
                } else if(buildDirection == "left") {
                    horizontalBuildPos -= horizontalGapSize/2;
                    horizontalBuildPos -= enemyShipWidth/2;
                }
                totalEnemyCount++;
            }

            
            startPosVertical -= enemyShipHeight/2;
            startPosVertical -= verticalGapSize/2;
        }


        // Building Oscillating Path
        float rotation_Y = 0;
        float tempVerticalPos = centerPosition.z; // Initialized for buildDirection != "top"
        float tempHorizontalPos = startPosHorizontal + (buildWindowWidth/2);
        if(oscillateStartDirection == "right") {
            rotation_Y = 180;
        } else if(oscillateStartDirection == "left") {
            rotation_Y = 0;
        } else {
            Debug.LogError("Invalid StartOscillationDirection : " + oscillateStartDirection);
        }
        if(buildDirection == "top") {
            tempVerticalPos = tempStartUpperHeight - ((numberOfLayersToSkip * (enemyShipHeight + verticalGapSize)) + 
                                    ((numberOfLayersToBuild * (enemyShipHeight + verticalGapSize)) / 2));
        }
        tempGameObject = Instantiate(waveBuildingData.movementPathObjects[1], new Vector3(tempHorizontalPos, 0, tempVerticalPos), 
                                    Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.referenceScale * (horizontalGapSize/tempSizeData.occupiedDistance.x);


        // Building Intro Path
        GameObject introPath = null;
        float verticalIntroPathSize = 0;
        if(buildDirection == "top") {
            rotation_Y = 90;
            verticalIntroPathSize = Mathf.Abs(centerPosition.z - tempVerticalPos); // here tempVerticalPos was z pos of build of oscillating path
            tempVerticalPos = tempVerticalPos + verticalIntroPathSize/2; // here tempVerticalPos becomes z pos of build of intro path
            tempHorizontalPos = centerPosition.x;
        } else if(buildDirection == "right") {
            rotation_Y = 180;
            verticalIntroPathSize = Mathf.Abs(tempGameObject.transform.position.x - multipleEnemyHolder.transform.position.x);
            tempVerticalPos = centerPosition.z;
            tempHorizontalPos = tempGameObject.transform.position.x + (verticalIntroPathSize/2);
        } else if(buildDirection == "left") {
            rotation_Y = 0;
            verticalIntroPathSize = Mathf.Abs(tempGameObject.transform.position.x - multipleEnemyHolder.transform.position.x);
            tempVerticalPos = centerPosition.z;
            tempHorizontalPos = tempGameObject.transform.position.x - (verticalIntroPathSize/2);
        } else {
            Debug.Log("Invalid buildDirection : " + buildDirection);
        }
        introPath = Instantiate(waveBuildingData.movementPathObjects[0], new Vector3(tempHorizontalPos, 0, tempVerticalPos), 
                                Quaternion.Euler(0, rotation_Y, 0)) as GameObject;
        tempSizeData = introPath.GetComponent<SizeData>();
        introPath.transform.localScale = tempSizeData.referenceScale * (verticalIntroPathSize / tempSizeData.occupiedDistance.x);


        // Giving necessary data to multiple enemy holder.
        GameObject[] movementPaths = new GameObject[] {tempGameObject};
        EnemyHolderDataHub enemyHolderDataHub =  multipleEnemyHolder.GetComponent<EnemyHolderDataHub>();
        enemyHolderDataHub.TakePathsForDeletion(this, currentWaveLevel, introPath, movementPaths);
        enemyHolderDataHub.increaseChildrenShipsBy(totalEnemyCount);
        multipleEnemyHolder.AddComponent<EnemyShipMovementScript>().startMovingEnemies(1, introPath, movementPaths, introSpeed, movementSpeed);
    }


    void buildWave2(int enemyShipIndex, float percentageOfDefaultScaleToUse, int numberOfLayersToSkip, int numberOfLayersToBuild, 
                    int enemiesPerLayer, float startUpperHeight, float startLeftGap, float endRightGap, float verticalGapSize, 
                    float introSpeed, float movementSpeed){
        // index => which enemyShip to build
        for(int i = 0; i < numberOfLayersToBuild; i++) {
            buildWave1((i%2==0)?"left":"right", (i%2==0)?"right":"left", enemyShipIndex, percentageOfDefaultScaleToUse, i, 1, enemiesPerLayer, 
                        startUpperHeight, startLeftGap, endRightGap, verticalGapSize, introSpeed, movementSpeed);
        }
    }


    void buildWave3(string buildDirection, int enemyShipIndex, float percentageOfDefaultScaleToUse, int numberOfShipsToBuild, float introSpeed, 
                    float introDuration, minMaxVariable minMaxMovementSpeed) {
        
        Vector3 velocityDirection = new Vector3();
        buildDirection = buildDirection.ToLower();
        float waitBeforeNextEnemy = 0.0f;
        
        
        // Get data regarding the ship we are going to build by instantiating a temporary enemyShip to get SizeData and then delete it.
        GameObject tempGameObject;
        SizeData tempSizeData;
        float enemyShipWidth, enemyShipHeight;
        tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], new Vector3(0, 25, 0), Quaternion.Euler(0, 0, 0)) as GameObject; // Temporary Instantiation
        tempSizeData = tempGameObject.GetComponent<SizeData>();
        tempGameObject.transform.localScale = tempSizeData.defaultScaleForUse*percentageOfDefaultScaleToUse;
        enemyShipWidth = (tempSizeData.occupiedDistance.x/tempSizeData.referenceScale.x)*tempGameObject.transform.localScale.x;
        enemyShipHeight = (tempSizeData.occupiedDistance.z/tempSizeData.referenceScale.z)*tempGameObject.transform.localScale.z;
        Destroy(tempGameObject);


        // Calculating centrePosition of multipleEnemyHolder
        Vector3 centerPosition = new Vector3();
        if(buildDirection == "top") {
            centerPosition = new Vector3(0, 0, viewableScreenConstrains.top + enemyShipHeight/2);
        } else if(buildDirection == "left") {
            centerPosition = new Vector3(viewableScreenConstrains.left - enemyShipWidth/2, 0, viewableScreenConstrains.top/2);
        } else if(buildDirection == "right") {
            centerPosition = new Vector3(viewableScreenConstrains.right + enemyShipWidth/2, 0, viewableScreenConstrains.top/2);
        }

        // Building all enemies
        for(int i = 0; i < numberOfShipsToBuild; i++) {
            GameObject multipleEnemyHolder = Instantiate(enemyHolderObject, centerPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            if(buildDirection == "top") {
                velocityDirection = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, -6f));
            } else if(buildDirection == "left") {
                velocityDirection = new Vector3(Random.Range(6f, 10f), 0, Random.Range(-10f, -10f));
            } else if(buildDirection == "right") {
                velocityDirection = new Vector3(Random.Range(-10f, -6f), 0, Random.Range(-10f, -10f));
            } else {
                Debug.LogError("Invalid buildDirection : " + buildDirection);
            }
            velocityDirection = velocityDirection.normalized;

            tempGameObject = Instantiate(waveBuildingData.enemyObjects[enemyShipIndex], centerPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            tempGameObject.transform.localScale = tempGameObject.GetComponent<SizeData>().defaultScaleForUse*percentageOfDefaultScaleToUse;
            tempGameObject.transform.SetParent(multipleEnemyHolder.transform);
            tempGameObject.GetComponent<EnemyShipDataHub>().TakeHolder(multipleEnemyHolder);

            // Giving necessary data to multiple enemy holder.
            EnemyHolderDataHub enemyHolderDataHub =  multipleEnemyHolder.GetComponent<EnemyHolderDataHub>();
            enemyHolderDataHub.TakePathsForDeletion(this, currentWaveLevel, null, null);
            enemyHolderDataHub.increaseChildrenShipsBy(1);
            enemyHolderDataHub.enableRebouncingCollider(buildColliderSize(tempGameObject));
            multipleEnemyHolder.AddComponent<EnemyShipMovementScript>().startMovingEnemies(2, velocityDirection, waitBeforeNextEnemy, introSpeed, 
                                                                                        introDuration, minMaxMovementSpeed);
            waitBeforeNextEnemy += 0.4f*introDuration;
            totalNumberOfActiveEnemyHolders++;
        }
    }
    




    public void startBuildingWaves(LBRTValues viewableScreenConstrains, Object enemyHolderObject) {
        this.viewableScreenConstrains = viewableScreenConstrains;
        this.enemyHolderObject = enemyHolderObject;
        shouldBuildEnemyWave = true;
    }

    public void buildNextWave() {
        currentWaveLevel++;
        shouldBuildEnemyWave = true;
    }

    public void allChilderEnemiesDefeated() {
        totalNumberOfActiveEnemyHolders--;
        if(totalNumberOfActiveEnemyHolders == 0) {
            buildNextWave();
        }
    }

    public Vector3 buildColliderSize(GameObject tempGameObject) {
        Vector3 retVec3, tempVec3;
        tempVec3 = tempGameObject.transform.localScale;
        SizeData tempSizeData = tempGameObject.GetComponent<SizeData>();
        retVec3 = new Vector3(tempVec3.x*tempSizeData.occupiedDistance.x/tempSizeData.referenceScale.x, 
                            tempVec3.y*tempSizeData.occupiedDistance.y/tempSizeData.referenceScale.y, 
                            tempVec3.z*tempSizeData.occupiedDistance.z/tempSizeData.referenceScale.z);
        retVec3 += new Vector3(0.2f, 0.2f, 0.2f);
        return retVec3;
    }
}