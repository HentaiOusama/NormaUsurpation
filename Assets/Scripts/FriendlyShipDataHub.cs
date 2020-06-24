using UnityEngine;
using UnityEngine.UI;

public class FriendlyShipDataHub : MonoBehaviour
{
    // Serialized variables
    public float healthPerSubLifeLevel;

    // Non-Serialized variables
    Slider lifeLevelSlider;
    Canvas lifeBarCanvas;
    float lifeLevelSliderValue;
    int lifeLevel;
    int lifeLvlLimit;
    float currentHealthForCurrentSubLifeLevel;
    Text lifeLevelText;
    CurrentBulletData currentBulletData;
    FriendlyShieldData friendlyShieldData;
    GameObject shieldGameObject;
    bool isShieldActive = false;



    // Start is called before the first frame update
    void Start()
    {
        
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
        else if(colliderTag == "EnemyBullet") 
        {
            float damage = other.gameObject.GetComponent<BulletHandler>().bulletData.getDamageValue();
            for(int i = 0; i < (int) damage/healthPerSubLifeLevel; i++) {
                decreaseLifeOfFriendlyShip();
            }
            damage = damage%healthPerSubLifeLevel;
            currentHealthForCurrentSubLifeLevel -= damage;
            if(currentHealthForCurrentSubLifeLevel <= 0) {
                currentHealthForCurrentSubLifeLevel += healthPerSubLifeLevel;
                decreaseLifeOfFriendlyShip();
            }
        }

        Destroy(other.gameObject);
    }



    // Fetches data from Main Script
    public void TakeData(Canvas lifeBarCanvas, float lifeLevelSliderValue, int lifeLevel, int lifeLvlLimit, CurrentBulletData currentBulletData, 
                        FriendlyShieldData friendlyShieldData) {
        lifeLevelSlider = lifeBarCanvas.GetComponentInChildren<Slider>();
        lifeLevelText = lifeBarCanvas.GetComponentInChildren<Text>();
        lifeLevelSlider.value = lifeLevelSliderValue;
        lifeLevelText.text = "01";
        this.lifeLevelSliderValue = lifeLevelSliderValue;
        this.lifeLevel = lifeLevel;
        this.lifeLvlLimit = lifeLvlLimit;
        this.currentBulletData = currentBulletData;
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
                buildProperBullets();
            } else {
                lifeLevelSliderValue = 1;
            }
        }
        lifeLevelSlider.value = lifeLevelSliderValue;
    }

    public void ShieldInactive() {
        isShieldActive = false;
    }

    // Calls the class that builds friendly bullets
    public void buildProperBullets() {
        gameObject.GetComponent<FriendlyBulletBuilder>().buildFriendlyBullets(lifeLevel, currentBulletData.getSpwanPoints());
    }

    // Ends the Game  <-- Not Yet Complete
    public void gameOver() {
        FriendlyBulletBuilder.stopBuildingBullets();
        Destroy(gameObject);
    }
}