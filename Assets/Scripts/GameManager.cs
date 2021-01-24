using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<GameObject> allPlayers= new List<GameObject>();
    public Transform[] spawnPos;
    public GameObject enemy;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        foreach (var item in spawnPos)
        {
            Instantiate(enemy, item.position, Quaternion.identity);
        }
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        int randomPoint = Random.Range(0, spawnPos.Length);
        Instantiate(enemy, spawnPos[randomPoint].position, Quaternion.identity);
    }

}
