using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShipMovementScript : MonoBehaviour {

    // Non-Serialized Variables
    bool dataGiven = false;
    Vector3 currentPosition;
    LBRTValues LBRTPositionConstrains; // It's  Position extremities allowed for ship's transform (details in TakeData Method)
    Touch firstTouch;
    float drag;
    float deltaX;



    // For Getting and Setting relevent data from Main Script
    public void TakeData(LBRTValues PositionConstrains, Vector3 currentScale, SizeData shipSizeData, float percentHeightAllowedForMovement,
                        float movementDrag) {

        LBRTPositionConstrains = PositionConstrains; // Initially it's visible screen LBRT constrains
        float maximumTopPositionAllowed = LBRTPositionConstrains.bottom + ((2*LBRTPositionConstrains.top*percentHeightAllowedForMovement)/100);
        float minimumBottomPositionAllowed = LBRTPositionConstrains.bottom + ((shipSizeData.referenceScale.z/shipSizeData.occupiedDistance.z)*currentScale.z/2);
        float minimumLeftPositionAllowed = LBRTPositionConstrains.left;
        float maximumRightPositionAllowed = LBRTPositionConstrains.right;
        // Now LBRTPositionConstrains holds values of extremities of transform of ship which are allowed
        LBRTPositionConstrains = new LBRTValues(minimumLeftPositionAllowed, minimumBottomPositionAllowed, maximumRightPositionAllowed, maximumTopPositionAllowed);
        drag = 1/movementDrag;
        dataGiven = true;
    }



    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if(dataGiven) {
            if(Input.touchCount > 0) {
                firstTouch = Input.GetTouch(0);
                currentPosition = gameObject.transform.position;
                currentPosition.x += firstTouch.deltaPosition.x * drag;
                currentPosition.z += firstTouch.deltaPosition.y * drag;
                fixCurrentPosition();
                gameObject.transform.position = currentPosition;
            }
        }
    }

    public void fixCurrentPosition () {
        if(currentPosition.x < LBRTPositionConstrains.left) {
            currentPosition.x = LBRTPositionConstrains.left;
        } else if(currentPosition.x > LBRTPositionConstrains.right) {
            currentPosition.x = LBRTPositionConstrains.right;
        }

        if(currentPosition.z < LBRTPositionConstrains.bottom) {
            currentPosition.z = LBRTPositionConstrains.bottom;
        } else if(currentPosition.z > LBRTPositionConstrains.top) {
            currentPosition.z = LBRTPositionConstrains.top;
        }
    }
}