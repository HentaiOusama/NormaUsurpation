using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    // Serialized Variables
    public BulletData bulletData;
    public Object muzzelFlare;
    public float muzzelFlareScaleMultiplier;
    public Object impactObject;
    public float impactObjectScaleMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        if(muzzelFlare != null) {
            GameObject flare = Instantiate(muzzelFlare, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            flare.transform.localScale = gameObject.transform.localScale*muzzelFlareScaleMultiplier;
        }
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletData.getSpeedValue();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "FriendlyShip") {
            other.GetComponent<FriendlyShipDataHub>().performAttack(bulletData.damageValue);
            GameObject impact = Instantiate(impactObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            impact.transform.localScale = gameObject.transform.localScale*impactObjectScaleMultiplier;
            Destroy(gameObject);
        }
    }
}
