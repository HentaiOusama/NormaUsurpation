using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolderDataHub : MonoBehaviour
{
    // Non-Serialized Variables
    GameObject introPath;
    GameObject[] movementPaths;
    int numberOfChildrenShips = -1;
    EnemyWaveBuilder enemyWaveBuilder;

    // Update is called once per frame
    void Update()
    {
        if(numberOfChildrenShips == 0) {
            Destroy(introPath);
            for(int i = 0; i < movementPaths.Length; i++) {
                Destroy(movementPaths[i]);
            }
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

    public void TakePathsForDeletion(EnemyWaveBuilder enemyWaveBuilder, GameObject introPath, GameObject[] movementPaths) {
        this.introPath = introPath;
        this.enemyWaveBuilder = enemyWaveBuilder;
        this.movementPaths = movementPaths;
    }
}