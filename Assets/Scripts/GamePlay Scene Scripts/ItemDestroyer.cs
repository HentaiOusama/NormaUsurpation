using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    public string[] tagsOfObjectsNotToDestroy;

    bool shouldDestroy = true;

    private void OnTriggerExit(Collider other) {
        shouldDestroy = true;
        for(int i = 0; i < tagsOfObjectsNotToDestroy.Length; i++) {
            if(other.tag == tagsOfObjectsNotToDestroy[i]) {
                shouldDestroy = false;
            }
        }

        if(shouldDestroy) {
            Destroy(other.gameObject);
            if(other.tag != "FriendlyBullet" && other.tag != "EnemyBullet") {
                Debug.Log("Destroyed Item With Tag : " + other.tag);
            }
        }
    }
}