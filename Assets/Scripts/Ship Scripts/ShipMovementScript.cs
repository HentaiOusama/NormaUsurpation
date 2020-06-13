using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementScript : MonoBehaviour {

    // Non-Serialized Variables
    bool dataGiven = false;
    Transform currentFriendlyShipTransform;
    LBRTValues LBRTPositionConstrains; // It's  Position extremities allowed for ship's transform (details in TakeData Method)
    LBRTValues LBRTShipTouchConstrains; // Distances around Ship for touch
    float heightOffset;
    int count;
    Vector2 touchLocationOnScreen;
    Vector3 touchLocationInWorld;



    // For Getting and Setting relevent data from Main Script
    public void TakeData(Transform transform, LBRTValues PositionConstrains, Vector3 currentScale, SizeData shipSizeData, float extraHorizontalPosition, 
                        float extraVerticalPositionBottom, float extraVerticalPositionTop, float heightOffset, float percentHeightAllowedForMovement) {

        currentFriendlyShipTransform = transform;
        LBRTPositionConstrains = PositionConstrains; // Initially it's visible screen LBRT constrains
        float leftRight = (shipSizeData.occupiedDistance.x/shipSizeData.referenceScale.x)*currentScale.x/2 + extraHorizontalPosition;
        float bottom = (shipSizeData.occupiedDistance.z/shipSizeData.referenceScale.z)*currentScale.z/2 + extraVerticalPositionBottom;
        float top = (shipSizeData.occupiedDistance.z/shipSizeData.referenceScale.z)*currentScale.z/2 + extraVerticalPositionTop;
        LBRTShipTouchConstrains = new LBRTValues(leftRight, bottom, leftRight, top);
        this.heightOffset = heightOffset;
        float maximumTopPositionAllowed = LBRTPositionConstrains.bottom + ((2*LBRTPositionConstrains.top*percentHeightAllowedForMovement)/100);
        float minimumBottomPositionAllowed = LBRTPositionConstrains.bottom + ((shipSizeData.referenceScale.z/shipSizeData.occupiedDistance.z)*currentScale.z/2);
        float minimumLeftPositionAllowed = LBRTPositionConstrains.left;
        float maximumRightPositionAllowed = LBRTPositionConstrains.right;
        // Now LBRTPositionConstrains holds values of extremities of transform of ship which are allowed
        LBRTPositionConstrains = new LBRTValues(minimumLeftPositionAllowed, minimumBottomPositionAllowed, maximumRightPositionAllowed, maximumTopPositionAllowed);
        dataGiven = true;
    }



    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {

        if(dataGiven) {
            count = Input.touchCount;
            if(count > 0) {
                touchLocationOnScreen = Input.GetTouch(0).position;
                touchLocationInWorld = Camera.main.ScreenToWorldPoint(touchLocationOnScreen);
                if(isTouchWithinConstrains()) {

                    if(touchLocationInWorld.z > LBRTPositionConstrains.top) {
                        touchLocationInWorld.z = LBRTPositionConstrains.top;
                    }
                    if(touchLocationInWorld.z < LBRTPositionConstrains.bottom) {
                        touchLocationInWorld.z = LBRTPositionConstrains.bottom;
                    }
                    if(touchLocationInWorld.x > LBRTPositionConstrains.right) {
                        touchLocationInWorld.x = LBRTPositionConstrains.right;
                    }
                    if(touchLocationInWorld.x < LBRTPositionConstrains.left) {
                        touchLocationInWorld.x = LBRTPositionConstrains.left;
                    }


                    touchLocationInWorld = new Vector3(touchLocationInWorld.x, 0, touchLocationInWorld.z + heightOffset);
                    currentFriendlyShipTransform.SetPositionAndRotation(touchLocationInWorld, currentFriendlyShipTransform.rotation);
                }
            }
        }
    }

    public bool isTouchWithinConstrains () {
        bool isWithinLimits = false;

        if ((touchLocationInWorld.x <= (currentFriendlyShipTransform.position.x + LBRTShipTouchConstrains.right)) && 
            (touchLocationInWorld.x >= (currentFriendlyShipTransform.position.x - LBRTShipTouchConstrains.left)) &&
            (touchLocationInWorld.z <= (currentFriendlyShipTransform.position.z + LBRTShipTouchConstrains.top)) &&
            (touchLocationInWorld.z >= (currentFriendlyShipTransform.position.z - LBRTShipTouchConstrains.bottom))) {
                isWithinLimits = true;
            }

        return isWithinLimits;
    }


    // Method for Debuging
    public void logTouchPositions(Vector2 touchLocationOnScreen, Vector3 touchLocationInWorld) {
        Debug.Log("Position Testing");
        Debug.Log("touchLocationOnScreen (x, y) = " + touchLocationOnScreen.x + ", " + touchLocationOnScreen.y);
        Debug.Log("WorldTouchPoint (x, y, z) = " + touchLocationInWorld.x + ", " + touchLocationInWorld.y + ", " + touchLocationInWorld.z);
    }

    public void logTouchPositions(Vector3 touchLocationInWorld) {
        Debug.Log("WorldTouchPoint (x, y, z) = " + touchLocationInWorld.x + ", " + touchLocationInWorld.y + ", " + touchLocationInWorld.z);
    }
}