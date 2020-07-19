using PathCreation;
using UnityEngine;

public class Test_Script : MonoBehaviour
{
    // Serialized Variables

    // Non-Serialized Variables
    bool start = true;


    // Update is called once per frame
    void Update()
    {
        if(start) {
            Debug.Log("Update Working");
            start = false;
        }
    }
}
