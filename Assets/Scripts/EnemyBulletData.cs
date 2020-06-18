using UnityEngine;

public class EnemyBulletData : MonoBehaviour
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