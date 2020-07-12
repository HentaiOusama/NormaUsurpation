using UnityEngine;

[System.Serializable]
public class BeforeShootDetails {
    public bool hasShootAnimation;
    public bool hasShootParticleSystem;
    public Animation[] animationList;
    public float[] respectiveAnimDuration;
    public ParticleSystem[] particleSystemList;
    public float[] respectivePSDuration;
}

public class EnemyBulletBuilder : MonoBehaviour
{
    // Serialized Variables
    public string enemyType;
    public GameObject[] bulletSpawnPoints;
    public Object[] enemyBulletObject;
    public minMaxVariable bulletGapInSec;
    public float probabilityOfPrimaryBulletBuild;
    public BeforeShootDetails beforeShootDetails;

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

                            case "StarViper":
                                buildStarViperBullets();
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
        return (Random.Range((float)0, (float)1) <= probabilityOfPrimaryBulletBuild);
    }

    void buildFusionCoreBullets() {
        if(PrimaryInstantiateCheck()) {
            spwanBullet(0, 0);
        }
    }

    void buildStarViperBullets() {
        if(PrimaryInstantiateCheck()) {
            spwanBullet(0, 1);
            spwanBullet(0, 2);
        }
    }


    // Method that all builders call when instantiating the bullet
    void spwanBullet(int bulletObjIdx, int bulletSpPointIdx) {
        GameObject tempGameObject = Instantiate(enemyBulletObject[bulletObjIdx], bulletSpawnPoints[bulletSpPointIdx].transform.position, 
                                                bulletSpawnPoints[bulletSpPointIdx].transform.rotation) as GameObject;
        tempGameObject.transform.localScale = tempGameObject.GetComponent<SizeData>().defaultScaleForUse;
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