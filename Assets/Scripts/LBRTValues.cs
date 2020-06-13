using UnityEngine;

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