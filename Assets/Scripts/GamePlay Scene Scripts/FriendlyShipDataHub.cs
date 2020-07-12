using UnityEngine;
using UnityEngine.UI;

public class FriendlyShipDataHub : MonoBehaviour
{
    // Serialized variables
    public float healthPerSubLifeLevel;
    public Object destroyVFXEffect;

    // Non-Serialized variables
    Slider lifeLevelSlider;
    float lifeLevelSliderValue;
    int lifeLevel;
    int lifeLvlLimit;
    float currentHealthForCurrentSubLifeLevel;
    Text lifeLevelText;
    FriendlyShieldData friendlyShieldData;
    GameObject shieldGameObject;
    bool isShieldActive = false;



    // Start is called before the first frame update
    void Start()
    {
        currentHealthForCurrentSubLifeLevel = healthPerSubLifeLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTriggerEnter is called when collision starts
    void OnTriggerEnter(Collider other) {
        string colliderTag = other.gameObject.tag;

        if(colliderTag == "DropItemLife") 
        {
            increaseLifeOfFriendlyShip();
        } 
        else if(colliderTag == "DropItemArmour") 
        {
            if(!isShieldActive) {
                shieldGameObject = Instantiate(friendlyShieldData.friendlyShieldObject , new Vector3(0, 50, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                shieldGameObject.GetComponent<FriendlyShieldHandler>().TakeData(this, friendlyShieldData);
                isShieldActive = true;
            } else {
                shieldGameObject.GetComponent<FriendlyShieldHandler>().ResetDuration();
            }
        }

        if(colliderTag != "FriendlyBullet" && colliderTag != "EnemyShip") {
            Destroy(other.gameObject);
        }
    }



    // Fetches data from Main Script
    public void TakeData(Canvas lifeBarCanvas, float lifeLevelSliderValue, int lifeLevel, int lifeLvlLimit, FriendlyShieldData friendlyShieldData) {
        lifeLevelSlider = lifeBarCanvas.GetComponentInChildren<Slider>();
        lifeLevelText = lifeBarCanvas.GetComponentInChildren<Text>();
        lifeLevelSlider.value = lifeLevelSliderValue;
        lifeLevelText.text = "01";
        this.lifeLevelSliderValue = lifeLevelSliderValue;
        this.lifeLevel = lifeLevel;
        this.lifeLvlLimit = lifeLvlLimit;
        this.friendlyShieldData = friendlyShieldData;
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
                buildProperBullets();
            } else {
                lifeLevelSliderValue = 1;
            }
        }
        lifeLevelSlider.value = lifeLevelSliderValue;
    }

    // Slider Value Decreaser
    public void decreaseLifeOfFriendlyShip() {
        lifeLevelSliderValue -= 0.1f;
        if(lifeLevelSliderValue < 0.08f) {
            if(lifeLevel == 1) {
                lifeLevelSlider.value = 0;
                gameOver();
                return;
            }
            if(lifeLevel > 1) {
                lifeLevel--;
                lifeLevelSliderValue = 1;
                lifeLevelTextSetValue();
                buildProperBullets();
            } else {
                lifeLevelSliderValue = 1;
            }
        }
        lifeLevelSlider.value = lifeLevelSliderValue;
    }

    public void performAttack(float damageValue) {
        float damage = damageValue;
        int iterations = (int) (damage/healthPerSubLifeLevel);
        for(int i = 0; i < iterations; i++) {
                decreaseLifeOfFriendlyShip();
            }
        damage = damage%healthPerSubLifeLevel;
        currentHealthForCurrentSubLifeLevel -= damage;
       if(currentHealthForCurrentSubLifeLevel <= 0) {
                currentHealthForCurrentSubLifeLevel += healthPerSubLifeLevel;
                decreaseLifeOfFriendlyShip();
            }
    }

    public void lifeLevelTextSetValue() {
        if(lifeLevel < 10) {
            lifeLevelText.text = "0" + lifeLevel.ToString();
        } else {
            lifeLevelText.text = lifeLevel.ToString();
        }
    }

    public void ShieldInactive() {
        isShieldActive = false;
    }

    // Calls the class that builds friendly bullets
    public void buildProperBullets() {
        gameObject.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel);
    }

    // Ends the Game  <--  Not Yet Complete
    public void gameOver() {
        gameObject.GetComponent<FriendlyBulletBuilder>().stopBuildingBullets();
        GameObject tempGameObject = Instantiate(destroyVFXEffect, new Vector3(0, -100, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        tempGameObject.transform.SetPositionAndRotation(gameObject.transform.position, tempGameObject.GetComponent<DefaultRotation>().Rotation());
        Destroy(gameObject);
    }
}