using UnityEngine;

public class FriendlyBulletBuilder : MonoBehaviour
{
    // Serialized Variables
    public GameObject[] bulletSpawnPoints;
    public Object friendlyBulletObject;
    public float secondsBeforeNextBullet;

    // Non-Serialized Variables
    int lifeLevel = 0;
    bool shouldBuildBullets = false;
    float elapsedTimeAfterlastBullet = 0;


    // Update is called once per frame
    void Update()
    {
        elapsedTimeAfterlastBullet += Time.deltaTime;
        if(shouldBuildBullets && (lifeLevel > 0) && (elapsedTimeAfterlastBullet >= secondsBeforeNextBullet)) {
            elapsedTimeAfterlastBullet = 0;

            try {
                switch(lifeLevel) {
                    case 1:
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[0].transform.position, bulletSpawnPoints[0].transform.rotation);
                        break;

                    case 2:
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[1].transform.position, bulletSpawnPoints[1].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[2].transform.position, bulletSpawnPoints[2].transform.rotation);
                        break;

                    case 3:
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[0].transform.position, bulletSpawnPoints[0].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[1].transform.position, bulletSpawnPoints[1].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[2].transform.position, bulletSpawnPoints[2].transform.rotation);
                        break;

                    case 4:
                        lifeLevel = 3;
                        break;

                    case 5:
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[0].transform.position, bulletSpawnPoints[0].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[1].transform.position, bulletSpawnPoints[1].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[2].transform.position, bulletSpawnPoints[2].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[3].transform.position, bulletSpawnPoints[3].transform.rotation);
                        Instantiate(friendlyBulletObject, bulletSpawnPoints[4].transform.position, bulletSpawnPoints[4].transform.rotation);
                        break;
                }
            } catch (System.Exception e) {
                shouldBuildBullets = false;
                bulletSpawnPoints = null;
                lifeLevel = 0;
                Debug.LogError("Stopped Building Bullets due to error : " + e.Message);
            }
        }
    }



    // Method that other class calls to give data regarding what type of bullet to build
    public void buildFriendlyBullets(int lifeLevel) {
        this.lifeLevel = lifeLevel;
        elapsedTimeAfterlastBullet = secondsBeforeNextBullet;
        shouldBuildBullets = true;
    }


    public void stopBuildingBullets() {
        shouldBuildBullets = false;
    }
}