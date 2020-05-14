using System.Collections;
using System.Collections.Generic;
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
    GameObject currentFriendlySpacceShip;
    Canvas canvas;
    Slider slider;
    Text lifeLevelText;
    int k = 0;
    int lifeLevel = 1;
    bool shouldIntroduce = true;



    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Disables VSync for custom Frame Rate
        Application.targetFrameRate = FPSTarget; // Sets custom FPS


        // Regarding Friendly Ship
        currentFriendlySpacceShip = Instantiate(CSTData.currentShipObject, CSTData.currentShipCoOrdinates, CSTData.currentShipQuaternion) as GameObject;
        currentFriendlySpacceShip.transform.localScale = CSTData.currentShipScale;
        currIntroPosition = fromIntroCoOrdinates;
        currentFriendlySpacceShip.transform.SetPositionAndRotation(fromIntroCoOrdinates, currentFriendlySpacceShip.transform.rotation);


        // Regarding Life Bar
        lifeBarCanvasObject = Instantiate(LBCTData.lifeBarCanvasObject, LBCTData.lifeBarCanvasCoOrdinates, LBCTData.lifeBarCanvasQuaternion) as GameObject;
        canvas = lifeBarCanvasObject.GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        slider = canvas.GetComponentInChildren<Slider>();
        slider.value = 0;
        lifeLevelText = canvas.GetComponentInChildren<Text>();
        lifeLevelText.text = "01";


        // Regarding building Bullets
        currentBullet = new CurrentBulletData(new Transform[]{currentFriendlySpacceShip.transform.GetChild(1).GetChild(0), 
                                                            currentFriendlySpacceShip.transform.GetChild(1).GetChild(1), 
                                                            currentFriendlySpacceShip.transform.GetChild(1).GetChild(2), 
                                                            currentFriendlySpacceShip.transform.GetChild(1).GetChild(3), 
                                                            currentFriendlySpacceShip.transform.GetChild(1).GetChild(4)});
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
            currentFriendlySpacceShip.transform.SetPositionAndRotation(currIntroPosition, currentFriendlySpacceShip.transform.rotation);
            if(currentFriendlySpacceShip.transform.position.z >= toIntroCoOrdinates.z) {
                currentFriendlySpacceShip.transform.SetPositionAndRotation(toIntroCoOrdinates, currentFriendlySpacceShip.transform.rotation);
                shouldIntroduce = false;
                BulletBuilder.buildBullet(lifeLevel, currentBullet.getSpwanPoints());
                BulletBuilder.startBuildingBullets();
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
        slider.value += 0.1f;
        if(slider.value >= 1) {
            if(lifeLevel < lifeLvlLimit) {
                lifeLevel++;
                slider.value = 0;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                BulletBuilder.buildBullet(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                slider.value = 1;
            }
        }
    }

    public void decreaseLifeOfFriendlyShip() {
        slider.value -= 0.1f;
        if(slider.value <= 0) {
            if(lifeLevel == 1) {
                gameOver();
                return;
            }
            if(lifeLevel > 1) {
                lifeLevel--;
                slider.value = 1;
                if(lifeLevel < 10) {
                    lifeLevelText.text = "0" + lifeLevel.ToString();
                } else {
                    lifeLevelText.text = lifeLevel.ToString();
                }
                BulletBuilder.buildBullet(lifeLevel, currentBullet.getSpwanPoints());
            } else {
                slider.value = 1;
            }
        }
    }


    // Ends the Game  <-- Not Yet Complete
    public void gameOver() {

    }
}