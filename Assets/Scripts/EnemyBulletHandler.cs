using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    // Serialized Variables
    public BulletData bulletData;
    public Object impactObject;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletData.getSpeedValue();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "FriendlyShip") {
            other.GetComponent<FriendlyShipDataHub>().performAttack(bulletData.damageValue);
            Instantiate(impactObject, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
