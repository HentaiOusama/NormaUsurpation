using PathCreation;
using UnityEngine;

public class EnemyShipMovementScript : MonoBehaviour
{
    /*Details :-
    * 
    * Type = 1 => A to B Introduction and Oscillate at given MovementPath
    */




    // Non-Serialized Variables
    bool shouldMoveEnemies = false;
    bool shouldIntroduce = false;
    int movementType;
    PathCreator introductoryPath;
    PathCreator[] movementPaths;
    float introSpeed;
    float movementSpeed;
    float distanceTravelled = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldMoveEnemies) {
            switch (movementType) {
                case 1:
                    moveType1();
                    break;

            }
        }
    }

    // Fetches data for moving enemies
    public void startMovingEnemies(int movementType, GameObject introductoryPath, GameObject[] movementPaths, 
                                float introSpeed, float movementSpeed) {

        this.introductoryPath = introductoryPath.GetComponent<PathCreator>();
        this.movementPaths = new PathCreator[movementPaths.Length];
        for(int i = 0; i < movementPaths.Length; i++) {
            this.movementPaths[i] = movementPaths[i].GetComponent<PathCreator>();
        }
        this.introSpeed = introSpeed;
        this.movementSpeed = movementSpeed;
        this.movementType = movementType;
        shouldIntroduce = true;
        shouldMoveEnemies = true;
    }


    // Different Movement Types
    void moveType1() {
        if(shouldIntroduce) {
            distanceTravelled += introSpeed * Time.deltaTime;
            gameObject.transform.position = introductoryPath.path.GetPointAtDistance(distanceTravelled);
            if(distanceTravelled >= introductoryPath.path.length) {
                distanceTravelled = introductoryPath.path.length;
                gameObject.transform.position = introductoryPath.path.GetPointAtDistance(distanceTravelled);
                gameObject.GetComponent<EnemyHolderDataHub>().introComplete();
                shouldIntroduce = false;
                distanceTravelled = 0;
            }
            return;
        }
 
        distanceTravelled += movementSpeed * Time.deltaTime;
        gameObject.transform.position = movementPaths[0].path.GetPointAtDistance(distanceTravelled);
        
    }
}
