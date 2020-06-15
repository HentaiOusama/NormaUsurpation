using PathCreation;
using UnityEngine;

public class Test_Script : MonoBehaviour
{
    // Serialized Variables
    public PathCreator pathCreator;
    public float speedOnPath;

    // Non-Serialized Variables
    float distanceTravelled;


    // Update is called once per frame
    void Update()
    {
        distanceTravelled += speedOnPath * Time.deltaTime;
        gameObject.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
    }
}
