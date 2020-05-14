using System.Collections;
using UnityEngine;

public class BulletBuilder : MonoBehaviour
{
    public Object bulletObject;
    public int bulletWaitIntVal = 0;
    static int lifeLevel = 0;
    static Transform[] spawnPoints = null;
    static bool shouldBuildBullets = false;



    // Non-Serialized Variables
    Quaternion tempQuat;
    int val = 0;



    // Update is called once per frame
    void Update()
    {
        if(shouldBuildBullets && (lifeLevel > 0) && (val == 0)) {

            val = bulletWaitIntVal;

            switch(lifeLevel) {
                case 1:
                    Instantiate(bulletObject, spawnPoints[0].position, spawnPoints[0].rotation);
                    break;

                case 2:
                    Instantiate(bulletObject, spawnPoints[1].position, spawnPoints[1].rotation);
                    Instantiate(bulletObject, spawnPoints[2].position, spawnPoints[2].rotation);
                    break;

                 case 3:
                    Instantiate(bulletObject, spawnPoints[0].position, spawnPoints[0].rotation);
                    Instantiate(bulletObject, spawnPoints[1].position, spawnPoints[1].rotation);
                    Instantiate(bulletObject, spawnPoints[2].position, spawnPoints[2].rotation);
                    break;

                case 4:
                    lifeLevel = 3;
                    break;

                case 5:
                    Instantiate(bulletObject, spawnPoints[0].position, spawnPoints[0].rotation);
                    Instantiate(bulletObject, spawnPoints[1].position, spawnPoints[1].rotation);
                    Instantiate(bulletObject, spawnPoints[2].position, spawnPoints[2].rotation);
                    Instantiate(bulletObject, spawnPoints[3].position, spawnPoints[3].rotation);
                    Instantiate(bulletObject, spawnPoints[4].position, spawnPoints[4].rotation);
                    break;
            }
        } else {
            val--;
        }

        if(val < 0) {
            val = 0;
        }
    }



// Method that other class calls to give data regarding what type of bullet to build
    public static void buildBullet(int lifeLevel_, Transform[] spawnPoints_) {
        lifeLevel = lifeLevel_;
        spawnPoints = new Transform[spawnPoints_.Length];

        for(int i = 0; i < spawnPoints.Length; i++) {
            spawnPoints[i] = spawnPoints_[i];
        }
    }


    public static void stopBuildingBullets() {
        shouldBuildBullets = false;
    }
    public static void startBuildingBullets() {
        shouldBuildBullets = true;
    }


    // Wait for seconds
    IEnumerator waiter(float time)
    {
        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(time);
    }
}