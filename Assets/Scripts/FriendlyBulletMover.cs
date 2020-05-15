using UnityEngine;

public class FriendlyBulletMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * gameObject.GetComponent<FriendlyBulletData>().getSpeedValue();
    }
}