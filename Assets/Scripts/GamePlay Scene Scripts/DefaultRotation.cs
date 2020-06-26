using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultRotation : MonoBehaviour
{
    public Vector3 defaultRotation;

    public Quaternion Rotation() {
        return Quaternion.Euler(defaultRotation);
    }
}