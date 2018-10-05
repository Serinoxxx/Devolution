using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class RandomWallEnabler : MonoBehaviour {

    [SerializeField] List<GameObject> WallContainers; //These gameobjects will hold the invactive wall gameobjects

    private void Start()
    {
        ActivateRandomWalls();
    }

    private void ActivateRandomWalls()
    {
        foreach (GameObject wallContainer in WallContainers)
        {
            int wallIndex = Random.Range(0, wallContainer.transform.childCount);
            wallContainer.transform.GetChild(wallIndex).gameObject.SetActive(true);

            Transform spawnPoint = wallContainer.transform.GetChild(wallIndex).transform.Find("spawnPoint");
            if (spawnPoint != null)
            {
                GameController.instance.arenaSpawnManager.AddSpawnPoint(spawnPoint);
            }
        }
    }
}
