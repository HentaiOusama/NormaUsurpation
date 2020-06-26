using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    /* Index :- 
       0 => Life
       1 => Armor
    */
    public Object[] dropItemObjects;
    public float movementVelocity;
    static Vector3 positionOfEnemyShip;
    static int dropItemIndex = -1, dropItemObjectsLength;

    public static void DropItem(Vector3 position, float probability) {
        if(Random.Range(1, 101) <= (int) (probability * 100)) {
            dropItemIndex = Random.Range(0, dropItemObjectsLength);
            positionOfEnemyShip = position;
        }
    }

    void Start() {
        dropItemObjectsLength = dropItemObjects.Length;
    }

    void Update() {
        if(dropItemIndex != -1) {
            GameObject tempObject = Instantiate(dropItemObjects[dropItemIndex], positionOfEnemyShip, Quaternion.Euler(0, 180, 0)) as GameObject;
            tempObject.transform.localScale = tempObject.GetComponent<SizeData>().defaultScaleForUse;
            tempObject.GetComponent<Rigidbody>().velocity = movementVelocity * tempObject.transform.forward;
            dropItemIndex = -1;
        }
    }
}