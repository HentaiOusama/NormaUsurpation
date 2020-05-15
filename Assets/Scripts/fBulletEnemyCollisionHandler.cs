using UnityEngine;
using UnityEngine.UI;

public class fBulletEnemyCollisionHandler : MonoBehaviour
{
    public float maxLife;


    // Non-Serialized Variables
    Slider enemyHealthSlider;
    float currentEnemyHealth;
    float damageRecieved;


    // Start is called before the first frame update
    void Start() {
        currentEnemyHealth = maxLife;
        enemyHealthSlider = gameObject.GetComponentInChildren<Slider>();
        enemyHealthSlider.value = 1;
    }

   private void OnTriggerEnter(Collider other) {
       if(other.tag == "FriendlyBullet") {
           damageRecieved = other.GetComponent<FriendlyBulletData>().getDamageValue();
           Destroy(other.gameObject);
           decreaseEnemyLife(damageRecieved);
       }
   }


   void decreaseEnemyLife(float damage) {
       currentEnemyHealth -= damage;
       enemyHealthSlider.value = (Mathf.Clamp(currentEnemyHealth, 0, maxLife))/(maxLife);
       if(currentEnemyHealth <= 0) {
           Destroy(gameObject);
       }
   }
}