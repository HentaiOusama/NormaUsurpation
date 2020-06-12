using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class Backgrounds {
    public Material material;
    public float width;
    public float height;
}

[System.Serializable]
public class CanvasData {
    public Object CanvasObject;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 Rotation;
}

[System.Serializable]
public class CurrentShipTransformData {
    public Object currentShipObject;
    public Vector3 currentShipCoOrdinates;
    public Vector3 currentShipScale;
    public Quaternion currentShipQuaternion;
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
    public Backgrounds[] backgrounds;
    public CanvasData lifeBarCanvasData, gameLevelCanvasData;
    public CurrentShipTransformData CSTData;
    public CurrentBulletData currentBullet;
    public int FPSTarget;
    public Camera mainCamera;
    public float defaultCameraOrthogonalSize;
    public int startBgIndex;
    public float baseBackgroundWidth;
    public Vector3 fromIntroCoOrdinates, currIntroPosition, toIntroCoOrdinates;
    public float friendlyShipIntroSpeed;
    public int lifeLvlLimit;




    // Non-Serialized Variables
    GameObject currentFriendlySpaceShip;
    Slider lifeLevelSlider;
    Text lifeLevelText;
    Text gameLevelText;
    int k = 0;
    int lifeLevel = 1;
    float lifeLevelSliderValue = 0.1f;
    bool shouldIntroduce = true;



    // Start is called before the first frame update
    void Start()
    {
        // Setting Background and Adjusting Camera size
        setBackground(startBgIndex);
        float refrenceHeight = Screen.height;
        float refrenceWidth = Screen.width;
        float refrenceAspect = refrenceWidth/refrenceHeight;
        float screenHeight = Screen.currentResolution.height;
        float screenWidth = Screen.currentResolution.width;
        float screenAspect = screenWidth/screenHeight;
        Debug.Log("refrenceWidth = " + refrenceWidth + " refrenceHeight = " + refrenceHeight + " refrenceAspect = " + refrenceAspect);
        Debug.Log("screenWidth = " + screenWidth + " screenHeight = " + screenHeight + " screenAspect = " + screenAspect);
        Debug.Log("cameraAspect = " + Camera.main.aspect);
        if(screenAspect >= 1) {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize/screenAspect/refrenceAspect;
        } else {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize*screenAspect/refrenceAspect;
        }


        QualitySettings.vSyncCount = 0; // Disables VSync for custom Frame Rate
        Application.targetFrameRate = FPSTarget; // Sets custom FPS


        // Regarding Friendly Ship
        currentFriendlySpaceShip = Instantiate(CSTData.currentShipObject, CSTData.currentShipCoOrdinates, CSTData.currentShipQuaternion) as GameObject;
        currentFriendlySpaceShip.transform.localScale = CSTData.currentShipScale;
        currIntroPosition = fromIntroCoOrdinates;
        currentFriendlySpaceShip.transform.SetPositionAndRotation(fromIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);


        // Regarding Life Bar
        GameObject lifeBarCanvasObject = Instantiate(lifeBarCanvasData.CanvasObject , lifeBarCanvasData.Position, 
                                                    Quaternion.Euler(lifeBarCanvasData.Rotation)) as GameObject;
        GameObject gameLevelCanvasObject = Instantiate(gameLevelCanvasData.CanvasObject , gameLevelCanvasData.Position, 
                                                    Quaternion.Euler(gameLevelCanvasData.Rotation)) as GameObject;
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


        // Regarding building Bullets
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
        if(backgrounds[index].width != baseBackgroundWidth) {
            backgrounds[index].height = baseBackgroundWidth*backgrounds[index].height/backgrounds[index].width;
            backgrounds[index].width = baseBackgroundWidth;
        }
        gameObject.transform.localScale = new Vector3(backgrounds[index].width, backgrounds[index].height, 5);
    }

}