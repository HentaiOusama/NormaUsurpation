using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public GameObject bullet;
    public float Speed;
    
    // Start is called before the first frame update

    void Start()
    {
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
}