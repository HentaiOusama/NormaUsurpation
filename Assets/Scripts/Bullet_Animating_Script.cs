using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Animating_Script : MonoBehaviour
{
    public GameObject gameObj;
    public Material[] materials;
    public int materialsSize;
    int i = 0;
    // Update is called once per frame
    void Update()
    {
        if(i >= materialsSize) {
            i = 0;
        }

        gameObj.GetComponent<MeshRenderer>().material = materials[i];

        i++;
    }
}
