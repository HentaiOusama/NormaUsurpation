using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolderDataHub : MonoBehaviour
{
    // Serialized Variables
    public BoxCollider holderRebouncer;

    // Non-Serialized Variables
    GameObject introPath;
    GameObject[] movementPaths;
    int numberOfChildrenShips = -1;
    EnemyWaveBuilder enemyWaveBuilder;
    int waveLevel = -1;
    bool isIntroComplete = false;
    bool isRebounding = false;

    void Start() {
        holderRebouncer.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfChildrenShips == 0) {
            numberOfChildrenShips = -1;
            if(introPath != null) {
                Destroy(introPath);
            }
            if(movementPaths != null) {
                for(int i = 0; i < movementPaths.Length; i++) {
                    Destroy(movementPaths[i]);
                }
            }
            Debug.Log("Holder Destroyed... WaveLevel = " + waveLevel);
            enemyWaveBuilder.allChilderEnemiesDefeated();
            Destroy(gameObject);
        }        
    }

    public void increaseChildrenShipsBy(int numberOfNewShips) {
        if(numberOfChildrenShips == -1) {
            numberOfChildrenShips = numberOfNewShips;
        } else {
            numberOfChildrenShips += numberOfNewShips;
        }
    }

    public void decreaseChildrenShipsBy(int numberOfShipsToDecrease) {
        numberOfChildrenShips -= numberOfShipsToDecrease;
        if(numberOfChildrenShips < 0) {
            numberOfChildrenShips = 0;
        }
    }

    public void TakePathsForDeletion(EnemyWaveBuilder enemyWaveBuilder, int waveLevel, GameObject introPath, GameObject[] movementPaths) {
        this.waveLevel = waveLevel;
        this.introPath = introPath;
        this.enemyWaveBuilder = enemyWaveBuilder;
        this.movementPaths = movementPaths;
    }

    public void introComplete() {
        if(isRebounding) {
            holderRebouncer.isTrigger = false;
        }
        isIntroComplete = true;
    }

    public bool IsIntroComplete() {
        return isIntroComplete;
    }

    public void enableRebouncingCollider(Vector3 colliderSize) {
        holderRebouncer.isTrigger = true;
        holderRebouncer.size = colliderSize;
        isRebounding = true;
    }
}