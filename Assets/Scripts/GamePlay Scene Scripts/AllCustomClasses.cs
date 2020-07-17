using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class minMaxVariable {
    public minMaxVariable(float min, float max) {
        this.min = min;
        this.max = max;
    }
    
    public float min;
    public float max;
}


[System.Serializable]
public class LBRTValues
{
    public float left, bottom, right, top;
    public LBRTValues(float left, float bottom, float right, float top) {
        this.left = left;
        this.bottom = bottom;
        this.right = right;
        this.top = top;
    }
}


[System.Serializable]
public class BackgroundElement {
    public Material material;
    public Vector4 BGFogColorARGB;
    public float width;
    public float height;
}



[System.Serializable]
public class BackgroundData {
    public BackgroundElement[] BGMaterialList;
    public int startBgIndex;
    public float baseBackgroundWidth;
}



[System.Serializable]
public class CanvasData {
    public Object CanvasObject;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 Rotation;
    public float verticalOffset;
}



[System.Serializable]
public class FriendlyShipElement {
    ///// To get the scale, use GetComponent<SizeData>().defaultScaleForUse /////
    public Object FriendlyShipObject;
    public Vector3 Rotation;
    Vector3 Scale;
    Quaternion retRotation;
    public FriendlyShipElement(){
        retRotation = Quaternion.Euler(Rotation);
    }
    public Quaternion getRotation() {
        return retRotation;
    }
}



[System.Serializable]
public class FriendlyShieldData {
    public Object friendlyShieldObject;
    public float shieldDuration;
    [HideInInspector]
    public Transform friendlyShipTransform;
    [HideInInspector]
    public float friendlyShipHeight;
    public float gap;
}



[System.Serializable]
public class FriendlyShipData {
    public FriendlyShipElement[] FSList;
    public FriendlyShieldData friendlyShieldData;
    public int startShipIndex;
    public int currentShipIndex = 0;
    public float FSIntroSpeed;
    public float percentHeightAllowedForMovement;
    public float movementDrag;
    public int lifeLvlLimit;

}



[System.Serializable]
public class CurrentBulletData {
    public GameObject currentBulletObject;
    Transform[] spwanPoints;

    public CurrentBulletData(Transform[] spwanPoints) {
        this.spwanPoints = spwanPoints;
    }

    public Transform[] getSpwanPoints() {
        return spwanPoints;
    }
}



[System.Serializable]
public class BulletData
{
    // Serialized Variables
    public float damageValue;
    public float speedValue;

    public float getDamageValue() {
        return damageValue;
    }

    public float getSpeedValue() {
        return speedValue;
    }
}



[System.Serializable]
public class WaveBuildingData {
    public Object[] enemyObjects;
    public Object[] movementPathObjects;
    public int waveLevel;
    public int maxWaves;
}



public class AllCustomClasses : MonoBehaviour
{
    
}