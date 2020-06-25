using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class minMaxVariable {
    public float min;
    public float max;
}

public class EnemyBulletBuilder : MonoBehaviour
{
    // Serialized Variables
    public string enemyType;
    public GameObject[] bulletSpawnPoints;
    public Object[] enemyBulletObject;
    public minMaxVariable bulletGapInSec;
    public float probabilityOfPrimaryBulletBuild;

    // Non-Serialized Variables
    bool shouldBuildBullets = false;
    float currentSecondsBeforeNextPrimaryBullet = 0;
    float elapsedTimeAfterlastBullet = 0;


    void Start() {
        currentSecondsBeforeNextPrimaryBullet = Random.Range(bulletGapInSec.min, bulletGapInSec.max);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeAfterlastBullet += Time.deltaTime;
        if(shouldBuildBullets && (elapsedTimeAfterlastBullet >= currentSecondsBeforeNextPrimaryBullet)) {
            elapsedTimeAfterlastBullet = 0;
            if(gameObject.GetComponent<EnemyShipDataHub>().myEnemyHolder != null) {
                if(gameObject.GetComponent<EnemyShipDataHub>().myEnemyHolder.GetComponent<EnemyHolderDataHub>().IsIntroComplete()) {
                    try {
                        switch(enemyType) {
                            case "FusionCore":
                                buildFusionCoreBullets();
                                break;

                            case "NorthStar":
                                buildNorthStarBullets();
                                break;
                            
                        }
                    } catch (System.Exception e) {
                        shouldBuildBullets = false;
                        bulletSpawnPoints = null;
                        Debug.LogError("Stopped Building Bullets due to error : " + e.Message);
                    }
                }
            }
        }
    }


    bool PrimaryInstantiateCheck() {
        currentSecondsBeforeNextPrimaryBullet = Random.Range(bulletGapInSec.min, bulletGapInSec.max);
        if(Random.Range((float)0, (float)1) <= probabilityOfPrimaryBulletBuild) {
            return true;
        } else {
            return false;
        }
    }

    void buildFusionCoreBullets() {
        if(PrimaryInstantiateCheck()) {
            Instantiate(enemyBulletObject[0], bulletSpawnPoints[0].transform.position, bulletSpawnPoints[0].transform.rotation);

        }
    }

    void buildNorthStarBullets() {
        if(PrimaryInstantiateCheck()) {

        }
    }



    // Method that other class calls to give data regarding what type of bullet to build or to stop building bullets
    public void buildEnemyBullets() {
        elapsedTimeAfterlastBullet = currentSecondsBeforeNextPrimaryBullet;
        shouldBuildBullets = true;
    }

    public void stopBuildingBullets() {
        shouldBuildBullets = false;
    }
}