using UnityEngine.UI;
using UnityEngine;

public class EnemyShipDataHub : MonoBehaviour
{
    // Serialized Variables
    public Object destroyVFXEffect;
    public float maxLife;
    public bool shouldEnableRebouncer;


    // Non-Serialized Variables
    [HideInInspector]
    public GameObject myEnemyHolder = null;
    Slider enemyHealthSlider;
    float currentEnemyHealth;
    float damageRecieved;
    bool bulletsBeingBuilt = false;


    // Start is called before the first frame update
    void Start() {
        currentEnemyHealth = maxLife;
        enemyHealthSlider = gameObject.GetComponentInChildren<Slider>();
        enemyHealthSlider.value = 1;
    }

    // Update is called once per frame
    void Update(){
        if(bulletsBeingBuilt == false) {
            if(myEnemyHolder.GetComponent<EnemyHolderDataHub>().IsIntroComplete()) {
                gameObject.GetComponent<EnemyBulletBuilder>().buildEnemyBullets();
                bulletsBeingBuilt = true;
            }
        }
    }

    // OnTriggerEnter is called when collision starts
    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "FriendlyBullet") {
            if(myEnemyHolder != null) {
                damageRecieved = other.GetComponent<FriendlyBulletHandler>().bulletData.getDamageValue();
                other.GetComponent<FriendlyBulletHandler>().DestroyBullet();
                if(myEnemyHolder.GetComponent<EnemyHolderDataHub>().IsIntroComplete()) {
                    decreaseEnemyLife(damageRecieved);
                }
            } else {
                Destroy(other.gameObject);
            }
        }
   }


   void decreaseEnemyLife(float damage) {
       currentEnemyHealth -= damage;
       enemyHealthSlider.value = (Mathf.Clamp(currentEnemyHealth, 0, maxLife))/(maxLife);
       if(currentEnemyHealth <= 0) {
           ItemDropper.DropItem(gameObject.transform.position, 0.3f);
           gameObject.transform.parent.GetComponent<EnemyHolderDataHub>().decreaseChildrenShipsBy(1);
           Instantiate(destroyVFXEffect, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
           Destroy(gameObject);
       }
   }

    public void TakeHolder(GameObject myEnemyHolder) {
        this.myEnemyHolder = myEnemyHolder;
    }
}