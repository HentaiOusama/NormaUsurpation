using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class BackgroundElement {
    public Material material;
    public float width;
    public float height;
}

[System.Serializable]
public class BackgroundData {
    public BackgroundElement[] BGMaterialList;
    public int startBgIndex;
    public float baseBackgroundWidth;
}

[System.Serializable]
public class LBRTValues{
    public float left, bottom, right, top;
    public LBRTValues(float left, float bottom, float right, float top) {
        this.left = left;
        this.bottom = bottom;
        this.right = right;
        this.top = top;
    }
}

[System.Serializable]
public class CanvasData {
    public Object CanvasObject;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 Rotation;
}

[System.Serializable]
public class FriendlyShipElement {
    ///// To get the scale, use GetComponent<SizeData>().defaultScaleForUse /////
    public Object FriendlyShipObject;
    public Vector3 Rotation;
    Vector3 Scale;
    Quaternion retRotation;
    public FriendlyShipElement(){
        retRotation = Quaternion.Euler(Rotation);
    }
    public Quaternion getRotation() {
        return retRotation;
    }
}

[System.Serializable]
public class FriendlyShipData {
    public FriendlyShipElement[] FSList;
    public int startShipIndex;
    public int currentShipIndex = 0;
    public float FSIntroSpeed;   

}

[System.Serializable]
public class CurrentBulletData {
    public GameObject currentBulletObject;
    Transform[] spwanPoints;

    public CurrentBulletData(Transform[] spwanPoints) {
        this.spwanPoints = spwanPoints;
    }

    public Transform[] getSpwanPoints() {
        return spwanPoints;
    }
}



public class Main_Script : MonoBehaviour
{
    // Serialized Variables
    public BackgroundData backgrounds;
    public CanvasData lifeBarCanvasData, gameLevelCanvasData;
    public FriendlyShipData friendlyShips;
    public CurrentBulletData currentBullet;
    public int FPSTarget;
    public Camera mainCamera;
    public float defaultCameraOrthogonalSize;
    public float refrenceWidth;
    public float refrenceHeight;
    public float lifeBarVerticalOffset, gameLevelVerticalOffset;
    public int lifeLvlLimit;
    public float friendlyShipIntroSpeed;


    // Non-Serialized Variables
    GameObject currentFriendlySpaceShip;
    LBRTValues viewableScaleConstrains;
    Slider lifeLevelSlider;
    Text lifeLevelText;
    Text gameLevelText;
    float resizeRatio; // Only for canvases. No need to resize other stuff
    int k = 0;
    int lifeLevel = 1;
    float lifeLevelSliderValue = 0.1f;
    Vector3 fromIntroCoOrdinates, currIntroPosition, toIntroCoOrdinates;
    bool shouldIntroduce = true;



    // Start is called before the first frame update
    void Start()
    {
        // Setting Background and Adjusting Camera size
        setBackground(backgrounds.startBgIndex);
        float refrenceAspect = refrenceWidth/refrenceHeight;
        float screenHeight = Screen.currentResolution.height;
        float screenWidth = Screen.currentResolution.width;
        float screenAspect = screenWidth/screenHeight;
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
        lifeBarCanvas.worldCamera = mainCamera;
        gameLevelCanvas.worldCamera = mainCamera;
        lifeLevelSlider = lifeBarCanvas.GetComponentInChildren<Slider>();
        lifeLevelText = lifeBarCanvas.GetComponentInChildren<Text>();
        gameLevelText = gameLevelCanvas.GetComponentInChildren<Text>();
        lifeLevelSlider.value = 0.1f;
        lifeLevelSliderValue = 0.1f;
        lifeLevelText.text = "01";
        gameLevelText.text = "01";



        // Regarding Friendly Ship
        fromIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom - 5);
        currentFriendlySpaceShip = Instantiate(friendlyShips.FSList[friendlyShips.currentShipIndex].FriendlyShipObject, 
                                            fromIntroCoOrdinates, friendlyShips.FSList[friendlyShips.currentShipIndex].getRotation()) as GameObject;
        SizeData tempSizeData = currentFriendlySpaceShip.GetComponent<SizeData>();
        currentFriendlySpaceShip.transform.localScale = tempSizeData.defaultScaleForUse;
        fromIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom - 
                                        (tempSizeData.occupiedDistance.z*tempSizeData.defaultScaleForUse.z/tempSizeData.referenceScale.z)/2);
        toIntroCoOrdinates = new Vector3(0, 0, viewableScaleConstrains.bottom + 
                                        (tempSizeData.occupiedDistance.z*tempSizeData.defaultScaleForUse.z/tempSizeData.referenceScale.z)/2);
        currIntroPosition = fromIntroCoOrdinates;
        currentFriendlySpaceShip.transform.SetPositionAndRotation(fromIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);



        // Regarding Building Friendly Bullets
        currentBullet = new CurrentBulletData(new Transform[]{currentFriendlySpaceShip.transform.GetChild(1).GetChild(0), 
                                                            currentFriendlySpaceShip.transform.GetChild(1).GetChild(1), 
                                                            currentFriendlySpaceShip.transform.GetChild(1).GetChild(2), 
                                                            currentFriendlySpaceShip.transform.GetChild(1).GetChild(3), 
                                                            currentFriendlySpaceShip.transform.GetChild(1).GetChild(4)});
    }




    // Update is called once per frame
    void Update()
    {
        // For FPS
        if(Application.targetFrameRate != FPSTarget) {
            Application.targetFrameRate = FPSTarget;
        }
        

        // For Introducing Friendly SpcaeShip
        if(shouldIntroduce) {
            currIntroPosition.z += friendlyShipIntroSpeed;
            currentFriendlySpaceShip.transform.SetPositionAndRotation(currIntroPosition, currentFriendlySpaceShip.transform.rotation);
            if(currentFriendlySpaceShip.transform.position.z >= toIntroCoOrdinates.z) {
                currentFriendlySpaceShip.transform.SetPositionAndRotation(toIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);
                shouldIntroduce = false;
                currentFriendlySpaceShip.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBullet.getSpwanPoints());
                FriendlyBulletBuilder.startBuildingBullets();
            }
        }


        // This if else Block has to be ......DELETED......
        if(k == 50) {
            increaseLifeOfFriendlyShip();
            k = 0;
        } else {
            k++;
        }
    }


    // Slider Value Increaser
    public void increaseLifeOfFriendlyShip() {
        lifeLevelSliderValue += 0.1f;
        if(lifeLevelSliderValue > 1.09f) {
            if(lifeLevel < lifeLvlLimit) {
                lifeLevel++;
                lifeLevelSliderValue = 0.1f;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                currentFriendlySpaceShip.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                lifeLevelSliderValue = 1;
            }
        }
        lifeLevelSlider.value = lifeLevelSliderValue;
    }

    public void decreaseLifeOfFriendlyShip() {
        lifeLevelSliderValue -= 0.1f;
        if(lifeLevelSliderValue < 0.1) {
            if(lifeLevel == 1) {
                gameOver();
                return;
            }
            if(lifeLevel > 1) {
                lifeLevel--;
                lifeLevelSliderValue = 1;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                currentFriendlySpaceShip.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                lifeLevelSliderValue = 1;
            }
        }
        lifeLevelSlider.value = lifeLevelSliderValue;
    }


    // Ends the Game  <-- Not Yet Complete
    public void gameOver() {
        FriendlyBulletBuilder.stopBuildingBullets();
        Destroy(currentFriendlySpaceShip);
    }


    // This function fixes the size of backgroud quad and sets it 
    public void setBackground(int index) {
        if( backgrounds.BGMaterialList[index].width != backgrounds.baseBackgroundWidth) {
            backgrounds.BGMaterialList[index].height = backgrounds.baseBackgroundWidth*backgrounds.BGMaterialList[index].height/backgrounds.BGMaterialList[index].width;
            backgrounds.BGMaterialList[index].width = backgrounds.baseBackgroundWidth;
        }
        gameObject.transform.localScale = new Vector3(backgrounds.BGMaterialList[index].width, backgrounds.BGMaterialList[index].height, 5);
    }

}