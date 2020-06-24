using UnityEngine.UI;
using UnityEngine;

public class EnemyShipDataHub : MonoBehaviour
{
    // Serialized Variables
    public float maxLife;


    // Non-Serialized Variables
    GameObject myEnemyHolder = null;
    Slider enemyHealthSlider;
    float currentEnemyHealth;
    float damageRecieved;


    // Start is called before the first frame update
    void Start() {
        currentEnemyHealth = maxLife;
        enemyHealthSlider = gameObject.GetComponentInChildren<Slider>();
        enemyHealthSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTriggerEnter is called when collision starts
    private void OnTriggerEnter(Collider other) {
       if(myEnemyHolder != null) {
           damageRecieved = other.GetComponent<BulletHandler>().bulletData.getDamageValue();
            Destroy(other.gameObject);
           if(myEnemyHolder.GetComponent<EnemyHolderDataHub>().IsIntroComplete() && other.tag == "FriendlyBullet") {
                decreaseEnemyLife(damageRecieved);
            }
        }
   }


   void decreaseEnemyLife(float damage) {
       currentEnemyHealth -= damage;
       enemyHealthSlider.value = (Mathf.Clamp(currentEnemyHealth, 0, maxLife))/(maxLife);
       if(currentEnemyHealth <= 0) {
           ItemDropper.DropItem(gameObject.transform.position, 0.15f);
           gameObject.transform.parent.GetComponent<EnemyHolderDataHub>().decreaseChildrenShipsBy(1);
           Destroy(gameObject);
       }
   }

    public void TakeHolder(GameObject myEnemyHolder) {
        this.myEnemyHolder = myEnemyHolder;
    }
}
