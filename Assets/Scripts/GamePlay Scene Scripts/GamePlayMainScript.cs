using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class GamePlayMainScript : MonoBehaviour
{
    // Serialized Variables
    public BackgroundData backgrounds;
    public CanvasData lifeBarCanvasData, gameLevelCanvasData;
    public FriendlyShipData friendlyShips;
    public int FPSTarget;
    public float defaultCameraOrthogonalSize;
    public float refrenceWidth;
    public float refrenceHeight;
    public ParticleSystem bgFogParticleSystem;
    public float lifeBarVerticalOffset, gameLevelVerticalOffset;
    public int lifeLvlLimit;
    public float friendlyShipIntroSpeed;
    public GameObject EnemyRebouncer;
    public Object enemyHolderObject;


    // Non-Serialized Variables
    GameObject currentFriendlySpaceShip;
    LBRTValues viewableScaleConstrains;
    Text gameLevelText;
    float resizeRatio; // Only for canvases. No need to resize other stuff
    ParticleSystem.MainModule bgFogPSMainModule;
    Vector3 fromIntroCoOrdinates, currIntroPosition, toIntroCoOrdinates;
    SizeData tempSizeData;
    bool shouldIntroduce = true;
    EnemyWaveBuilder enemyWaveBuilder;



    // Start is called before the first frame update
    void Start()
    {
        // Setting Background and Adjusting Camera size and EnemyRebouncer Size
        bgFogPSMainModule = bgFogParticleSystem.main;
        setBackground(backgrounds.startBgIndex);
        float screenHeight = Screen.currentResolution.height;
        float screenWidth = Screen.currentResolution.width;
        float refrenceAspect = refrenceWidth/refrenceHeight;
        float screenAspect = screenWidth/screenHeight;
        Debug.Log("Game Play");
        Debug.Log("refrenceWidth = " + refrenceWidth + " refrenceHeight = " + refrenceHeight + " refrenceAspect = " + refrenceAspect);
        Debug.Log("screenWidth = " + screenWidth + " screenHeight = " + screenHeight + " screenAspect = " + screenAspect);
        Debug.Log("cameraAspect = " + Camera.main.aspect);
        if(screenAspect >= 1) {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize*refrenceAspect*screenAspect;
            resizeRatio = (float) System.Math.Round(((refrenceAspect*screenAspect)/4) + 0.75f, 3);
        } else {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize*refrenceAspect/screenAspect;
            resizeRatio = (float) System.Math.Round(((refrenceAspect/screenAspect)/4) + 0.75f, 3);
        }
        Debug.Log("Resize Ratio = " + resizeRatio);
        // Sets the scale wise constrains of workspace on the background
        viewableScaleConstrains = new LBRTValues(-backgrounds.baseBackgroundWidth/2, -Camera.main.orthographicSize, 
                                                backgrounds.baseBackgroundWidth/2, Camera.main.orthographicSize);
        SizeData sizeData = EnemyRebouncer.GetComponent<SizeData>();
        EnemyRebouncer.transform.localScale = new Vector3(2*viewableScaleConstrains.right*sizeData.referenceScale.x/sizeData.occupiedDistance.x, 
                                                        EnemyRebouncer.transform.localScale.y, 
                                                        2*viewableScaleConstrains.top*sizeData.referenceScale.z/sizeData.occupiedDistance.z);



        // Disables VSync for custom Frame Rate and sets custom FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPSTarget;



        // Regarding Life Bar and game Level
        GameObject lifeBarCanvasObject = Instantiate(lifeBarCanvasData.CanvasObject , lifeBarCanvasData.Position, 
                                                    Quaternion.Euler(lifeBarCanvasData.Rotation)) as GameObject;
        GameObject gameLevelCanvasObject = Instantiate(gameLevelCanvasData.CanvasObject , gameLevelCanvasData.Position, 
                                                    Quaternion.Euler(gameLevelCanvasData.Rotation)) as GameObject;
        Vector3 tempPosition = lifeBarCanvasObject.transform.position;
        lifeBarCanvasObject.transform.localScale = new Vector3(lifeBarCanvasData.Scale.x*resizeRatio, lifeBarCanvasData.Scale.y*resizeRatio, lifeBarCanvasData.Scale.z);
        lifeBarCanvasObject.transform.SetPositionAndRotation(new Vector3(tempPosition.x, tempPosition.y, viewableScaleConstrains.bottom + 
                                                            (lifeBarCanvasObject.GetComponent<SizeData>().occupiedDistance.z*resizeRatio/2) + 
                                                            lifeBarVerticalOffset), lifeBarCanvasObject.transform.rotation);
        tempPosition = gameLevelCanvasObject.transform.position;
        gameLevelCanvasObject.transform.localScale = new Vector3(gameLevelCanvasData.Scale.x*resizeRatio, gameLevelCanvasData.Scale.y*resizeRatio, gameLevelCanvasData.Scale.z);
        gameLevelCanvasObject.transform.SetPositionAndRotation(new Vector3(tempPosition.x, tempPosition.y, viewableScaleConstrains.top - 
                                                            (gameLevelCanvasObject.GetComponent<SizeData>().occupiedDistance.z*resizeRatio/2) - 
                                                            gameLevelVerticalOffset), gameLevelCanvasObject.transform.rotation);
        gameLevelCanvasObject.transform.localScale = gameLevelCanvasData.Scale;
        Canvas lifeBarCanvas = lifeBarCanvasObject.GetComponent<Canvas>();
        Canvas gameLevelCanvas = gameLevelCanvasObject.GetComponent<Canvas>();
        lifeBarCanvas.worldCamera = Camera.main;
        gameLevelCanvas.worldCamera = Camera.main;
        gameLevelText = gameLevelCanvas.GetComponentInChildren<Text>();
        gameLevelText.text = "01";



        // Regarding Friendly Ship
        fromIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom - 5);
        currentFriendlySpaceShip = Instantiate(friendlyShips.FSList[friendlyShips.currentShipIndex].FriendlyShipObject, 
                                            fromIntroCoOrdinates, friendlyShips.FSList[friendlyShips.currentShipIndex].getRotation()) as GameObject;
        tempSizeData = currentFriendlySpaceShip.GetComponent<SizeData>();
        currentFriendlySpaceShip.transform.localScale = tempSizeData.defaultScaleForUse;
        fromIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom - 
                                        (tempSizeData.occupiedDistance.z*tempSizeData.defaultScaleForUse.z/tempSizeData.referenceScale.z)/2);
        toIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom + 
                                        (tempSizeData.occupiedDistance.z*tempSizeData.defaultScaleForUse.z/tempSizeData.referenceScale.z)/2);
        currIntroPosition = fromIntroCoOrdinates;
        currentFriendlySpaceShip.transform.SetPositionAndRotation(fromIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);
        currentFriendlySpaceShip.GetComponent<FriendlyShipDataHub>().TakeData(lifeBarCanvas, 0.1f, 1, lifeLvlLimit, friendlyShips.friendlyShieldData);
        
        friendlyShips.friendlyShieldData.friendlyShipTransform = currentFriendlySpaceShip.transform;
        friendlyShips.friendlyShieldData.friendlyShipHeight = (tempSizeData.occupiedDistance.z / tempSizeData.referenceScale.z) * 
                                                            currentFriendlySpaceShip.transform.localScale.z;
        


        // Regarding EnemyWaveBuilder
        enemyWaveBuilder = gameObject.GetComponent<EnemyWaveBuilder>();



        // Regarding
    }




    // Update is called once per frame
    void Update()
    {
        // For FPS
        if(Application.targetFrameRate != FPSTarget) {
            Application.targetFrameRate = FPSTarget;
        }
        

        // For Introducing Friendly SpcaeShip And Starting It's Functionalities
        if(shouldIntroduce) {
            currIntroPosition.z += friendlyShipIntroSpeed;
            currentFriendlySpaceShip.transform.SetPositionAndRotation(currIntroPosition, currentFriendlySpaceShip.transform.rotation);

            // On Intro Completion
            if(currentFriendlySpaceShip.transform.position.z >= toIntroCoOrdinates.z) {
                currentFriendlySpaceShip.transform.SetPositionAndRotation(toIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);
                shouldIntroduce = false;
                currentFriendlySpaceShip.GetComponent<FriendlyShipDataHub>().buildProperBullets();
                currentFriendlySpaceShip.GetComponent<FriendlyShipMovementScript>().TakeData(currentFriendlySpaceShip.transform, viewableScaleConstrains, 
                                        currentFriendlySpaceShip.transform.localScale, tempSizeData, friendlyShips.extraHorizontalPosition, 
                                        friendlyShips.extraVerticalPositionBottom, friendlyShips.extraVerticalPositionTop, 
                                        friendlyShips.heightOffset, friendlyShips.percentHeightAllowedForMovement);
                enemyWaveBuilder.startBuildingWaves(viewableScaleConstrains, enemyHolderObject);
            }
        }

    }


    // This function fixes the size of backgroud quad and sets it 
    void setBackground(int index) {
        if( backgrounds.BGMaterialList[index].width != backgrounds.baseBackgroundWidth) {
            backgrounds.BGMaterialList[index].height = backgrounds.baseBackgroundWidth*backgrounds.BGMaterialList[index].height/backgrounds.BGMaterialList[index].width;
            backgrounds.BGMaterialList[index].width = backgrounds.baseBackgroundWidth;
        }
        gameObject.transform.localScale = new Vector3(backgrounds.BGMaterialList[index].width, backgrounds.BGMaterialList[index].height, 5);
        Vector4 col = backgrounds.BGMaterialList[index].BGFogColorARGB;
        bgFogPSMainModule.startColor = new ParticleSystem.MinMaxGradient(new Color32((byte) col.x, (byte) col.y, (byte) col.z, (byte) col.w));
    }
}