using UnityEngine;
using UnityEngine.UI;

public class FriendlyShieldHandler : MonoBehaviour
{
    // Serialized Variables
    public Slider durationSliderRight, durationSliderLeft;

    // Non-Serialized Variables
    FriendlyShipDataHub friendlyShipDataHub;
    FriendlyShieldData friendlyShieldData;
    float elapsedTime = 0f;
    float sliderValue;

    void Update() {
        gameObject.transform.position = new Vector3(friendlyShieldData.friendlyShipTransform.position.x, 0, friendlyShieldData.friendlyShipTransform.position.z + 
                                                    (friendlyShieldData.friendlyShipHeight/2) + friendlyShieldData.gap);
                                                    
        
        elapsedTime += Time.deltaTime;
        sliderValue = (friendlyShieldData.shieldDuration - elapsedTime)/friendlyShieldData.shieldDuration;
        sliderValue = Mathf.Clamp(sliderValue, 0, 1);
        durationSliderRight.value = sliderValue;
        durationSliderLeft.value = sliderValue;

        if(elapsedTime >= friendlyShieldData.shieldDuration) {
            friendlyShipDataHub.ShieldInactive();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "EnemyBullet") {
            Destroy(other.gameObject);
        }
    }

    public void TakeData(FriendlyShipDataHub friendlyShipDataHub, FriendlyShieldData friendlyShieldData) {
        this.friendlyShipDataHub = friendlyShipDataHub;
        this.friendlyShieldData = friendlyShieldData;
    }

    public void ResetDuration() {
            elapsedTime = 0f;
    }
}
