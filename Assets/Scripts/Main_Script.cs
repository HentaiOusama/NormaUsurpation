using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LifeBarCanvasTransformData {
    public Object lifeBarCanvasObject;
    public Vector3 lifeBarCanvasCoOrdinates;
    public Vector3 lifeBarCanvasScale;
    public Quaternion lifeBarCanvasQuaternion;
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
    public int FPSTarget;
    public Camera mainCamera;
    public LifeBarCanvasTransformData LBCTData;
    public CurrentShipTransformData CSTData;
    public Vector3 fromIntroCoOrdinates, currIntroPosition, toIntroCoOrdinates;
    public float friendlyShipIntroSpeed;
    public int lifeLvlLimit;
    public CurrentBulletData currentBullet;




    // Non-Serialized Variables
    GameObject lifeBarCanvasObject;
    GameObject currentFriendlySpaceShip;
    Canvas canvas;
    Slider slider;
    Text lifeLevelText;
    int k = 0;
    int lifeLevel = 1;
    float sliderValue = 0.1f;
    bool shouldIntroduce = true;



    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Disables VSync for custom Frame Rate
        Application.targetFrameRate = FPSTarget; // Sets custom FPS


        // Regarding Friendly Ship
        currentFriendlySpaceShip = Instantiate(CSTData.currentShipObject, CSTData.currentShipCoOrdinates, CSTData.currentShipQuaternion) as GameObject;
        currentFriendlySpaceShip.transform.localScale = CSTData.currentShipScale;
        currIntroPosition = fromIntroCoOrdinates;
        currentFriendlySpaceShip.transform.SetPositionAndRotation(fromIntroCoOrdinates, currentFriendlySpaceShip.transform.rotation);


        // Regarding Life Bar
        lifeBarCanvasObject = Instantiate(LBCTData.lifeBarCanvasObject, LBCTData.lifeBarCanvasCoOrdinates, LBCTData.lifeBarCanvasQuaternion) as GameObject;
        canvas = lifeBarCanvasObject.GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        slider = canvas.GetComponentInChildren<Slider>();
        slider.value = 0.1f;
        sliderValue = 0.1f;
        lifeLevelText = canvas.GetComponentInChildren<Text>();
        lifeLevelText.text = "01";


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
        sliderValue += 0.1f;
        if(sliderValue > 1.09f) {
            if(lifeLevel < lifeLvlLimit) {
                lifeLevel++;
                sliderValue = 0.1f;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                currentFriendlySpaceShip.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                sliderValue = 1;
            }
        }
        slider.value = sliderValue;
    }

    public void decreaseLifeOfFriendlyShip() {
        sliderValue -= 0.1f;
        if(sliderValue < 0.1) {
            if(lifeLevel == 1) {
                gameOver();
                return;
            }
            if(lifeLevel > 1) {
                lifeLevel--;
                sliderValue = 1;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                currentFriendlySpaceShip.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                sliderValue = 1;
            }
        }
        slider.value = sliderValue;
    }


    // Ends the Game  <-- Not Yet Complete
    public void gameOver() {
        FriendlyBulletBuilder.stopBuildingBullets();
        Destroy(currentFriendlySpaceShip);
    }
}