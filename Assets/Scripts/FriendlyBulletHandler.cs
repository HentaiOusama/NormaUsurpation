using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBulletHandler : MonoBehaviour
{

    // Serialized Variables
    public BulletData bulletData;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletData.getSpeedValue();
    }

    // Not yet complete
    public void DestroyBullet() {
        Destroy(gameObject);
    }
}