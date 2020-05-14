using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchConstrains {
    public float bottom, left, top, right;

    public TouchConstrains (float bottom, float left, float top, float right) {
        this.bottom = bottom;
        this.left = left;
        this.top = top;
        this.right = right;
    }

}

[System.Serializable]
public class ValueConstrains {
    public float bottom, left, top, right;

    public ValueConstrains (float bottom, float left, float top, float right) {
        this.bottom = bottom;
        this.left = left;
        this.top = top;
        this.right = right;
    }

}

public class ShipMovementScript : MonoBehaviour {

    
    public Transform currentFriendlyShipTransform;
    public ValueConstrains BLTRValueConstrains;
    public float percentageWidthForTouch, percentageHeightForTouch; // For Screen
    public float AllowedHorizontalTouchWidth, AllowedVerticalTouchWidth; // W.R.T. Ship
    public float heightOffset, touchHeightOffset;

    // Non-Serialized Variables
    float ScreenWidth, ScreenHeight;
    TouchConstrains BLTRTouchConstrains;

    // Start is called before the first frame update
    void Start() {
        ScreenWidth =  Screen.width/2;
        ScreenHeight = Screen.height;
        float left = BLTRValueConstrains.left - (((100 - percentageWidthForTouch)/100) * BLTRValueConstrains.left);
        float right = -left;
        float top = percentageHeightForTouch * (BLTRValueConstrains.top) / 100;
        BLTRTouchConstrains = new TouchConstrains(0, left, top, right);
    }

    // Update is called once per frame
    void FixedUpdate() {
        int count = Input.touchCount;
        if(count > 0) {
            Vector2 touchLocation = Input.GetTouch(0).position;
            touchLocation.x = (touchLocation.x - ScreenWidth) * (BLTRValueConstrains.right/ScreenWidth);
            touchLocation.y = (touchLocation.y * BLTRValueConstrains.top / ScreenHeight);

            if(touchWithinConstrains(touchLocation, BLTRTouchConstrains)) {

                if(validTouch(touchLocation, BLTRTouchConstrains, BLTRValueConstrains)) {

                    if(touchLocation.y > BLTRTouchConstrains.top) {
                        touchLocation.y = BLTRTouchConstrains.top;
                    }

                    currentFriendlyShipTransform.SetPositionAndRotation(new Vector3(touchLocation.x, 0, (touchLocation.y - heightOffset)),
                    currentFriendlyShipTransform.rotation);
                }
            }
        }
    }

    public bool touchWithinConstrains (Vector2 touch, TouchConstrains BLTR) {
        bool isWithinLimits = false;

        if (touch.y <= (BLTR.top + touchHeightOffset) && 
            touch.y >= (BLTR.bottom) && 
            touch.x <= (BLTR.right) && 
            touch.x >= (BLTR.left)) {
                isWithinLimits = true;
            }

        return isWithinLimits;
    }

    public bool validTouch (Vector2 touch, TouchConstrains TBLTR, ValueConstrains BLTR) {
        bool isValid = false;

        float currentHorizontal = currentFriendlyShipTransform.position.x; // Allowed is +- 0.75
        float currentVertical = currentFriendlyShipTransform.position.z;


        if (touch.x <= (currentHorizontal + AllowedHorizontalTouchWidth) && 
            touch.x >= (currentHorizontal - AllowedHorizontalTouchWidth) && 
            touch.y <= (currentVertical + AllowedVerticalTouchWidth) && 
            touch.y >= (currentVertical - 0.01f)) {
                isValid = true;
            }

        return isValid;
    }
}