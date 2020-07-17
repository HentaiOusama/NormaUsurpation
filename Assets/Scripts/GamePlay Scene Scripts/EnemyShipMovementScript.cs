using PathCreation;
using UnityEngine;

public class EnemyShipMovementScript : MonoBehaviour
{
    /*Details :-
    * 
    * Type = 1 => A to B Introduction and Oscillate at given MovementPath.
    * Type = 2 => No Intro or MovementPaths. Initial Velocity is given and they keep colliding inside visible area.
    */




    // Non-Serialized Variables
    bool shouldMoveEnemies = false;
    bool introStarted = false;
    bool shouldIntroduce = false;
    int movementType;
    PathCreator introductoryPath;
    PathCreator[] movementPaths;
    float waitBeforeIntroStart;
    float introSpeed;
    float remainingIntroDuration;
    float movementSpeed;
    minMaxVariable minMaxMovementSpeed;
    Vector3 velocityDirection;
    float distanceTravelled = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shouldMoveEnemies) {
            switch (movementType) {
                case 1:
                    moveType1();
                    break;
                
                case 2:
                    moveType2();
                    break;

            }
        }
    }

    ////////////////////////////////////////////////////////////////////////// Fetches data for moving enemies
    // For MovementType = 1
    public void startMovingEnemies(int movementType, GameObject introductoryPath, GameObject[] movementPaths, 
                                float introSpeed, float movementSpeed) {

        if(introductoryPath != null) {
            this.introductoryPath = introductoryPath.GetComponent<PathCreator>();
        }
        if(movementPaths != null) {
            this.movementPaths = new PathCreator[movementPaths.Length];
            for(int i = 0; i < movementPaths.Length; i++) {
                this.movementPaths[i] = movementPaths[i].GetComponent<PathCreator>();
            }
        }
        this.introSpeed = introSpeed;
        this.movementSpeed = movementSpeed;
        this.movementType = movementType;
        shouldIntroduce = true;
        shouldMoveEnemies = true;
    }
    
    // For MovementType = 2
    public void startMovingEnemies(int movementType, Vector3 velocityDirection, float waitBeforeIntroStart, float introSpeed, 
                                float remainingIntroDuration, minMaxVariable minMaxMovementSpeed) {
        
        this.velocityDirection = velocityDirection;
        this.waitBeforeIntroStart = waitBeforeIntroStart;
        this.introSpeed = introSpeed;
        this.remainingIntroDuration = remainingIntroDuration;
        this.minMaxMovementSpeed = minMaxMovementSpeed;
        this.movementType = movementType;
        introStarted = false;
        shouldIntroduce = true;
        shouldMoveEnemies = true;
    }
    ////////////////////////////////////////////////////////////////////////



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

    void moveType2() {
        if(shouldIntroduce) {
            if(waitBeforeIntroStart > 0) {
                waitBeforeIntroStart -= Time.deltaTime;
            } else if(!introStarted) {
                gameObject.GetComponent<Rigidbody>().velocity = velocityDirection * introSpeed;
                introStarted = true;
            } else {
                if(remainingIntroDuration > 0) {
                    remainingIntroDuration -= Time.deltaTime;
                } else {
                    remainingIntroDuration = 0;
                    shouldIntroduce = false;
                    gameObject.GetComponent<EnemyHolderDataHub>().introComplete();
                }
            }
            return;
        }
    }


    // Rebounce Handler with In_Game_EnemyRebouncer
}
